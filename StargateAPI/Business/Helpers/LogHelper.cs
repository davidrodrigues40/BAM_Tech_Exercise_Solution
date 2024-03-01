namespace StargateAPI.Business.Helpers
{
    public interface ILogHelper
    {
        void LogSuccess(string className, string methodName);
        void LogException(string className, string methodName, Exception e);
        void LogBadRequest(string className, string methodName, BadHttpRequestException e);
    }
    public class LogHelper : ILogHelper
    {
        private readonly ILogger _logger;
        private const string _logPrefix = "{className}: {methodName} - {message}";

        public LogHelper(ILogger logger)
        {
            _logger = logger;
        }

        public void LogSuccess(string className, string methodName)
        {
            const string message = "Success";
            _logger.LogInformation(_logPrefix, className, methodName, message);
        }

        public void LogException(string className, string methodName, Exception e)
        {
            string message = "Exception";
            _logger.LogCritical(e, _logPrefix, className, methodName, message);
        }

        public void LogBadRequest(string className, string methodName, BadHttpRequestException e)
        {
            string message = "Bad Request";
            _logger.LogError(e, _logPrefix, className, methodName, message);
        }
    }
}
