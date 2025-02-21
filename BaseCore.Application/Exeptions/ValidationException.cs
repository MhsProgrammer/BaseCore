using FluentValidation.Results;


namespace BaseCore.Application.Exeptions
{
    public class ValidationException : CustomException
    {
        public List<string> ValidationErrors { get; set; }

        public ValidationException(ValidationResult validationResult)
            : base("اعتبارسنجی ناموفق", 400)
        {
            // فقط پیام‌های خطا رو استخراج می‌کنیم
            ValidationErrors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        }

        // در صورت استفاده مستقیم از لیست
        public ValidationException(List<ValidationFailure> failures)
            : base("اعتبارسنجی ناموفق", 400)
        {
            ValidationErrors = failures.Select(e => e.ErrorMessage).ToList();
        }
    }
}
