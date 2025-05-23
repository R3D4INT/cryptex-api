﻿using CryptexApi.Enums;

namespace CryptexApi.Helpers
{
    public static class BinanceIntervalExtensions
    {
        public static string ToIntervalString(this BinanceInterval interval)
        {
            return interval.ToString().TrimStart('_');
        }
    }

}
