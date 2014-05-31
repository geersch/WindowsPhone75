using System;
using System.IO;
using System.Net;
using System.Threading;

namespace StackExchange.Api
{
    public static class ExtensionMethods
    {
        public static DateTime FromUnixTime(this Int64 self)
        {
            var ret = new DateTime(1970, 1, 1);
            return ret.AddSeconds(self);
        }

        public static Int64 ToUnixTime(this DateTime self)
        {
            var epoc = new DateTime(1970, 1, 1);
            var delta = self - epoc;

            if (delta.TotalSeconds < 0)
            {
                throw new ArgumentOutOfRangeException("Unix epoc starts January 1st, 1970");
            }

            return (long)delta.TotalSeconds;
        }
    }
}