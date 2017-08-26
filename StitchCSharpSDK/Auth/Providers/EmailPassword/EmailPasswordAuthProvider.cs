namespace Stitch.Auth
{
	public struct EmailPasswordAuthProvider : IAuthProvider
	{
		private readonly string _email, _password;

		public EmailPasswordAuthProvider(string email, string password)
		{
			this._email = email;
			this._password = password;
		}

		string IAuthProvider.Type
		{
			get
			{
				return "local";
			}
		}

		string IAuthProvider.ProviderName
		{
			get
			{
				return "userpass";
			}
		}

		MongoDB.Bson.BsonDocument IAuthProvider.Payload
		{
			get
			{
				return new MongoDB.Bson.BsonDocument
				{
					{ "username", this._email },
					{ "password", this._password }
				};
			}
		}

		internal MongoDB.Bson.BsonDocument RegistrationPayload
		{
			get
			{
				return new MongoDB.Bson.BsonDocument()
				{
					{ "email", this._email },
					{ "password", this._password }
				};
			}
		}
	}
}
