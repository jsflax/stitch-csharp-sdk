using System;
using System.Net.Http;
using System.Threading.Tasks;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using Stitch.Auth;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Collections;
using MongoDB.Bson.Serialization;

namespace Stitch
{
    public abstract class StitchClientBase : KeyValueStore
    {
		private const string DataAuthTokenKey = "auth_token";
        private const string DataRefreshTokenKey = "refresh_token";
        private const string DataDeviceIdKey = "deviceId";

        private volatile AuthInfo _auth;
        public AuthInfo Auth 
        { get { return _auth; } internal set { _auth = value; } }

        private readonly string _appId;
        private readonly string _baseUrl;

        private readonly HttpClient _httpClient = new HttpClient();

        private readonly ConcurrentDictionary<int, IAuthListener> _authListeners =
            new ConcurrentDictionary<int, IAuthListener>();

        public StitchClientBase(string appId,
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
                HttpMethod.Post,
                String.Format(
                    "{0}/{1}/{2}",
                    GetResourcePath(Paths.Auth),
                    authProvider.Type,
                    authProvider.ProviderName
                ),
                GetAuthRequest(authProvider.Payload).ToJson(),
                json =>
                {
                    Auth = json.ToObject<AuthInfo>();
                    var refreshToken = json.ToObject<RefreshTokenHolder>();

                    Put(DataAuthTokenKey, json.ToString());
                    Put(DataDeviceIdKey, Auth.DeviceId);
                    Put(DataRefreshTokenKey, refreshToken.RefreshToken);

                    OnLogin();
                    return Auth;
                }
            );
        }

        /// <summary>
        /// Logout the current user.
        /// </summary>
        /// <returns>Success of logout.</returns>
        public async Task<StitchResult<bool>> Logout()
        {
            if (!IsAuthenticated()) 
            {
                return new StitchResult<bool>(true, true);        
            }

            return await this.ExecuteRequest(
                HttpMethod.Delete,
                GetResourcePath(Paths.Auth),
                null,
                rawUnmarshaller: (arg) => {
                    ClearAuth();
                    return arg != null;
                },
                useRefreshToken: false,
                refreshOnFailure: true
            );
        }

		/// <summary>
		/// Registers the current user using email and password.
		/// </summary>
		/// <returns>A task containing whether or not registration was 
        /// successful.</returns>
		/// <param name="email">Email for the given user</param>
		/// <param name="password">Password for the given user.</param>
		public async Task<StitchResult<bool>> Register(string email,
                                                       string password)
        {
            var provider = new EmailPasswordAuthProvider(email, password);
            return await ExecuteRequest(
                HttpMethod.Post,
                String.Format(
                    "{0}/{1}/{2}",
                    GetResourcePath(Paths.Auth),
                    (provider as IAuthProvider).Type,
                    Paths.UserpassRegister
                ),
                GetAuthRequest(provider.RegistrationPayload).ToJson(),
                json => json != null
            );
        }

        /// <summary> Confirm a newly registered email in this context </summary>
        /// <param arg="token"> confirmation token emailed to new user </param>
        /// <param arg="tokenId"> confirmation tokenId emailed to new user </param>
        /// <returns> 
        /// A task containing whether or not the email was confirmed successfully
        /// </returns>
        public async Task<StitchResult<bool>> EmailConfirm(string token,
                                                           string tokenId)
        {
            return await this.ExecuteRequest(
                HttpMethod.Post,
                String.Format(
                    "{0}/{1}/{2}",
                    GetResourcePath(Paths.Auth),
                    "",
                    Paths.UserpassConfirm
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
                HttpMethod.Post,
                String.Format(
                    "{0}/{1}/{2}",
                    GetResourcePath(Paths.Auth),
                    "",
                    Paths.UserpassConfirmSend
                ),
                new JObject { { "email", email } }.ToString(),
                json => json != null
            );
        }

        public async Task<StitchResult<bool>> ResetPassword(string token,
                                                           string tokenId)
        {
            return await this.ExecuteRequest(
                HttpMethod.Post,
                String.Format(
                    "{0}/{1}/{2}",
                    this.GetResourcePath(Paths.Auth),
                    "",
                    Paths.UserpassReset
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
                HttpMethod.Post,
                String.Format(
                    "{0}/{1}/{2}",
                    this.GetResourcePath(Paths.Auth),
                    "",
                    Paths.UserpassResetSend
                ),
                new JObject { { "email", email } }.ToString(),
                json => json != null
            );
        }

        public async Task<StitchResult<UserProfile>> GetUserProfile()
        {
            if (!IsAuthenticated())
            {
                Console.WriteLine("Must log in before getting user profile");
                return new StitchResult<UserProfile> {
                    Error = new Exception("Must log in before getting user profile")
                };
            }

            return await this.ExecuteRequest(
                HttpMethod.Get,
                GetResourcePath(Paths.UserProfile),
                null,
                json => json.ToObject<UserProfile>()
            );
        }

        public async Task<StitchResult<AvailableAuthProviders>> GetAuthProviders()
        {
            return await this.ExecuteRequest(
                HttpMethod.Get,
                this.GetResourcePath(Paths.Auth),
                unmarshaller: (json => new AvailableAuthProviders(
                    json.ToBsonDocument().ToDictionary()))
            );
        }
		#endregion

		#region Pipelines
		/// <summary>Executes a pipeline with the current app</summary>
		/// <param name="pipeline">The pipeline to execute.</param>
		/// <returns>A task containing the result of the pipeline that can be
		/// resolved on completion of the execution. </returns>
		public async Task<StitchResult<List<BsonDocument>>> ExecutePipeline(
            params PipelineStage[] pipeline)
        {
            if (!IsAuthenticated())
            {
                throw new Exception("Must first authenticate");
            }

            var stitchResult = new StitchResult<List<BsonDocument>>();
            string pipeStr;
            try
            {
                pipeStr = pipeline.ToJson();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                stitchResult.Error = e;
                return stitchResult;
            }

            return await this.ExecuteRequest(
                HttpMethod.Post,
                GetResourcePath(Paths.Pipeline),
                pipeStr,
                rawUnmarshaller: (string res) =>
                {
                    return BsonDocument.Parse(res)["result"]
                                       .AsBsonArray
                                       .ToList()
                                       .ConvertAll(obj => obj.AsBsonDocument);
                }
            );
        }
        #endregion

        private static class Paths
        {
            public const string Auth = "auth";
            public const string NewAccessToken = "auth/newAccessToken";
            public const string UserProfile = "auth/me";

            public const string UserpassRegister = "userpass/register";
            public const string UserpassConfirm = "local/userpass/confirm";
            public const string UserpassConfirmSend = "local/userpass/confirm/send";
            public const string UserpassReset = "local/userpass/reset";
            public const string UserpassResetSend = "local/userpass/reset/send";

            public const string Pipeline = "pipeline";
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
		/// Called when a user logs in with this client.
		/// </summary>
		private void OnLogin()
		{
            foreach (KeyValuePair<int, IAuthListener> listener in _authListeners)
			{
				listener.Value.OnLogin();
			}
		}

		/// <summary>
		/// Called when a user is logged out from this client.
		/// </summary>
		/// <param name="lastProvider">Last provider.</param>
		private void OnLogout(string lastProvider)
		{
			foreach (KeyValuePair<int, IAuthListener> listener in _authListeners)
			{
				listener.Value.OnLogout(lastProvider);
			}
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

            Auth = JsonConvert.DeserializeObject<AuthInfo>(Get(DataAuthTokenKey, 
                                                               ""));

            if (Auth != null)
            {
                return true;
            }

            return false;
        }

		/// <summary>
		/// Clears all authentication material that has been persisted.
		/// </summary>
		private void ClearAuth()
        {
			if (Auth == null)
			{
				return;
			}

			string lastProvider = Auth.Provider;
			Auth = null;
            Remove(DataAuthTokenKey);
            Remove(DataRefreshTokenKey);
			OnLogout(lastProvider);
        }

		/// <summary>
		/// Gets the refresh token.
		/// </summary>
		/// <returns>The refresh token for the current user if authenticated; 
        /// throws otherwise.</returns>
		private String GetRefreshToken()
		{
			if (!IsAuthenticated())
			{
				throw new Exception("Must first authenticate");
			}

			return Get(DataRefreshTokenKey, "");
		}

		/// <summary>
		/// Handles an invalid session error from Stitch by refreshing the 
		/// access token and retrying the original request.
		/// </summary>
		/// <param name="method">The original HTTP method.</param>
		/// <param name="resource">The original resource.</param>
		/// <param name="payload">The original body.</param>
        /// <param name="unmarshaller">The original unmarshaller.</param>
        /// <param name="rawUnmarshaller">The original raw unmarshaller.</param>
		private async Task<StitchResult<T>> HandleInvalidSession<T>(
            HttpMethod method,
            string resource,
			string payload = null,
			Func<JObject, T> unmarshaller = null,
			Func<string, T> rawUnmarshaller = null)
		{
            var result = await RefreshAccessToken();

            if (!result.IsSuccessful)
            {
                return new StitchResult<T>
                {
                    Error = result.Error
                };
            }

            return await this.ExecuteRequest(
                method, 
                resource, 
                payload,
                unmarshaller,
                rawUnmarshaller,
                false
            );
        }

		/// <summary>
		/// Refreshes the current access token using the current refresh token.
		/// </summary>
		private async Task<StitchResult<bool>> RefreshAccessToken()
        {
            return await this.ExecuteRequest(
                HttpMethod.Post,
                GetResourcePath(Paths.NewAccessToken),
                null,
                json =>
                {
                    string newAccessToken = json["accessToken"].ToString();
                    Auth.AccessToken = newAccessToken;

                    Put(DataAuthTokenKey, Auth.ToJson());
                    return new StitchResult<bool>(true, true);
                },
                useRefreshToken: true
            );
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
        /// Check if the device identifier has been stored.
        /// </summary>
        /// <returns><c>true</c>, if device identifier was stored, 
        /// <c>false</c> otherwise.</returns>
        private bool HasDeviceId()
        {
            return Contains(DataDeviceIdKey);
        }

        /// <summary>
        /// Gets the device identifier from the store.
        /// </summary>
        /// <returns>The device identifier.</returns>
        private string GetDeviceId()
        {
            return Get(DataDeviceIdKey);
        }

        /// <summary>
        /// Gets the device info. To be implemented by the platform user.
        /// </summary>
        /// <returns>The device info.</returns>
		protected abstract DeviceInfo GetDeviceInfo();

		/// <summary>
		/// Generates an authenticated request </summary>
		/// <param name="request"> Arbitrary document for authentication 
		/// </param>
		/// <returns>
		/// A Document representing all information required for
		/// an auth request against a specific provider. </returns>
		private BsonDocument GetAuthRequest(BsonDocument request)
        {
            var deviceInfo = this.GetDeviceInfo().ToBsonDocument();

            if (this.HasDeviceId())
            {
                deviceInfo.AddRange(
                    new BsonDocument { { "deviceId", this.GetDeviceId() } }
                );
            }

            request.AddRange(
                new BsonDocument {
                    { "options", new BsonDocument {
                        { "device", deviceInfo }
                    }  }
                }
            );

            return request;
        }

        private async Task<StitchResult<T>> ExecuteRequest<T>(
            HttpMethod httpMethod,
            string url,
            string payload = null,
            Func<JObject, T> unmarshaller = null,
            Func<string, T> rawUnmarshaller = null,
            bool refreshOnFailure = true,
            bool useRefreshToken = false
        ) {
            var stitchResult = new StitchResult<T>();
            try
            {
                var request = new HttpRequestMessage(
                    httpMethod,
                    url);

                if (IsAuthenticated())
                {
                    Console.WriteLine(Auth.AccessToken);
                    request.Headers.Add(
                        "Authorization", 
                        "Bearer " + (useRefreshToken ? 
                                     GetRefreshToken() : Auth.AccessToken));
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

				var json = JObject.Parse(content);

                Console.WriteLine("Request: {0}", payload);
				Console.WriteLine("Response: {0}", content);

				var error = json["error"];
				if (error != null)
				{
					if (json["errorCode"] != null &&
                        json["errorCode"].ToString() == "InvalidSession")
					{
                        Console.Error.Write(error);
						if (!refreshOnFailure)
						{
							ClearAuth();
							stitchResult.Error = new Exception(error.ToString());
							return stitchResult;
						}

						return await HandleInvalidSession(
							httpMethod,
							url,
							payload,
							unmarshaller,
							rawUnmarshaller
						);
					}
					else
					{
						stitchResult.Error = new Exception(error.ToString());
					}
                } 
                else 
                {
					if (rawUnmarshaller != null)
					{
						stitchResult.Value = rawUnmarshaller(content);
					}
					else
					{
						stitchResult.Value = unmarshaller(json);
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
