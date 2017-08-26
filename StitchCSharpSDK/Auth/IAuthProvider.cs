using System;
using System.Collections.Generic;

namespace Stitch.Auth
{
    public interface IAuthProvider
    {
        string Type { get; }
        string ProviderName { get; }
        MongoDB.Bson.BsonDocument Payload { get; }
    }
}
