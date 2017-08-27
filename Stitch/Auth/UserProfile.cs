using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Stitch.Auth
{
    public struct UserProfile
    {
		[JsonProperty(PropertyName = "userId")]
		public string UserId;
		[JsonProperty(PropertyName = "identities")]
		public List<Identity> Identities;
		[JsonProperty(PropertyName = "data")]
		public Dictionary<string, object> Data;

        public struct Identity
        {
			[JsonProperty(PropertyName = "id")]
			public string Id;
			[JsonProperty(PropertyName = "provider")]
			public string Provider;
        }
    }
}
