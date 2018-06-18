using Microsoft.Extensions.Logging;

namespace ServiceHub.User.Service
{
    public static class AppLoggerFactory
    {
        private static ILoggerFactory _Factory = null;
        private static string categoryName = "app";

        public static void ConfigureLogger(string _categoryName)
        {
            categoryName = _categoryName;
        }

        private static ILoggerFactory LoggerFactory
        {
            get
            {
                if (_Factory == null)
                {
                    _Factory = new LoggerFactory();
                }
                return _Factory;
            }
            set { _Factory = value; }
        }

        public static ILogger CreateLogger() => LoggerFactory.CreateLogger(categoryName);
    }
}
