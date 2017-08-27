using System;
using System.Collections.Generic;

namespace Stitch.Auth
{
    public struct GoogleAuthProviderInfo
    {
        /// Id of your Facebook app
        public string ClientId
        { get; private set; }

        /// Scopes enabled for this app
        public string[] Scopes
        { get; private set; }

        private GoogleAuthProviderInfo(string clientId,
                                         string[] scopes)
        {
            this.ClientId = clientId;
            this.Scopes = scopes;
        }

        internal static GoogleAuthProviderInfo? Builder(
            Dictionary<string, object> dictionary
        )
        {
            object clientId;
            dictionary.TryGetValue("clientId", out clientId);

            if (clientId == null || !(clientId is string))
            {
                return null;
            }

            var scopes = dictionary["metadataFields"] as string[];

            return new GoogleAuthProviderInfo(clientId as string, scopes);
        }
    }
}
