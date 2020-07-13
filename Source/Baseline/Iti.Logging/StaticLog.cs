using Iti.Baseline.Inversion;

namespace Iti.Baseline.Logging
{
    public static class StaticLog
    {
        private static ILogger _logger = null;
        private static readonly object LockObject = new object();

        public static ILogger Log
        {
            get
            {
                if (_logger == null)
                {
                    lock (LockObject)
                    {
                        _logger = IOC.TryResolveStatic<ILogger>(() => null);
                    }
                }

                return _logger;
            }
        }
    }
}