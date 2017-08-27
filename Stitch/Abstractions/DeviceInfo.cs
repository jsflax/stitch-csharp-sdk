using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Stitch
{
    public struct DeviceInfo
    {
		[BsonElement("appVersion")]
		public string AppVersion;

		[BsonElement("appId")]
		public string AppId;

		[BsonElement("platform")]
		public string Platform;

		[BsonElement("platformVersion")]
		public string PlatformVersion;
    }
}
