using System;
namespace Stitch.Auth
{
    public struct GoogleAuthProvider : IAuthProvider
    {
		string IAuthProvider.Type
		{
			get
			{
				return "oauth2";
			}
		}

		string IAuthProvider.ProviderName
		{
			get
			{
				return "google";
			}
		}

		private string _authCode;

		MongoDB.Bson.BsonDocument IAuthProvider.Payload
		{
			get
			{
				return new MongoDB.Bson.BsonDocument
				{
					{ "authCode", _authCode}
				};
			}
		}

		public GoogleAuthProvider(string authCode)
		{
			this._authCode = authCode;
		}
    }
}
