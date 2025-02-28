

namespace BaseCore.Application.Exeptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string name, object key)
            : base($"{name} ({key}) یافت نشد!")
        {
        }
    }
}
