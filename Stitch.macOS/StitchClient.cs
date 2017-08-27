using System;
using Foundation;

namespace Stitch.macOS
{
    public sealed class StitchClient : StitchClientBase
    {
        private readonly NSUserDefaults _userDefaults =
            new NSUserDefaults("com.mongodb.stitch.sdk.UserDefaults");

        public StitchClient(string appId,
                            string baseUrl = "https://stitch.mongodb.com") :
        base(appId, baseUrl) {}

        protected override DeviceInfo GetDeviceInfo()
        {
            return new DeviceInfo
            {
                AppId = NSBundle.MainBundle
                                .InfoDictionary["CFBundleName"].ToString(),
                AppVersion = NSBundle.MainBundle
                                     .InfoDictionary["CFBundleVersion"].ToString(),
                Platform = "xamarin-macOS",
                PlatformVersion = new NSProcessInfo()
                    .OperatingSystemVersionString
            };
        }

        protected override string Get(string key, string def = "")
        {
            return _userDefaults.StringForKey(key);
        }

        protected override void Put(string key, string val)
        {
            _userDefaults.SetString(val, key);
        }

        protected override void Remove(string key)
        {
            _userDefaults.RemoveObject(key);
		}

        protected override bool Contains(string key)
        {
            return _userDefaults.StringForKey(key) != null;
        }
    }
}
