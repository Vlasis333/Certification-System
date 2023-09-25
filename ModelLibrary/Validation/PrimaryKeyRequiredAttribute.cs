using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ModelLibrary.Validation;

public class PrimaryKeyRequiredAttribute : RequiredAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var httpContextAccessor = (IHttpContextAccessor?)
            validationContext.GetService(typeof(IHttpContextAccessor));
        var httpMethod = httpContextAccessor?.HttpContext?.Request.Method;

        if (httpMethod != "POST" && value == null)
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}
