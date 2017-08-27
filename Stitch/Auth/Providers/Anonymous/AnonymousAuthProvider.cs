using System;
namespace Stitch.Auth
{
	public struct AnonymousAuthProvider : IAuthProvider
	{
		string IAuthProvider.Type
		{
			get
			{
				return "anon";
			}
		}

		string IAuthProvider.ProviderName
		{
			get
			{
				return "user";
			}
		}

		MongoDB.Bson.BsonDocument IAuthProvider.Payload
		{
			get
			{
				return new MongoDB.Bson.BsonDocument();
			}
		}
	}
}
