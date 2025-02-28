

namespace BaseCore.Application.Responses
{
    public class BaseApiResponse<T>
    {
        public bool Success { get; set; } 
        public string? Message { get; set; } 
        public T? Data { get; set; } 
        public List<string>? Errors { get; set; }


        public BaseApiResponse(string message = null)
        {
            Success = true;
            Message = message ?? "عملیات موفقیت‌آمیز بود.";
            Data = default;
            Errors = null;
        }

        public BaseApiResponse(T data, string message = null)
        {
            Success = true;
            Message = message ?? "عملیات موفقیت‌آمیز بود.";
            Data = data;
            Errors = null;
        }

       

        public BaseApiResponse(string message, List<string> errors = null)
        {
            Success = false;
            Message = message;
            Data = default;
            Errors = errors;
        }
    }
}
