

namespace BaseCore.Application.Exeptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string name, object key)
            : base($"{name} ({key}) is not found")
        {
        }
    }
}
