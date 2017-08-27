using System;
namespace Stitch.Auth
{
    public struct FacebookAuthProvider : IAuthProvider
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
				return "facebook";
			}
		}

        private string _accessToken;

		MongoDB.Bson.BsonDocument IAuthProvider.Payload
		{
			get
			{
				return new MongoDB.Bson.BsonDocument
                {
                    { "accessToken", _accessToken}
                };
			}
		}

        public FacebookAuthProvider(string accessToken)
        {
            this._accessToken = accessToken;
        }
    }
}
