using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VerificationCodeProvider.Data.Contexts;
using VerificationCodeProvider.Services;

namespace VerificationCodeProvider.Functions
{
    public class ValidateVerificationCode(ILogger<ValidateVerificationCode> logger, DataContext context, IValidateVerificationCodeService validateVerificationCodeService)
    {
        private readonly ILogger<ValidateVerificationCode> _logger = logger;
        private readonly DataContext _context = context;
        private readonly IValidateVerificationCodeService _validateVerificationCodeService = validateVerificationCodeService;

        [Function("ValidateVerificationCode")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "verify")] HttpRequest req)
        {

            try
            {
                var validateRequest = await _validateVerificationCodeService.UnPackValidateRequestAsync(req);
                if (validateRequest != null)
                {
                    var validateResult = await _validateVerificationCodeService.ValidateCodeAsync(validateRequest);
                    if (validateResult)
                    {
                        return new OkResult();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR: ValidateVerificationCode.Run() ::  {ex.Message}");
            }

            return new UnauthorizedResult();    
            
        }


    }
}
