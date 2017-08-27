using System;
namespace Stitch
{
    public sealed class StitchClientImpl : StitchClientBase
    {
        private readonly KeyValueStore _keyValueStore;
        private readonly DeviceInfo _deviceInfo;

        public StitchClientImpl(string appId,
                                KeyValueStore keyValueStore,
                                DeviceInfo deviceInfo,
                                string baseUrl = "https://stitch.mongodb.com") :
        base(appId, baseUrl)
        {
            this._keyValueStore = keyValueStore;
            this._deviceInfo = deviceInfo;
        }

        protected override DeviceInfo GetDeviceInfo()
        {
            return _deviceInfo;
        }

        protected internal override string Get(string key, string def = "")
        {
            return _keyValueStore.Get(key, def);
        }

        protected internal override void Put(string key, string val)
        {
			_keyValueStore.Put(key, val);
		}

        protected internal override void Remove(string key)
        {
            _keyValueStore.Remove(key);
        }

        protected internal override bool Contains(string key)
        {
            return _keyValueStore.Contains(key);
        }
    }
}
