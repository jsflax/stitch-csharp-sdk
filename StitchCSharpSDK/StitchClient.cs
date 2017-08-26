using System;
using System.Net.Http;
using System.Threading.Tasks;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using Stitch.Auth;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Stitch
{
    public class StitchClient
    {
        private volatile AuthInfo _auth;
        public AuthInfo Auth 
        { get { return _auth; } internal set { _auth = value; } }

        private readonly string _appId;
        private readonly string _baseUrl;

        private readonly HttpClient _httpClient = new HttpClient();

        private readonly ConcurrentDictionary<int, IAuthListener> _authListeners =
            new ConcurrentDictionary<int, IAuthListener>();

        public StitchClient(string appId,
                            string baseUrl = "https://stitch.mongodb.com")
        {
            this._appId = appId;
            this._baseUrl = baseUrl;
        }



        #region HTTP Methods
        /// <summary>
        /// Logs the current user in using a specific auth provider.
        /// </summary>
        /// <returns>an Auth session that can be resolved on 
        /// completion of log in</returns>
        /// <param name="authProvider">The provider that will handle the login.
        /// </param>
        public async Task<StitchResult<AuthInfo>> Login(IAuthProvider authProvider)
        {
            return await ExecuteRequest(
                String.Format(
                    "{0}/{1}/{2}",
                    GetResourcePath(Paths.AUTH),
                    authProvider.Type,
                    authProvider.ProviderName
                ),
                GetAuthRequest(authProvider.Payload).ToJson(),
                json => {
                    Auth = json.ToObject<AuthInfo>();
                    Console.WriteLine(Auth);
                    return Auth;
                }
            );
        }

        public async Task<StitchResult<bool>> Register(string email,
                                                       string password)
        {
            var provider = new EmailPasswordAuthProvider(email, password);
            return await ExecuteRequest(
                String.Format(
                    "{0}/{1}/{2}",
                    GetResourcePath(Paths.AUTH),
                    (provider as IAuthProvider).Type,
                    Paths.USERPASS_REGISTER
                ),
                GetAuthRequest(provider.RegistrationPayload).ToJson(),
                json => json != null
            );
        }

        // <summary> Confirm a newly registered email in this context </summary>
        // <param arg="token"> confirmation token emailed to new user </param>
        // <param arg="tokenId"> confirmation tokenId emailed to new user </param>
        // <returns> 
        // A task containing whether or not the email was confirmed successfully
        // </returns>
        public async Task<StitchResult<bool>> EmailConfirm(string token,
                                                           string tokenId)
        {
            return await this.ExecuteRequest(
                String.Format(
                    "{0}/{1}/{2}",
                    GetResourcePath(Paths.AUTH),
                    "",
                    Paths.USERPASS_CONFIRM
                ), new JObject {
                    { "token", token },
                    { "tokenId", tokenId }
                }.ToString(),
                json => json != null
            );
        }

        /// <summary>
        /// Send a confirmation email for a newly registered user
        /// </summary>
        /// <returns>Whether or not the email was sent successfully.</returns>
        /// <param name="email">Email address of user.</param>
        public async Task<StitchResult<bool>> SendEmailConfirm(string email)
        {
            return await this.ExecuteRequest(
                String.Format(
                    "{0}/{1}/{2}",
                    GetResourcePath(Paths.AUTH),
                    "",
                    Paths.USERPASS_CONFIRM_SEND
                ),
                new JObject { { "email", email } }.ToString(),
                json => json != null
            );
        }

        public async Task<StitchResult<bool>> ResetPassword(string token,
                                                           string tokenId)
        {
            return await this.ExecuteRequest(
                String.Format(
                    "{0}/{1}/{2}",
                    this.GetResourcePath(Paths.AUTH),
                    "",
                    Paths.USERPASS_RESET
                ),
                new JObject {
                    { "token", token },
                    { "tokenId", tokenId}
                }.ToString(),
                json => json != null
            );
        }

        public async Task<StitchResult<bool>> SendResetPassword(string email)
        {
            return await this.ExecuteRequest(
                String.Format(
                    "{0}/{1}/{2}",
                    this.GetResourcePath(Paths.AUTH),
                    "",
                    Paths.USERPASS_RESET_SEND
                ),
                new JObject { { "email", email } }.ToString(),
                json => json != null
            );
        }

        public async Task<StitchResult<AvailableAuthProviders>> GetAuthProviders()
        {
            return await this.ExecuteRequest(
                this.GetResourcePath(Paths.AUTH),
                unmarshaller: (json => new AvailableAuthProviders(
                    json.ToBsonDocument().ToDictionary()))
            );
        }
        #endregion
        #region Pipelines

        /// Executes a pipeline with the current app
        /// @param pipeline The pipeline to execute.
        /// return A task containing the result of the pipeline that can be resolved on completion
        /// of the execution
        public async Task<StitchResult<List<BsonValue>>> ExecutePipeline(
            params PipelineStage[] pipeline)
        {
            if (!IsAuthenticated())
            {
                throw new Exception("Must first authenticate");
            }

            var stitchResult = new StitchResult<List<BsonValue>>();
            string pipeStr;
            try
            {
                pipeStr = JsonConvert.SerializeObject(pipeline);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                stitchResult.Error = e;
                return stitchResult;
            }

            return await this.ExecuteRequest(
                GetResourcePath(Paths.PIPELINE),
                pipeStr,
                rawUnmarshaller: (string res) =>
                {
                Console.WriteLine(res);
                    return BsonDocument.Parse(res)["result"].AsBsonArray.ToList();
                }
            );
        }
        #endregion

        private static class Paths
        {
            public const string AUTH = "auth";
            public const string USERPASS_REGISTER = "userpass/register";
            public const string USERPASS_CONFIRM = "local/userpass/confirm";
            public const string USERPASS_CONFIRM_SEND = "local/userpass/confirm/send";
            public const string USERPASS_RESET = "local/userpass/reset";
            public const string USERPASS_RESET_SEND = "local/userpass/reset/send";

            public const string PIPELINE = "pipeline";
        }

        #region Auth Utility Methods
        public void AddAuthListener(IAuthListener authListener)
        {
            this._authListeners[authListener.GetHashCode()] = authListener;
        }

        public bool RemoveAuthListener(IAuthListener authListener)
        {
            return this._authListeners.TryRemove(authListener.GetHashCode(),
                                                 out authListener);
        }

        /// <summary>
        /// Check whether or not the client is currently authenticated 
        /// </summary>
        /// <returns> Whether or not the client is authenticated. </returns>
        public bool IsAuthenticated()
        {
            if (Auth != null)
            {
                return true;
            }

            //Auth = _realm.All<AuthInfo>().GetEnumerator().Current;

            if (Auth != null)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region General Utility Methods

        private String GetResourcePath(string resource)
        {
            return String.Format("{0}/api/client/v1.0/app/{1}/{2}",
                                 _baseUrl,
                                 _appId,
                                 resource);
        }

        /// <summary>
        /// Generates an authenticated request </summary>
        /// <param name="request"> Arbitrary document for authentication 
        /// </param>
        /// <returns>
        /// A Document representing all information required for
        /// an auth request against a specific provider. </returns>
        private BsonDocument GetAuthRequest(BsonDocument request)
        {
            //request.AddRange(
            //    new BsonDocument {
            //        { "options", new BsonDocument {
            //            { "device", "" }
            //        } }
            //    }
            //);

            return request;
        }

        private async Task<StitchResult<T>> ExecuteRequest<T>(
            string url,
            string payload = null,
            Func<JObject, T> unmarshaller = null,
            Func<string, T> rawUnmarshaller = null
        )
        {
            var stitchResult = new StitchResult<T>();
            try
            {
                var request = new HttpRequestMessage(
                    payload == null ? HttpMethod.Get : HttpMethod.Post,
                    url);

                if (IsAuthenticated())
                {
                    Console.WriteLine(Auth.AccessToken);
                    request.Headers.Add("Authorization", "Bearer " + Auth.AccessToken);
                }

                if (payload != null)
                {
                    Console.WriteLine(payload);
					request.Content = new StringContent(payload,
                                                        System.Text.Encoding.UTF8,
                                                        "application/json");
				}

                var response = await _httpClient.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                if (rawUnmarshaller != null)
                {
                    stitchResult.Result = rawUnmarshaller(content);
                }
                else
                {
                    var json = JObject.Parse(content);

                    var error = json["error"];
                    if (error != null)
                    {
                        stitchResult.Error = new Exception(error.ToString());
                    }
                    else
                    {
                        stitchResult.Result = unmarshaller(json);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                stitchResult.Error = e;
            }

            return stitchResult;
        }
        #endregion
    }
}
