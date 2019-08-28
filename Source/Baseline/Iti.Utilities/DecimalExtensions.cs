using System;

namespace Iti.Utilities
{
    public static class DecimalExtensions
    {
        public static decimal? RoundTo(this decimal? d, int numDigits)
        {
            return d?.RoundTo(numDigits);
        }

        public static decimal RoundTo(this decimal d, int numDigits)
        {
            return Math.Round(d, numDigits);
        }

        public static double? RoundTo(this double? d, int numDigits)
        {
            return d?.RoundTo(numDigits);
        }

        public static double RoundTo(this double d, int numDigits)
        {
            return Math.Round(d, numDigits);
        }
    }
}