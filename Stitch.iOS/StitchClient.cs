using System;
using Foundation;
using UIKit;

namespace Stitch.iOS
{
    public sealed class StitchClient : StitchClientBase
    {
		private readonly NSUserDefaults _userDefaults =
			new NSUserDefaults("com.mongodb.stitch.sdk.UserDefaults");

		public StitchClient(string appId,
							string baseUrl = "https://stitch.mongodb.com") :
		base(appId, baseUrl)
		{}

		protected override DeviceInfo GetDeviceInfo()
		{
			return new DeviceInfo
			{
				AppId = NSBundle.MainBundle
								.InfoDictionary["CFBundleName"].ToString(),
				AppVersion = NSBundle.MainBundle
									 .InfoDictionary["CFBundleVersion"].ToString(),
				Platform = "xamarin-iOS",
				PlatformVersion = UIDevice.CurrentDevice.SystemVersion
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
