using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.DataContext
{
    public static class TestAppFieldLengths
    {
        public static class SimpleAddress
        {
            public const int Line1 = 64;
            public const int Line2 = 64;
            public const int City = 64;
            public const int State = 16;
            public const int Zip = 16;
        }

        public static class SimplePersonName
        {
            public const int Prefix = 16;
            public const int First = 64;
            public const int Middle = 64;
            public const int Last = 64;
        }
    }
}
