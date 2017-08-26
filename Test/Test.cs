using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Stitch;
using Stitch.Auth;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.IO;

namespace Test
{
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
                new PipelineStage("namedPipeline", args: doc.ToDictionary())
            );
            Console.WriteLine(docs.Result);
        }

        [Test()]
        public void BSONTest()
        {
            var now = System.DateTime.UtcNow;
            Console.WriteLine(now);
            MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonArray>(bs);
            Console.WriteLine((DateTime.UtcNow - now).TotalMilliseconds);

			now = System.DateTime.UtcNow;
			Console.WriteLine(now);
			JArray.Parse(bs);
			Console.WriteLine((DateTime.UtcNow - now).TotalMilliseconds);
		}

        [Test()]
        public async Task TestAuthProviders()
        {
            var providers = (await _client.GetAuthProviders()).Result;

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

		string bs = @"
        [
  {
    ""_id"": ""59a0db001cf73a43f7199594"",
    ""index"": 0,
    ""guid"": ""8879b5fb-c8ce-4693-b187-bef7c3d68121"",
    ""isActive"": true,
    ""balance"": ""$3,549.26"",
    ""picture"": ""http://placehold.it/32x32"",
    ""age"": 31,
    ""eyeColor"": ""brown"",
    ""name"": ""Miller Long"",
    ""gender"": ""male"",
    ""company"": ""SPEEDBOLT"",
    ""email"": ""millerlong@speedbolt.com"",
    ""phone"": ""+1 (987) 572-3132"",
    ""address"": ""385 Nassau Street, Fannett, Louisiana, 1828"",
    ""about"": ""Duis cupidatat nostrud consectetur qui pariatur enim. Culpa laborum consectetur nisi incididunt ex tempor aliqua ad eu. Ullamco cillum tempor veniam eiusmod enim sunt do culpa elit id laboris. Incididunt consectetur ea dolor magna et sit.\r\n"",
    ""registered"": ""2016-08-22T12:54:04 +04:00"",
    ""latitude"": 40.026554,
    ""longitude"": 60.849941,
    ""tags"": [
      ""voluptate"",
      ""irure"",
      ""nostrud"",
      ""ad"",
      ""sunt"",
      ""Lorem"",
      ""occaecat"",
      ""ex"",
      ""velit"",
      ""voluptate"",
      ""incididunt"",
      ""id"",
      ""elit"",
      ""do"",
      ""qui"",
      ""consectetur"",
      ""aute"",
      ""proident"",
      ""anim"",
      ""proident""
    ],
    ""friends"": [
      {
        ""id"": 0,
        ""name"": ""Nannie Mcknight""
      },
      {
        ""id"": 1,
        ""name"": ""Jayne Collins""
      },
      {
        ""id"": 2,
        ""name"": ""Sparks Boyer""
      },
      {
        ""id"": 3,
        ""name"": ""Lily Duke""
      },
      {
        ""id"": 4,
        ""name"": ""Booker Gibbs""
      },
      {
        ""id"": 5,
        ""name"": ""Ana Mejia""
      },
      {
        ""id"": 6,
        ""name"": ""Lynne Deleon""
      },
      {
        ""id"": 7,
        ""name"": ""Sheryl Mccarthy""
      },
      {
        ""id"": 8,
        ""name"": ""Hurley Cooley""
      },
      {
        ""id"": 9,
        ""name"": ""Roxie Mckinney""
      },
      {
        ""id"": 10,
        ""name"": ""Fay Glover""
      },
      {
        ""id"": 11,
        ""name"": ""Jaime Sharp""
      },
      {
        ""id"": 12,
        ""name"": ""Montoya Becker""
      },
      {
        ""id"": 13,
        ""name"": ""Brock Hart""
      },
      {
        ""id"": 14,
        ""name"": ""Travis Mathews""
      },
      {
        ""id"": 15,
        ""name"": ""Cotton Hunter""
      },
      {
        ""id"": 16,
        ""name"": ""Marci Logan""
      }
    ],
    ""greeting"": ""Hello, Miller Long! You have 7 unread messages."",
    ""favoriteFruit"": ""banana""
  },
  {
    ""_id"": ""59a0db004808cab27ccb4341"",
    ""index"": 1,
    ""guid"": ""aa064822-cc61-475a-9061-f46590955618"",
    ""isActive"": true,
    ""balance"": ""$2,392.05"",
    ""picture"": ""http://placehold.it/32x32"",
    ""age"": 20,
    ""eyeColor"": ""green"",
    ""name"": ""Belinda Juarez"",
    ""gender"": ""female"",
    ""company"": ""COMCUBINE"",
    ""email"": ""belindajuarez@comcubine.com"",
    ""phone"": ""+1 (904) 441-2301"",
    ""address"": ""937 Eagle Street, Snyderville, Utah, 6788"",
    ""about"": ""Qui consectetur ex sit do. Veniam ex cillum sunt dolore. Minim et nulla incididunt exercitation. Nostrud laborum dolore do fugiat ipsum aliquip ex. Sit ad minim non est cupidatat ut labore dolor sint sit laboris.\r\n"",
    ""registered"": ""2014-12-15T09:33:33 +05:00"",
    ""latitude"": -85.655667,
    ""longitude"": -146.371122,
    ""tags"": [
      ""nulla"",
      ""in"",
      ""in"",
      ""in"",
      ""aute"",
      ""nostrud"",
      ""nisi"",
      ""ad"",
      ""aliqua"",
      ""anim"",
      ""dolor"",
      ""aliquip"",
      ""reprehenderit"",
      ""et"",
      ""id"",
      ""sit"",
      ""deserunt"",
      ""deserunt"",
      ""reprehenderit"",
      ""irure""
    ],
    ""friends"": [
      {
        ""id"": 0,
        ""name"": ""Tyler Duncan""
      },
      {
        ""id"": 1,
        ""name"": ""Angel Ashley""
      },
      {
        ""id"": 2,
        ""name"": ""Adriana Dejesus""
      },
      {
        ""id"": 3,
        ""name"": ""Adrienne Hodges""
      },
      {
        ""id"": 4,
        ""name"": ""Lidia Tyson""
      },
      {
        ""id"": 5,
        ""name"": ""Erna Roach""
      },
      {
        ""id"": 6,
        ""name"": ""Reese Ayala""
      },
      {
        ""id"": 7,
        ""name"": ""Cassie Clarke""
      },
      {
        ""id"": 8,
        ""name"": ""Higgins Chandler""
      },
      {
        ""id"": 9,
        ""name"": ""Morrison Tran""
      },
      {
        ""id"": 10,
        ""name"": ""Saunders Sharpe""
      },
      {
        ""id"": 11,
        ""name"": ""Cabrera Pena""
      },
      {
        ""id"": 12,
        ""name"": ""Cantrell Leon""
      },
      {
        ""id"": 13,
        ""name"": ""Spears Mills""
      },
      {
        ""id"": 14,
        ""name"": ""Samantha Scott""
      },
      {
        ""id"": 15,
        ""name"": ""Shepard Hansen""
      },
      {
        ""id"": 16,
        ""name"": ""Ruby Dunn""
      }
    ],
    ""greeting"": ""Hello, Belinda Juarez! You have 4 unread messages."",
    ""favoriteFruit"": ""strawberry""
  },
  {
    ""_id"": ""59a0db00c342db5eab404b4d"",
    ""index"": 2,
    ""guid"": ""98f8c277-d922-4a6c-a92b-739b57818e94"",
    ""isActive"": true,
    ""balance"": ""$2,473.47"",
    ""picture"": ""http://placehold.it/32x32"",
    ""age"": 37,
    ""eyeColor"": ""brown"",
    ""name"": ""Collier Talley"",
    ""gender"": ""male"",
    ""company"": ""BOSTONIC"",
    ""email"": ""colliertalley@bostonic.com"",
    ""phone"": ""+1 (809) 509-3781"",
    ""address"": ""108 Butler Street, Deercroft, Colorado, 1364"",
    ""about"": ""Fugiat adipisicing cupidatat duis non ex anim nostrud reprehenderit veniam ullamco esse consequat dolore. Minim irure occaecat nisi laborum voluptate do deserunt qui nostrud incididunt incididunt exercitation magna laboris. Deserunt irure ipsum id consequat mollit labore. Cupidatat labore non proident ipsum ullamco tempor irure dolor amet Lorem excepteur consectetur laboris eiusmod. Excepteur elit veniam ullamco Lorem cillum labore qui elit deserunt dolor nulla quis.\r\n"",
    ""registered"": ""2015-03-22T08:17:10 +04:00"",
    ""latitude"": 78.266879,
    ""longitude"": -177.176399,
    ""tags"": [
      ""ut"",
      ""ullamco"",
      ""aliquip"",
      ""ad"",
      ""non"",
      ""qui"",
      ""nisi"",
      ""proident"",
      ""culpa"",
      ""commodo"",
      ""nulla"",
      ""exercitation"",
      ""commodo"",
      ""in"",
      ""labore"",
      ""amet"",
      ""incididunt"",
      ""nulla"",
      ""consequat"",
      ""labore""
    ],
    ""friends"": [
      {
        ""id"": 0,
        ""name"": ""Donna Bennett""
      },
      {
        ""id"": 1,
        ""name"": ""Eileen Waller""
      },
      {
        ""id"": 2,
        ""name"": ""Munoz Carey""
      },
      {
        ""id"": 3,
        ""name"": ""Aline Jefferson""
      },
      {
        ""id"": 4,
        ""name"": ""Elliott Norton""
      },
      {
        ""id"": 5,
        ""name"": ""Vickie Randall""
      },
      {
        ""id"": 6,
        ""name"": ""Sampson Lynch""
      },
      {
        ""id"": 7,
        ""name"": ""Jamie Tanner""
      },
      {
        ""id"": 8,
        ""name"": ""Queen Kramer""
      },
      {
        ""id"": 9,
        ""name"": ""Penelope Boyle""
      },
      {
        ""id"": 10,
        ""name"": ""Terra Kelley""
      },
      {
        ""id"": 11,
        ""name"": ""Morris Justice""
      },
      {
        ""id"": 12,
        ""name"": ""Althea Schneider""
      },
      {
        ""id"": 13,
        ""name"": ""Harmon Mcintyre""
      },
      {
        ""id"": 14,
        ""name"": ""Ayala Mcpherson""
      },
      {
        ""id"": 15,
        ""name"": ""Frieda Davenport""
      },
      {
        ""id"": 16,
        ""name"": ""Brenda Cummings""
      }
    ],
    ""greeting"": ""Hello, Collier Talley! You have 8 unread messages."",
    ""favoriteFruit"": ""strawberry""
  },
  {
    ""_id"": ""59a0db00e63323530400b380"",
    ""index"": 3,
    ""guid"": ""af64716d-1328-4874-a60f-2ffbc987082a"",
    ""isActive"": false,
    ""balance"": ""$1,525.98"",
    ""picture"": ""http://placehold.it/32x32"",
    ""age"": 21,
    ""eyeColor"": ""blue"",
    ""name"": ""Kerry Santos"",
    ""gender"": ""female"",
    ""company"": ""TROPOLIS"",
    ""email"": ""kerrysantos@tropolis.com"",
    ""phone"": ""+1 (827) 579-2092"",
    ""address"": ""495 Friel Place, Sims, Delaware, 7565"",
    ""about"": ""Nostrud ex commodo anim duis esse mollit duis ipsum dolor ipsum. Non cupidatat eiusmod culpa labore occaecat culpa nostrud aute et. Cillum ullamco amet irure eiusmod labore ea ullamco cupidatat irure nostrud culpa.\r\n"",
    ""registered"": ""2017-01-15T08:34:42 +05:00"",
    ""latitude"": -4.55424,
    ""longitude"": -126.068782,
    ""tags"": [
      ""pariatur"",
      ""proident"",
      ""amet"",
      ""laboris"",
      ""proident"",
      ""incididunt"",
      ""pariatur"",
      ""sit"",
      ""aliqua"",
      ""enim"",
      ""exercitation"",
      ""sunt"",
      ""quis"",
      ""cillum"",
      ""minim"",
      ""veniam"",
      ""pariatur"",
      ""dolor"",
      ""id"",
      ""minim""
    ],
    ""friends"": [
      {
        ""id"": 0,
        ""name"": ""May Perez""
      },
      {
        ""id"": 1,
        ""name"": ""Olive Dean""
      },
      {
        ""id"": 2,
        ""name"": ""Esperanza Gregory""
      },
      {
        ""id"": 3,
        ""name"": ""Crosby Warner""
      },
      {
        ""id"": 4,
        ""name"": ""Tameka Olsen""
      },
      {
        ""id"": 5,
        ""name"": ""Mitzi Murray""
      },
      {
        ""id"": 6,
        ""name"": ""Cruz Waters""
      },
      {
        ""id"": 7,
        ""name"": ""Page Byers""
      },
      {
        ""id"": 8,
        ""name"": ""Rhodes Melton""
      },
      {
        ""id"": 9,
        ""name"": ""Lucinda Velazquez""
      },
      {
        ""id"": 10,
        ""name"": ""Dawn Parrish""
      },
      {
        ""id"": 11,
        ""name"": ""Mejia Downs""
      },
      {
        ""id"": 12,
        ""name"": ""Marilyn Dunlap""
      },
      {
        ""id"": 13,
        ""name"": ""Rosalyn Hoffman""
      },
      {
        ""id"": 14,
        ""name"": ""Clarissa Harrington""
      },
      {
        ""id"": 15,
        ""name"": ""Phyllis Golden""
      },
      {
        ""id"": 16,
        ""name"": ""Powers Trujillo""
      }
    ],
    ""greeting"": ""Hello, Kerry Santos! You have 2 unread messages."",
    ""favoriteFruit"": ""banana""
  },
  {
    ""_id"": ""59a0db003975f467aef3f9f0"",
    ""index"": 4,
    ""guid"": ""a142f112-323b-4d20-bc8a-3468eed19757"",
    ""isActive"": true,
    ""balance"": ""$3,299.16"",
    ""picture"": ""http://placehold.it/32x32"",
    ""age"": 34,
    ""eyeColor"": ""brown"",
    ""name"": ""Reba Spence"",
    ""gender"": ""female"",
    ""company"": ""ICOLOGY"",
    ""email"": ""rebaspence@icology.com"",
    ""phone"": ""+1 (919) 518-3304"",
    ""address"": ""690 Bijou Avenue, Roosevelt, South Carolina, 3713"",
    ""about"": ""Velit amet veniam eu exercitation adipisicing veniam velit sint Lorem velit enim nulla pariatur. Velit esse reprehenderit et aliquip sit do anim eu qui consequat id magna. Ad Lorem ad ut id tempor cupidatat ipsum laborum qui. Et dolore consectetur veniam irure quis aliqua elit et ea dolor cillum dolore fugiat. Deserunt adipisicing laboris consectetur laboris nulla consectetur eu magna. Ex id ex pariatur ea incididunt incididunt amet adipisicing exercitation excepteur exercitation ullamco amet.\r\n"",
    ""registered"": ""2016-04-21T08:31:14 +04:00"",
    ""latitude"": -45.17724,
    ""longitude"": -5.294824,
    ""tags"": [
      ""Lorem"",
      ""proident"",
      ""ut"",
      ""magna"",
      ""culpa"",
      ""officia"",
      ""pariatur"",
      ""consectetur"",
      ""esse"",
      ""laborum"",
      ""anim"",
      ""ut"",
      ""cupidatat"",
      ""do"",
      ""ex"",
      ""sint"",
      ""dolor"",
      ""incididunt"",
      ""sit"",
      ""voluptate""
    ],
    ""friends"": [
      {
        ""id"": 0,
        ""name"": ""Amalia Barber""
      },
      {
        ""id"": 1,
        ""name"": ""Beach Leach""
      },
      {
        ""id"": 2,
        ""name"": ""Jeanine Benton""
      },
      {
        ""id"": 3,
        ""name"": ""Opal Crane""
      },
      {
        ""id"": 4,
        ""name"": ""Franks Bridges""
      },
      {
        ""id"": 5,
        ""name"": ""Krista Mcfadden""
      },
      {
        ""id"": 6,
        ""name"": ""Bernard Farmer""
      },
      {
        ""id"": 7,
        ""name"": ""June Kim""
      },
      {
        ""id"": 8,
        ""name"": ""Brandi Thompson""
      },
      {
        ""id"": 9,
        ""name"": ""Poole Mcintosh""
      },
      {
        ""id"": 10,
        ""name"": ""Dalton Weiss""
      },
      {
        ""id"": 11,
        ""name"": ""Cook Pope""
      },
      {
        ""id"": 12,
        ""name"": ""Patti Shepard""
      },
      {
        ""id"": 13,
        ""name"": ""Haynes Owens""
      },
      {
        ""id"": 14,
        ""name"": ""Norris Chambers""
      },
      {
        ""id"": 15,
        ""name"": ""Loretta Mercer""
      },
      {
        ""id"": 16,
        ""name"": ""Rosario Terrell""
      }
    ],
    ""greeting"": ""Hello, Reba Spence! You have 5 unread messages."",
    ""favoriteFruit"": ""banana""
  },
  {
    ""_id"": ""59a0db007a776204cfd79a3f"",
    ""index"": 5,
    ""guid"": ""9a4f1126-92a9-466e-a1bd-ce067c7dfda3"",
    ""isActive"": false,
    ""balance"": ""$2,209.76"",
    ""picture"": ""http://placehold.it/32x32"",
    ""age"": 26,
    ""eyeColor"": ""blue"",
    ""name"": ""Kennedy Webster"",
    ""gender"": ""male"",
    ""company"": ""GEEKWAGON"",
    ""email"": ""kennedywebster@geekwagon.com"",
    ""phone"": ""+1 (865) 472-2430"",
    ""address"": ""101 Congress Street, Suitland, California, 1525"",
    ""about"": ""Excepteur dolore consequat cupidatat irure quis esse amet dolor duis sit. Officia nulla culpa do excepteur in ex eu nulla. Exercitation occaecat proident enim et nulla dolore labore labore incididunt. Enim exercitation ullamco laboris voluptate reprehenderit do veniam excepteur. Minim aliqua ea laboris sint culpa nulla elit Lorem occaecat nulla tempor. Consectetur ea officia quis dolore cupidatat velit. Reprehenderit occaecat laboris voluptate proident excepteur commodo consequat elit deserunt exercitation.\r\n"",
    ""registered"": ""2016-09-19T11:24:20 +04:00"",
    ""latitude"": 40.592756,
    ""longitude"": -135.368337,
    ""tags"": [
      ""deserunt"",
      ""culpa"",
      ""quis"",
      ""fugiat"",
      ""proident"",
      ""aliqua"",
      ""ut"",
      ""labore"",
      ""mollit"",
      ""dolore"",
      ""laboris"",
      ""consequat"",
      ""sunt"",
      ""ullamco"",
      ""aliquip"",
      ""qui"",
      ""deserunt"",
      ""laboris"",
      ""esse"",
      ""irure""
    ],
    ""friends"": [
      {
        ""id"": 0,
        ""name"": ""Lizzie Rutledge""
      },
      {
        ""id"": 1,
        ""name"": ""Montgomery Reynolds""
      },
      {
        ""id"": 2,
        ""name"": ""Cash Bell""
      },
      {
        ""id"": 3,
        ""name"": ""Rollins Ray""
      },
      {
        ""id"": 4,
        ""name"": ""Lamb Mays""
      },
      {
        ""id"": 5,
        ""name"": ""Sara Gaines""
      },
      {
        ""id"": 6,
        ""name"": ""Charity Hoover""
      },
      {
        ""id"": 7,
        ""name"": ""Effie Browning""
      },
      {
        ""id"": 8,
        ""name"": ""Leanne Armstrong""
      },
      {
        ""id"": 9,
        ""name"": ""Hopper Richardson""
      },
      {
        ""id"": 10,
        ""name"": ""Carrie Boyd""
      },
      {
        ""id"": 11,
        ""name"": ""Cathryn Maxwell""
      },
      {
        ""id"": 12,
        ""name"": ""Blackburn Pearson""
      },
      {
        ""id"": 13,
        ""name"": ""Potts Mcclain""
      },
      {
        ""id"": 14,
        ""name"": ""Angelique Williams""
      },
      {
        ""id"": 15,
        ""name"": ""Maddox Love""
      },
      {
        ""id"": 16,
        ""name"": ""Briggs Howard""
      }
    ],
    ""greeting"": ""Hello, Kennedy Webster! You have 5 unread messages."",
    ""favoriteFruit"": ""banana""
  },
  {
    ""_id"": ""59a0db00b5bd971e0c3d4940"",
    ""index"": 6,
    ""guid"": ""a4448761-f0a2-4ee1-a30e-4174e3dbaf44"",
    ""isActive"": false,
    ""balance"": ""$3,852.93"",
    ""picture"": ""http://placehold.it/32x32"",
    ""age"": 40,
    ""eyeColor"": ""blue"",
    ""name"": ""Keith Berger"",
    ""gender"": ""male"",
    ""company"": ""TALKOLA"",
    ""email"": ""keithberger@talkola.com"",
    ""phone"": ""+1 (945) 538-2055"",
    ""address"": ""177 Hastings Street, Taycheedah, Mississippi, 640"",
    ""about"": ""Dolor aute tempor sit eu occaecat fugiat aliqua eu pariatur irure qui. Eu nostrud duis laboris cupidatat. Quis nulla do cupidatat voluptate dolor. Fugiat et elit tempor duis sit. Sunt eu ullamco sunt eiusmod tempor ipsum aute qui pariatur esse.\r\n"",
    ""registered"": ""2017-04-28T03:53:06 +04:00"",
    ""latitude"": 62.994344,
    ""longitude"": -47.666656,
    ""tags"": [
      ""eiusmod"",
      ""reprehenderit"",
      ""sit"",
      ""consectetur"",
      ""culpa"",
      ""tempor"",
      ""ex"",
      ""Lorem"",
      ""non"",
      ""laborum"",
      ""est"",
      ""ut"",
      ""adipisicing"",
      ""velit"",
      ""duis"",
      ""Lorem"",
      ""magna"",
      ""reprehenderit"",
      ""elit"",
      ""elit""
    ],
    ""friends"": [
      {
        ""id"": 0,
        ""name"": ""Byrd Gordon""
      },
      {
        ""id"": 1,
        ""name"": ""Diann Farrell""
      },
      {
        ""id"": 2,
        ""name"": ""Gwen Cohen""
      },
      {
        ""id"": 3,
        ""name"": ""Middleton Snow""
      },
      {
        ""id"": 4,
        ""name"": ""Lorrie Bradford""
      },
      {
        ""id"": 5,
        ""name"": ""Rosalinda Britt""
      },
      {
        ""id"": 6,
        ""name"": ""Fowler Oliver""
      },
      {
        ""id"": 7,
        ""name"": ""Alissa Jennings""
      },
      {
        ""id"": 8,
        ""name"": ""Lowe Moran""
      },
      {
        ""id"": 9,
        ""name"": ""Ellen Mcguire""
      },
      {
        ""id"": 10,
        ""name"": ""Alana Obrien""
      },
      {
        ""id"": 11,
        ""name"": ""Deann Clark""
      },
      {
        ""id"": 12,
        ""name"": ""Natalie Buchanan""
      },
      {
        ""id"": 13,
        ""name"": ""Hess Strong""
      },
      {
        ""id"": 14,
        ""name"": ""Bridgett Rush""
      },
      {
        ""id"": 15,
        ""name"": ""Avila Mccray""
      },
      {
        ""id"": 16,
        ""name"": ""Letitia Patrick""
      }
    ],
    ""greeting"": ""Hello, Keith Berger! You have 2 unread messages."",
    ""favoriteFruit"": ""strawberry""
  },
  {
    ""_id"": ""59a0db00f7f43d5fd5028a2c"",
    ""index"": 7,
    ""guid"": ""e7ece7ac-ede9-4755-a5e8-5d644ca2a67e"",
    ""isActive"": false,
    ""balance"": ""$2,623.71"",
    ""picture"": ""http://placehold.it/32x32"",
    ""age"": 26,
    ""eyeColor"": ""blue"",
    ""name"": ""Luna Petersen"",
    ""gender"": ""male"",
    ""company"": ""SYNTAC"",
    ""email"": ""lunapetersen@syntac.com"",
    ""phone"": ""+1 (925) 530-3992"",
    ""address"": ""961 Ditmas Avenue, Beechmont, Washington, 5010"",
    ""about"": ""Do in laboris id duis sunt labore. Ad eiusmod enim nisi aliquip adipisicing proident esse ipsum. Tempor cillum enim ullamco sunt pariatur dolore quis eu laboris qui dolore proident cillum. Eiusmod est mollit reprehenderit ullamco et est anim esse ea deserunt. Sint est culpa dolor ad velit fugiat minim id do occaecat esse ullamco ut fugiat. Minim ea cillum pariatur mollit culpa minim enim. Exercitation sit minim ad incididunt ipsum sit cupidatat reprehenderit.\r\n"",
    ""registered"": ""2014-07-10T12:51:59 +04:00"",
    ""latitude"": -36.833726,
    ""longitude"": -117.695805,
    ""tags"": [
      ""deserunt"",
      ""ad"",
      ""commodo"",
      ""proident"",
      ""ullamco"",
      ""ea"",
      ""dolor"",
      ""enim"",
      ""aliquip"",
      ""consequat"",
      ""irure"",
      ""incididunt"",
      ""id"",
      ""deserunt"",
      ""consequat"",
      ""pariatur"",
      ""enim"",
      ""ea"",
      ""duis"",
      ""occaecat""
    ],
    ""friends"": [
      {
        ""id"": 0,
        ""name"": ""Kelsey Harrell""
      },
      {
        ""id"": 1,
        ""name"": ""Janie Mueller""
      },
      {
        ""id"": 2,
        ""name"": ""George Knowles""
      },
      {
        ""id"": 3,
        ""name"": ""Wright Mason""
      },
      {
        ""id"": 4,
        ""name"": ""Ashlee Roth""
      },
      {
        ""id"": 5,
        ""name"": ""Dejesus Fuentes""
      },
      {
        ""id"": 6,
        ""name"": ""Peggy Nichols""
      },
      {
        ""id"": 7,
        ""name"": ""Justine Clemons""
      },
      {
        ""id"": 8,
        ""name"": ""Greene Sexton""
      },
      {
        ""id"": 9,
        ""name"": ""Yang Chan""
      },
      {
        ""id"": 10,
        ""name"": ""Shaw Walton""
      },
      {
        ""id"": 11,
        ""name"": ""Wilson Hess""
      },
      {
        ""id"": 12,
        ""name"": ""Curtis Stephenson""
      },
      {
        ""id"": 13,
        ""name"": ""Nettie Horn""
      },
      {
        ""id"": 14,
        ""name"": ""Bonner Washington""
      },
      {
        ""id"": 15,
        ""name"": ""Meredith Potts""
      },
      {
        ""id"": 16,
        ""name"": ""Francis Mccarty""
      }
    ],
    ""greeting"": ""Hello, Luna Petersen! You have 7 unread messages."",
    ""favoriteFruit"": ""banana""
  },
  {
    ""_id"": ""59a0db00a561c2a72379dfa7"",
    ""index"": 8,
    ""guid"": ""f68b84ba-a5ca-4585-a53d-209aaae037c6"",
    ""isActive"": true,
    ""balance"": ""$1,587.29"",
    ""picture"": ""http://placehold.it/32x32"",
    ""age"": 23,
    ""eyeColor"": ""blue"",
    ""name"": ""Peterson Frye"",
    ""gender"": ""male"",
    ""company"": ""YURTURE"",
    ""email"": ""petersonfrye@yurture.com"",
    ""phone"": ""+1 (929) 408-3838"",
    ""address"": ""262 Navy Walk, Russellville, Oklahoma, 3626"",
    ""about"": ""Tempor incididunt consequat fugiat excepteur veniam sit. Dolore ut cillum proident nulla sit culpa in anim consequat do. Ullamco laboris minim veniam id labore velit occaecat pariatur. Voluptate mollit aute et ex do enim ex.\r\n"",
    ""registered"": ""2014-09-17T11:01:39 +04:00"",
    ""latitude"": -52.570331,
    ""longitude"": -77.751225,
    ""tags"": [
      ""anim"",
      ""adipisicing"",
      ""irure"",
      ""duis"",
      ""voluptate"",
      ""labore"",
      ""occaecat"",
      ""id"",
      ""esse"",
      ""exercitation"",
      ""veniam"",
      ""ullamco"",
      ""dolore"",
      ""aute"",
      ""cupidatat"",
      ""nulla"",
      ""officia"",
      ""veniam"",
      ""adipisicing"",
      ""fugiat""
    ],
    ""friends"": [
      {
        ""id"": 0,
        ""name"": ""Marguerite Gardner""
      },
      {
        ""id"": 1,
        ""name"": ""Dominique Maynard""
      },
      {
        ""id"": 2,
        ""name"": ""Beard Cantrell""
      },
      {
        ""id"": 3,
        ""name"": ""Calhoun Blackwell""
      },
      {
        ""id"": 4,
        ""name"": ""Carey Robbins""
      },
      {
        ""id"": 5,
        ""name"": ""Clark Langley""
      },
      {
        ""id"": 6,
        ""name"": ""Janice Sosa""
      },
      {
        ""id"": 7,
        ""name"": ""Castillo Carney""
      },
      {
        ""id"": 8,
        ""name"": ""Summers Rich""
      },
      {
        ""id"": 9,
        ""name"": ""Nell Johnson""
      },
      {
        ""id"": 10,
        ""name"": ""Marlene Holloway""
      },
      {
        ""id"": 11,
        ""name"": ""Richardson Moss""
      },
      {
        ""id"": 12,
        ""name"": ""Rios Hampton""
      },
      {
        ""id"": 13,
        ""name"": ""Marquita Hughes""
      },
      {
        ""id"": 14,
        ""name"": ""Foreman Ayers""
      },
      {
        ""id"": 15,
        ""name"": ""Miranda Shaffer""
      },
      {
        ""id"": 16,
        ""name"": ""Etta Crawford""
      }
    ],
    ""greeting"": ""Hello, Peterson Frye! You have 3 unread messages."",
    ""favoriteFruit"": ""apple""
  },
  {
    ""_id"": ""59a0db0088961b0080204094"",
    ""index"": 9,
    ""guid"": ""ed8f75cb-53d4-44f9-be30-164ec3dfffd4"",
    ""isActive"": false,
    ""balance"": ""$1,650.39"",
    ""picture"": ""http://placehold.it/32x32"",
    ""age"": 30,
    ""eyeColor"": ""brown"",
    ""name"": ""Juliana Walter"",
    ""gender"": ""female"",
    ""company"": ""ZENTIX"",
    ""email"": ""julianawalter@zentix.com"",
    ""phone"": ""+1 (972) 448-3078"",
    ""address"": ""607 Manhattan Avenue, Chaparrito, Virginia, 9284"",
    ""about"": ""Occaecat irure amet consectetur dolore dolor eu dolor labore in ad in dolor elit tempor. Non aliqua tempor occaecat officia ipsum veniam esse. Deserunt qui ut incididunt laborum minim in. Incididunt deserunt sint duis aute quis et minim. Sit aliqua sit irure voluptate sint non est anim. Consequat irure elit amet quis. Deserunt magna esse adipisicing ea Lorem voluptate dolore.\r\n"",
    ""registered"": ""2015-01-22T08:36:44 +05:00"",
    ""latitude"": -64.058441,
    ""longitude"": -2.844359,
    ""tags"": [
      ""cillum"",
      ""tempor"",
      ""id"",
      ""exercitation"",
      ""mollit"",
      ""non"",
      ""laboris"",
      ""adipisicing"",
      ""velit"",
      ""dolore"",
      ""commodo"",
      ""do"",
      ""irure"",
      ""id"",
      ""consequat"",
      ""eiusmod"",
      ""nostrud"",
      ""deserunt"",
      ""ea"",
      ""id""
    ],
    ""friends"": [
      {
        ""id"": 0,
        ""name"": ""Erickson Holcomb""
      },
      {
        ""id"": 1,
        ""name"": ""Moreno York""
      },
      {
        ""id"": 2,
        ""name"": ""Annmarie Bishop""
      },
      {
        ""id"": 3,
        ""name"": ""Shelton Delacruz""
      },
      {
        ""id"": 4,
        ""name"": ""Best Bates""
      },
      {
        ""id"": 5,
        ""name"": ""Candice Malone""
      },
      {
        ""id"": 6,
        ""name"": ""Wynn Sheppard""
      },
      {
        ""id"": 7,
        ""name"": ""Keisha Salinas""
      },
      {
        ""id"": 8,
        ""name"": ""Marsh Hancock""
      },
      {
        ""id"": 9,
        ""name"": ""Hoffman Shepherd""
      },
      {
        ""id"": 10,
        ""name"": ""Natalia Salas""
      },
      {
        ""id"": 11,
        ""name"": ""Aida Morrow""
      },
      {
        ""id"": 12,
        ""name"": ""Newman Huff""
      },
      {
        ""id"": 13,
        ""name"": ""Ofelia Middleton""
      },
      {
        ""id"": 14,
        ""name"": ""Yvonne Norris""
      },
      {
        ""id"": 15,
        ""name"": ""Blanchard Santiago""
      },
      {
        ""id"": 16,
        ""name"": ""Terrie Cochran""
      }
    ],
    ""greeting"": ""Hello, Juliana Walter! You have 7 unread messages."",
    ""favoriteFruit"": ""banana""
  },
  {
    ""_id"": ""59a0db00bb08d1894bf1b12d"",
    ""index"": 10,
    ""guid"": ""4134851e-4c64-4a45-8907-c3e9caba51d6"",
    ""isActive"": false,
    ""balance"": ""$1,180.33"",
    ""picture"": ""http://placehold.it/32x32"",
    ""age"": 30,
    ""eyeColor"": ""green"",
    ""name"": ""Cecelia Rosa"",
    ""gender"": ""female"",
    ""company"": ""CORPORANA"",
    ""email"": ""ceceliarosa@corporana.com"",
    ""phone"": ""+1 (941) 518-2703"",
    ""address"": ""692 Hill Street, Yonah, Wyoming, 2046"",
    ""about"": ""Officia voluptate exercitation in consequat consectetur laboris tempor. Consequat tempor laborum non exercitation. Ipsum ut amet commodo mollit non nostrud tempor amet. Ad ipsum et cillum voluptate nulla id ipsum duis enim dolor in non. Aute dolore Lorem ea sint. Nostrud irure veniam amet Lorem velit officia sint in.\r\n"",
    ""registered"": ""2014-07-31T04:11:05 +04:00"",
    ""latitude"": -21.743129,
    ""longitude"": -43.59498,
    ""tags"": [
      ""aliqua"",
      ""aute"",
      ""duis"",
      ""aliqua"",
      ""sint"",
      ""occaecat"",
      ""adipisicing"",
      ""pariatur"",
      ""est"",
      ""reprehenderit"",
      ""ea"",
      ""aliquip"",
      ""nisi"",
      ""pariatur"",
      ""duis"",
      ""elit"",
      ""anim"",
      ""magna"",
      ""sit"",
      ""sunt""
    ],
    ""friends"": [
      {
        ""id"": 0,
        ""name"": ""Jenny Pratt""
      },
      {
        ""id"": 1,
        ""name"": ""Henry Nieves""
      },
      {
        ""id"": 2,
        ""name"": ""Victoria Faulkner""
      },
      {
        ""id"": 3,
        ""name"": ""Ashley Strickland""
      },
      {
        ""id"": 4,
        ""name"": ""Erin Gomez""
      },
      {
        ""id"": 5,
        ""name"": ""Elise Larson""
      },
      {
        ""id"": 6,
        ""name"": ""Burt Griffith""
      },
      {
        ""id"": 7,
        ""name"": ""Matilda Gray""
      },
      {
        ""id"": 8,
        ""name"": ""Jacobs Church""
      },
      {
        ""id"": 9,
        ""name"": ""Maryann Cobb""
      },
      {
        ""id"": 10,
        ""name"": ""Greta Simmons""
      },
      {
        ""id"": 11,
        ""name"": ""Sheila Sanchez""
      },
      {
        ""id"": 12,
        ""name"": ""Patterson Summers""
      },
      {
        ""id"": 13,
        ""name"": ""Johanna Odom""
      },
      {
        ""id"": 14,
        ""name"": ""Deleon Bowers""
      },
      {
        ""id"": 15,
        ""name"": ""Brady Adams""
      },
      {
        ""id"": 16,
        ""name"": ""Cross Curtis""
      }
    ],
    ""greeting"": ""Hello, Cecelia Rosa! You have 10 unread messages."",
    ""favoriteFruit"": ""banana""
  },
  {
    ""_id"": ""59a0db00ca0873bd8e79a0e1"",
    ""index"": 11,
    ""guid"": ""b02a033b-eb9c-4da5-8f22-986499cade13"",
    ""isActive"": false,
    ""balance"": ""$2,991.35"",
    ""picture"": ""http://placehold.it/32x32"",
    ""age"": 30,
    ""eyeColor"": ""blue"",
    ""name"": ""Kelly Albert"",
    ""gender"": ""male"",
    ""company"": ""XLEEN"",
    ""email"": ""kellyalbert@xleen.com"",
    ""phone"": ""+1 (911) 502-3122"",
    ""address"": ""301 Coventry Road, Ryderwood, Marshall Islands, 2032"",
    ""about"": ""Reprehenderit Lorem Lorem irure nulla ea laboris enim Lorem id laborum id consequat amet Lorem. Culpa aute magna culpa ea ipsum proident do minim esse exercitation reprehenderit laborum incididunt. Exercitation dolore fugiat aute sint aliqua ad labore cillum sit officia sint.\r\n"",
    ""registered"": ""2014-04-10T12:36:31 +04:00"",
    ""latitude"": -68.119884,
    ""longitude"": 32.846576,
    ""tags"": [
      ""occaecat"",
      ""esse"",
      ""est"",
      ""adipisicing"",
      ""nisi"",
      ""aliqua"",
      ""proident"",
      ""ex"",
      ""labore"",
      ""commodo"",
      ""qui"",
      ""duis"",
      ""culpa"",
      ""non"",
      ""aute"",
      ""aliquip"",
      ""ut"",
      ""culpa"",
      ""aute"",
      ""quis""
    ],
    ""friends"": [
      {
        ""id"": 0,
        ""name"": ""Diaz Valenzuela""
      },
      {
        ""id"": 1,
        ""name"": ""Vera Burnett""
      },
      {
        ""id"": 2,
        ""name"": ""Clarke Henry""
      },
      {
        ""id"": 3,
        ""name"": ""Harriett Baird""
      },
      {
        ""id"": 4,
        ""name"": ""Britt Thomas""
      },
      {
        ""id"": 5,
        ""name"": ""Yesenia Fowler""
      },
      {
        ""id"": 6,
        ""name"": ""Golden Chang""
      },
      {
        ""id"": 7,
        ""name"": ""Jeannie Price""
      },
      {
        ""id"": 8,
        ""name"": ""Frederick Bradshaw""
      },
      {
        ""id"": 9,
        ""name"": ""Taylor Kirby""
      },
      {
        ""id"": 10,
        ""name"": ""Elvira Calhoun""
      },
      {
        ""id"": 11,
        ""name"": ""Mai Holden""
      },
      {
        ""id"": 12,
        ""name"": ""Lourdes Shaw""
      },
      {
        ""id"": 13,
        ""name"": ""Diana Stark""
      },
      {
        ""id"": 14,
        ""name"": ""Maynard Lowery""
      },
      {
        ""id"": 15,
        ""name"": ""Jewel Whitfield""
      },
      {
        ""id"": 16,
        ""name"": ""Young Sargent""
      }
    ],
    ""greeting"": ""Hello, Kelly Albert! You have 2 unread messages."",
    ""favoriteFruit"": ""banana""
  },
  {
    ""_id"": ""59a0db001fb74de343e2f499"",
    ""index"": 12,
    ""guid"": ""ba4f0cf3-7617-4261-b9b0-6cf728eb1f57"",
    ""isActive"": false,
    ""balance"": ""$3,918.06"",
    ""picture"": ""http://placehold.it/32x32"",
    ""age"": 38,
    ""eyeColor"": ""brown"",
    ""name"": ""Bobbie Dalton"",
    ""gender"": ""female"",
    ""company"": ""EARTHMARK"",
    ""email"": ""bobbiedalton@earthmark.com"",
    ""phone"": ""+1 (895) 541-2815"",
    ""address"": ""718 Commerce Street, Sena, Federated States Of Micronesia, 1014"",
    ""about"": ""Incididunt eu culpa quis dolor eiusmod ut non enim dolor veniam excepteur mollit. In consequat consectetur ex aute laborum. Laboris fugiat fugiat ut velit magna voluptate in velit qui. Esse labore officia elit cillum deserunt sint proident nisi quis.\r\n"",
    ""registered"": ""2015-04-30T06:00:18 +04:00"",
    ""latitude"": -64.938563,
    ""longitude"": -62.719807,
    ""tags"": [
      ""officia"",
      ""aliquip"",
      ""incididunt"",
      ""ea"",
      ""sunt"",
      ""incididunt"",
      ""consectetur"",
      ""quis"",
      ""ipsum"",
      ""labore"",
      ""excepteur"",
      ""aute"",
      ""non"",
      ""non"",
      ""est"",
      ""laboris"",
      ""labore"",
      ""amet"",
      ""consequat"",
      ""dolore""
    ],
    ""friends"": [
      {
        ""id"": 0,
        ""name"": ""Dawson Rivers""
      },
      {
        ""id"": 1,
        ""name"": ""Strickland Knight""
      },
      {
        ""id"": 2,
        ""name"": ""Iva Blankenship""
      },
      {
        ""id"": 3,
        ""name"": ""Melisa Santana""
      },
      {
        ""id"": 4,
        ""name"": ""Harvey Woods""
      },
      {
        ""id"": 5,
        ""name"": ""Tate Cruz""
      },
      {
        ""id"": 6,
        ""name"": ""Parsons Hammond""
      },
      {
        ""id"": 7,
        ""name"": ""James Higgins""
      },
      {
        ""id"": 8,
        ""name"": ""Hines Ball""
      },
      {
        ""id"": 9,
        ""name"": ""Hancock Horton""
      },
      {
        ""id"": 10,
        ""name"": ""Deborah Underwood""
      },
      {
        ""id"": 11,
        ""name"": ""Nita Hodge""
      },
      {
        ""id"": 12,
        ""name"": ""Mayo Gutierrez""
      },
      {
        ""id"": 13,
        ""name"": ""Pena Sweet""
      },
      {
        ""id"": 14,
        ""name"": ""Cheri Beck""
      },
      {
        ""id"": 15,
        ""name"": ""Thornton Merritt""
      },
      {
        ""id"": 16,
        ""name"": ""Eunice Battle""
      }
    ],
    ""greeting"": ""Hello, Bobbie Dalton! You have 5 unread messages."",
    ""favoriteFruit"": ""apple""
  },
  {
    ""_id"": ""59a0db00c3af604d5f96d94c"",
    ""index"": 13,
    ""guid"": ""5fc3c8ff-11ac-4bb4-9398-2265e87c0f57"",
    ""isActive"": false,
    ""balance"": ""$1,952.83"",
    ""picture"": ""http://placehold.it/32x32"",
    ""age"": 20,
    ""eyeColor"": ""blue"",
    ""name"": ""Kristin Ware"",
    ""gender"": ""female"",
    ""company"": ""OULU"",
    ""email"": ""kristinware@oulu.com"",
    ""phone"": ""+1 (881) 473-3281"",
    ""address"": ""437 Essex Street, Courtland, Oregon, 7563"",
    ""about"": ""Proident dolor officia commodo elit occaecat anim consequat proident ut occaecat nulla. Sit ad laborum sint ullamco ipsum culpa magna. Cillum anim proident esse sint cupidatat dolore duis nulla exercitation labore. Id ea elit nisi quis aute anim amet consectetur veniam.\r\n"",
    ""registered"": ""2015-09-05T10:55:45 +04:00"",
    ""latitude"": -37.053317,
    ""longitude"": 120.789184,
    ""tags"": [
      ""eiusmod"",
      ""elit"",
      ""non"",
      ""anim"",
      ""qui"",
      ""pariatur"",
      ""excepteur"",
      ""eiusmod"",
      ""minim"",
      ""quis"",
      ""in"",
      ""in"",
      ""duis"",
      ""id"",
      ""minim"",
      ""qui"",
      ""aute"",
      ""consectetur"",
      ""nisi"",
      ""est""
    ],
    ""friends"": [
      {
        ""id"": 0,
        ""name"": ""Guzman Day""
      },
      {
        ""id"": 1,
        ""name"": ""Mendoza Chen""
      },
      {
        ""id"": 2,
        ""name"": ""Lindsey Rasmussen""
      },
      {
        ""id"": 3,
        ""name"": ""Patrica Grimes""
      },
      {
        ""id"": 4,
        ""name"": ""Mccormick Valdez""
      },
      {
        ""id"": 5,
        ""name"": ""Foster Stewart""
      },
      {
        ""id"": 6,
        ""name"": ""Hampton Moses""
      },
      {
        ""id"": 7,
        ""name"": ""Hendricks Foley""
      },
      {
        ""id"": 8,
        ""name"": ""Margo Coleman""
      },
      {
        ""id"": 9,
        ""name"": ""Knight Stein""
      },
      {
        ""id"": 10,
        ""name"": ""Donovan Harper""
      },
      {
        ""id"": 11,
        ""name"": ""Scott Mckee""
      },
      {
        ""id"": 12,
        ""name"": ""Rosie Gould""
      },
      {
        ""id"": 13,
        ""name"": ""Knapp Goodman""
      },
      {
        ""id"": 14,
        ""name"": ""Rochelle Gilbert""
      },
      {
        ""id"": 15,
        ""name"": ""Molina Case""
      },
      {
        ""id"": 16,
        ""name"": ""Mercedes Delaney""
      }
    ],
    ""greeting"": ""Hello, Kristin Ware! You have 8 unread messages."",
    ""favoriteFruit"": ""banana""
  },
  {
    ""_id"": ""59a0db005f8ef429cb72a9f5"",
    ""index"": 14,
    ""guid"": ""638116a9-5ebc-426b-877d-b799be471312"",
    ""isActive"": false,
    ""balance"": ""$2,129.43"",
    ""picture"": ""http://placehold.it/32x32"",
    ""age"": 28,
    ""eyeColor"": ""brown"",
    ""name"": ""Nicole Zimmerman"",
    ""gender"": ""female"",
    ""company"": ""UPDAT"",
    ""email"": ""nicolezimmerman@updat.com"",
    ""phone"": ""+1 (847) 502-2882"",
    ""address"": ""341 Apollo Street, Dennard, Connecticut, 5593"",
    ""about"": ""Tempor exercitation dolore ex et mollit est nisi sunt. Esse est fugiat sint exercitation enim ipsum eu consequat ea eu deserunt laboris ad nostrud. Et incididunt enim eiusmod commodo mollit do esse id sit. Id exercitation id aliqua incididunt dolore sunt non dolor ipsum. Qui occaecat culpa qui sit irure dolor labore nostrud. Sunt non minim minim aliquip.\r\n"",
    ""registered"": ""2014-05-23T04:23:54 +04:00"",
    ""latitude"": 62.799228,
    ""longitude"": -57.772912,
    ""tags"": [
      ""ea"",
      ""irure"",
      ""culpa"",
      ""fugiat"",
      ""nisi"",
      ""eiusmod"",
      ""voluptate"",
      ""magna"",
      ""esse"",
      ""Lorem"",
      ""veniam"",
      ""anim"",
      ""proident"",
      ""consectetur"",
      ""velit"",
      ""est"",
      ""esse"",
      ""in"",
      ""consectetur"",
      ""culpa""
    ],
    ""friends"": [
      {
        ""id"": 0,
        ""name"": ""Gretchen Mann""
      },
      {
        ""id"": 1,
        ""name"": ""Lewis Carson""
      },
      {
        ""id"": 2,
        ""name"": ""Oliver Hayden""
      },
      {
        ""id"": 3,
        ""name"": ""Cherry Skinner""
      },
      {
        ""id"": 4,
        ""name"": ""Bethany Wilson""
      },
      {
        ""id"": 5,
        ""name"": ""Mollie Duffy""
      },
      {
        ""id"": 6,
        ""name"": ""Frye Parsons""
      },
      {
        ""id"": 7,
        ""name"": ""Carla Workman""
      },
      {
        ""id"": 8,
        ""name"": ""Holden Benson""
      },
      {
        ""id"": 9,
        ""name"": ""Mariana Donaldson""
      },
      {
        ""id"": 10,
        ""name"": ""Raquel Lynn""
      },
      {
        ""id"": 11,
        ""name"": ""Staci Brewer""
      },
      {
        ""id"": 12,
        ""name"": ""Neva Willis""
      },
      {
        ""id"": 13,
        ""name"": ""Dina Roman""
      },
      {
        ""id"": 14,
        ""name"": ""Deirdre Randolph""
      },
      {
        ""id"": 15,
        ""name"": ""Goff Rosales""
      },
      {
        ""id"": 16,
        ""name"": ""Kemp Floyd""
      }
    ],
    ""greeting"": ""Hello, Nicole Zimmerman! You have 9 unread messages."",
    ""favoriteFruit"": ""strawberry""
  },
  {
    ""_id"": ""59a0db00214c1c21b23bd9ad"",
    ""index"": 15,
    ""guid"": ""433e7eaf-4673-411f-9817-3271188ce479"",
    ""isActive"": false,
    ""balance"": ""$2,517.99"",
    ""picture"": ""http://placehold.it/32x32"",
    ""age"": 31,
    ""eyeColor"": ""brown"",
    ""name"": ""Faye Perkins"",
    ""gender"": ""female"",
    ""company"": ""CONJURICA"",
    ""email"": ""fayeperkins@conjurica.com"",
    ""phone"": ""+1 (993) 451-2533"",
    ""address"": ""669 Clay Street, Barronett, District Of Columbia, 1680"",
    ""about"": ""Ut ex incididunt ea quis consectetur qui tempor et eiusmod. Reprehenderit magna incididunt consectetur ex cupidatat nostrud laboris cillum sit Lorem. Quis nisi quis irure quis consequat deserunt occaecat exercitation ullamco ea labore aute minim nostrud.\r\n"",
    ""registered"": ""2015-02-05T12:48:54 +05:00"",
    ""latitude"": -42.349513,
    ""longitude"": -34.592877,
    ""tags"": [
      ""et"",
      ""occaecat"",
      ""occaecat"",
      ""ut"",
      ""pariatur"",
      ""quis"",
      ""ullamco"",
      ""consectetur"",
      ""veniam"",
      ""ut"",
      ""eu"",
      ""reprehenderit"",
      ""laborum"",
      ""ipsum"",
      ""consectetur"",
      ""occaecat"",
      ""ipsum"",
      ""velit"",
      ""ullamco"",
      ""quis""
    ],
    ""friends"": [
      {
        ""id"": 0,
        ""name"": ""Hahn Mckenzie""
      },
      {
        ""id"": 1,
        ""name"": ""Gilbert Ryan""
      },
      {
        ""id"": 2,
        ""name"": ""Hester Ballard""
      },
      {
        ""id"": 3,
        ""name"": ""Alvarado Chavez""
      },
      {
        ""id"": 4,
        ""name"": ""Shawn Conner""
      },
      {
        ""id"": 5,
        ""name"": ""Tammy Ramsey""
      },
      {
        ""id"": 6,
        ""name"": ""Hoover Wolfe""
      },
      {
        ""id"": 7,
        ""name"": ""Young Cortez""
      },
      {
        ""id"": 8,
        ""name"": ""Marks Fields""
      },
      {
        ""id"": 9,
        ""name"": ""Webb Johns""
      },
      {
        ""id"": 10,
        ""name"": ""Figueroa Russell""
      },
      {
        ""id"": 11,
        ""name"": ""Clemons Wong""
      },
      {
        ""id"": 12,
        ""name"": ""Craig Stevens""
      },
      {
        ""id"": 13,
        ""name"": ""Chasity Carroll""
      },
      {
        ""id"": 14,
        ""name"": ""Ericka Saunders""
      },
      {
        ""id"": 15,
        ""name"": ""Georgina Jones""
      },
      {
        ""id"": 16,
        ""name"": ""Lakisha Huffman""
      }
    ],
    ""greeting"": ""Hello, Faye Perkins! You have 1 unread messages."",
    ""favoriteFruit"": ""banana""
  },
  {
    ""_id"": ""59a0db00e81462929ad85ddd"",
    ""index"": 16,
    ""guid"": ""ac822c43-d2b4-4c86-b955-ff0f4038b927"",
    ""isActive"": false,
    ""balance"": ""$3,715.52"",
    ""picture"": ""http://placehold.it/32x32"",
    ""age"": 28,
    ""eyeColor"": ""brown"",
    ""name"": ""Robert Swanson"",
    ""gender"": ""female"",
    ""company"": ""TSUNAMIA"",
    ""email"": ""robertswanson@tsunamia.com"",
    ""phone"": ""+1 (953) 429-2177"",
    ""address"": ""981 Crosby Avenue, Enoree, Tennessee, 9011"",
    ""about"": ""Nulla proident consequat consectetur mollit enim est minim. Aliqua sit non id enim anim eu do. Tempor tempor velit sint incididunt est excepteur aliqua nostrud incididunt laborum anim tempor id ut.\r\n"",
    ""registered"": ""2016-12-31T07:44:42 +05:00"",
    ""latitude"": 18.711008,
    ""longitude"": 95.959439,
    ""tags"": [
      ""occaecat"",
      ""aliqua"",
      ""aliqua"",
      ""exercitation"",
      ""ut"",
      ""ex"",
      ""nostrud"",
      ""laborum"",
      ""pariatur"",
      ""in"",
      ""laboris"",
      ""veniam"",
      ""commodo"",
      ""aliquip"",
      ""quis"",
      ""cillum"",
      ""et"",
      ""minim"",
      ""culpa"",
      ""aliqua""
    ],
    ""friends"": [
      {
        ""id"": 0,
        ""name"": ""Hammond Erickson""
      },
      {
        ""id"": 1,
        ""name"": ""Pam Stafford""
      },
      {
        ""id"": 2,
        ""name"": ""Hester Gamble""
      },
      {
        ""id"": 3,
        ""name"": ""Gwendolyn Flowers""
      },
      {
        ""id"": 4,
        ""name"": ""Shauna Gallagher""
      },
      {
        ""id"": 5,
        ""name"": ""Ora Peck""
      },
      {
        ""id"": 6,
        ""name"": ""Lauri Short""
      },
      {
        ""id"": 7,
        ""name"": ""Dudley Wagner""
      },
      {
        ""id"": 8,
        ""name"": ""Michael Wilcox""
      },
      {
        ""id"": 9,
        ""name"": ""Klein Key""
      },
      {
        ""id"": 10,
        ""name"": ""Eula Walker""
      },
      {
        ""id"": 11,
        ""name"": ""Rosella Gentry""
      },
      {
        ""id"": 12,
        ""name"": ""Burris Valentine""
      },
      {
        ""id"": 13,
        ""name"": ""Esmeralda Romero""
      },
      {
        ""id"": 14,
        ""name"": ""Imelda Warren""
      },
      {
        ""id"": 15,
        ""name"": ""Sloan Manning""
      },
      {
        ""id"": 16,
        ""name"": ""Elaine Walters""
      }
    ],
    ""greeting"": ""Hello, Robert Swanson! You have 9 unread messages."",
    ""favoriteFruit"": ""strawberry""
  }
]
        ";
    }
}
