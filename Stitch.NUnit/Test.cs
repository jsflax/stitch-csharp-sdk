using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Stitch;
using Stitch.Auth;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Test
{
    public class StitchClient : StitchClientBase
    {
        public StitchClient(string appId,
                            string baseUrl = "https://stitch.mongodb.com")
            : base(appId, baseUrl) {}
        
        protected override DeviceInfo GetDeviceInfo()
        {
            return new DeviceInfo
            {
                AppId = "",
                AppVersion = "",
                Platform = "",
                PlatformVersion = ""
            };
        }  

        protected override string Get(string key, string def = "")
        {
            return "";
        }

        protected override void Put(string key, string val)
        {
        }

        protected override void Remove(string key)
        {
        }

        protected override bool Contains(string key)
        {
            return true;
        }
    }

    [TestFixture()]
    public class Test
    {
        private StitchClient _client = new StitchClient(
            "test-uybga",
            "https://stitch.mongodb.com"
        );

        [Test()]
        public void DeviceTest()
        {
            Console.WriteLine(System.Environment.OSVersion);
        }

        [Test()]
        public async Task PipelineTest()
        {
            await _client.Login(
                new EmailPasswordAuthProvider("test_user@tester.com",
                                              "password")
            );

            Assert.IsNotNull(_client.Auth);
            var doc = new BsonDocument {
                { "name", "foo" },
                {
                    "items", new BsonArray {
                        new BsonDocument {
                            { "adjective", "brown" },
                            { "noun", "cow" }
                        },
                        new BsonDocument {
                            { "adjective", "red" },
                            { "noun", "hen" }
                        }
                    }

                }
            };
            var docs = await _client.ExecutePipeline(
                new PipelineStage("namedPipeline", args: doc)
            );
            Console.WriteLine(docs.Value);
        }

        [Test()]
        public async Task TestAuthProviders()
        {
            var providers = (await _client.GetAuthProviders()).Value;

            Assert.IsNotNull(providers.AnonymousProviderInfo);
            Assert.IsNull(providers.GoogleProviderInfo);

            Console.WriteLine(providers);
        }

        [Test()]
        public async Task TestLogin()
        {
            var isRegistered = await _client.Register("test_user@tester.com",
                                                      "password");

            Console.WriteLine(isRegistered);

            //Assert.IsTrue(isRegistered.Result);

            var auth = await _client.Login(
                new EmailPasswordAuthProvider("test_user@tester.com",
                                              "password")
            );

            Assert.IsNotNull(auth);

            return;
        }

        [Test()]
        public async Task TestAnon()
        {
            var anon = new AnonymousAuthProvider();

            var isLoggedIn = await _client.Login(anon);
            Assert.IsNotNull(_client.Auth);

            Console.WriteLine(isLoggedIn);

            return;
        }
    }
}
