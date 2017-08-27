using System;
using Newtonsoft.Json;

namespace Stitch
{
    struct RefreshTokenHolder
    {
		[JsonProperty(PropertyName = "refreshToken",
					  NullValueHandling = NullValueHandling.Ignore)]
        internal string RefreshToken;
    }
}
