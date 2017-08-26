using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Stitch.Auth
{
    public class AuthInfo
    {
        [JsonProperty(PropertyName = "accessToken")]
        [BsonElement("accessToken")]
        public string AccessToken { get; internal set; }

        [JsonProperty(PropertyName = "userId")]
		[BsonElement("userId")]
		public string UserId { get; internal set; }

        [JsonProperty(PropertyName = "deviceId")]
		[BsonElement("deviceId")]
		public string DeviceId { get; internal set; }

        [JsonProperty(PropertyName = "provider")]
		[BsonElement("provider")]
		public string Provider { get; internal set; }

        public override string ToString()
        {
            return string.Format("[AuthInfo: AccessToken={0}, UserId={1}, DeviceId={2}, Provider={3}]", AccessToken, UserId, DeviceId, Provider);
        }
    }
}
