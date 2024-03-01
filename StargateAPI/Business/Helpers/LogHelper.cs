namespace StargateAPI.Business.Helpers
{
    public interface ILogHelper
    {
        string CreateSuccessLogPrefix(string className, string methodName);
        string CreateExceptionLogPrefix(string className, string methodName);
        string CreateBadRequestLogPrefix(string className, string methodName);
    }
    public class LogHelper : ILogHelper
    {
        public string CreateSuccessLogPrefix(string className, string methodName)
        {
            return CreateLogPrefix(className, methodName, "Success");
        }

        public string CreateExceptionLogPrefix(string className, string methodName)
        {
            return CreateLogPrefix(className, methodName, "Exception");
        }

        public string CreateBadRequestLogPrefix(string className, string methodName)
        {
            return CreateLogPrefix(className, methodName, "Bad Request");
        }

        private string CreateLogPrefix(string className, string methodName, string message)
        {
            return $"{className}: {methodName} - {message}";
        }
    }
}
