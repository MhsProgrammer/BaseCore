

namespace BaseCore.Application.Exeptions
{
    public class BadRequestException : CustomException
    {
        public BadRequestException(string message) : base(message, 400)
        {


        }
    }
}
