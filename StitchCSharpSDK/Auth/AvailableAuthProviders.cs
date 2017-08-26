using System;
using System.Collections.Generic;

namespace Stitch.Auth
{
	/// Struct containing information about available providers
	public class AvailableAuthProviders
    {
        /// Info about the `AnonymousAuthProvider`
        public AnonymousAuthProviderInfo? AnonymousProviderInfo
        { get; private set; }

        /// Info about the `GoogleAuthProvider`
        public GoogleAuthProviderInfo? GoogleProviderInfo
        { get; private set; }

        /// Info about the `FacebookAuthProvider`
        public FacebookAuthProviderInfo? FacebookProviderInfo
        { get; private set; }

        /// Info about the `EmailPasswordAuthProvider`
        public EmailPasswordAuthProviderInfo? EmailPasswordProviderInfo
		{ get; private set; }

        public bool HasAnonymousProviderInfo 
        { get { return EmailPasswordProviderInfo != null; } }

        public bool HasGoogleProviderInfo 
        { get { return GoogleProviderInfo != null; } }

        public bool HasFacebookProviderInfo 
        { get { return FacebookProviderInfo != null; } }

		public bool HasEmailPassword 
        { get { return EmailPasswordProviderInfo != null; } }

        private const string GoogleName = "oauth/google";
        private const string FacebookName = "oauth/facebook";
        private const string EmailPassName = "local/userpass";
        private const string AnonymousName = "anon/user";

		internal AvailableAuthProviders(Dictionary<string, object> dictionary)
		{
            foreach (string providerName in dictionary.Keys) 
            {
                object infoDict;
                switch (providerName)
                {
                    case GoogleName:
                        if (dictionary.TryGetValue(providerName, out infoDict))
                        {
                            this.GoogleProviderInfo =
                                GoogleAuthProviderInfo.Builder(
                                        infoDict as Dictionary<string, object>);
                        }
                        break;
                    case FacebookName:
						if (dictionary.TryGetValue(providerName, out infoDict))
						{
							this.FacebookProviderInfo =
								FacebookAuthProviderInfo.Builder(
										infoDict as Dictionary<string, object>);
						}
                        break;
                    case EmailPassName:
                        this.EmailPasswordProviderInfo =
                            new EmailPasswordAuthProviderInfo();
                        break;
                    case AnonymousName:
                        this.AnonymousProviderInfo =
                            new AnonymousAuthProviderInfo();
                        break;
                    default:
                        break;
                }
			}
		}

        public override string ToString()
        {
            return string.Format("[AvailableAuthProviders: AnonymousAuthProviderInfo={0}, GoogleProviderInfo={1}, FacebookProviderInfo={2}, EmailPasswordProviderInfo={3}]", AnonymousProviderInfo, GoogleProviderInfo, FacebookProviderInfo, EmailPasswordProviderInfo);
        }
	}
}
