namespace ITI.Baseline.ValueObjects
{
    public static class FieldLengths
    {
        public class EmailAddress
        {
            public const int Value = 256;
        }

        public class PhoneNumber
        {
            public const int Value = 16;
        }

        public class TimeZone
        {
            public const int Value = 64;
        }

        public class GeoLocation
        {
            public const int Source = 16;
            public const int Status = 64;
            public const int LocationType = 64;
            public const int FormattedAddress = 128;
        }
    }
}
