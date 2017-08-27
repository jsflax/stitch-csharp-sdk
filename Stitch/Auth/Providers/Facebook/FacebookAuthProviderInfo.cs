using System;
using System.Collections.Generic;

namespace Stitch.Auth
{
    public struct FacebookAuthProviderInfo
    {
        /// Id of your Facebook app
        public string AppId
        { get; private set; }

        /// Scopes enabled for this app
        public string[] Scopes
        { get; private set; }

        private FacebookAuthProviderInfo(string appId,
                                         string[] scopes)
        {
            this.AppId = appId;
            this.Scopes = scopes;
        }

        internal static FacebookAuthProviderInfo? Builder(
            Dictionary<string, object> dictionary
        ) {
            object appId;
            dictionary.TryGetValue("clientId", out appId);

            if (appId == null || !(appId is string)) {
                return null;
            }

			var scopes = dictionary["metadataFields"] as string[];

            return new FacebookAuthProviderInfo(appId as string, scopes);
        }
    }
}
