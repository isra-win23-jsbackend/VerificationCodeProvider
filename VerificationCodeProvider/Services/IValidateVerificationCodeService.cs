using Microsoft.AspNetCore.Http;
using VerificationCodeProvider.Models;

namespace VerificationCodeProvider.Services
{
    public interface IValidateVerificationCodeService
    {
        Task<ValidateRequest> UnPackValidateRequestAsync(HttpRequest req);
        Task<bool> ValidateCodeAsync(ValidateRequest validateRequest);
    }
}