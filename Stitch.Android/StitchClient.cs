using Android.App;
using Android.Content;
using Android.OS;

namespace Stitch.Android
{
    public sealed class StitchClient : StitchClientBase
    {
		private const string SharedPreferencesName =
            "com.mongodb.stitch.sdk.SharedPreferences.{0}";
        
        private readonly ISharedPreferences _sharedPreferences;
        private readonly Context _context;

		public StitchClient(Context context,
                            string appId,
                            string baseUrl = "https://stitch.mongodb.com") :
        base(appId, baseUrl)
        {
            this._context = context;
            _sharedPreferences = _context.GetSharedPreferences(
                string.Format(SharedPreferencesName, appId),
                FileCreationMode.Private
            );
        }

        protected override DeviceInfo GetDeviceInfo()
        {
            var appVersion = Application
                .Context.ApplicationContext
                .PackageManager.GetPackageInfo(
                    Application.Context.ApplicationContext.PackageName, 0
                ).VersionName;

            return new DeviceInfo
            {
                AppVersion = appVersion,
                AppId = Application.Context.ApplicationContext.PackageName,
                Platform = "xamarin-android",
                PlatformVersion = Build.VERSION.Release
            };
        }

        protected override string Get(string key, string def = "")
        {
            return _sharedPreferences.GetString(key, def);
        }

        protected override void Put(string key, string val)
        {
            _sharedPreferences.Edit().PutString(key, val).Apply();
        }

        protected override void Remove(string key)
        {
			_sharedPreferences.Edit().Remove(key).Apply();
		}

        protected override bool Contains(string key)
        {
            return _sharedPreferences.Contains(key);
        }
    }
}
