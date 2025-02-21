

namespace BaseCore.Application.Exeptions
{
    public class CustomException : Exception
    {
        public int StatusCode { get; }


        
        protected CustomException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public CustomException(string? message) : base(message)
        {
        }
    }
}
