

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VerificationCodeProvider.Data.Contexts;
using VerificationCodeProvider.Models;

namespace VerificationCodeProvider.Services;

public class ValidateVerificationCodeService(ILogger<ValidateVerificationCodeService> logger, DataContext context) : IValidateVerificationCodeService
{
    private readonly ILogger<ValidateVerificationCodeService> _logger = logger;
    private readonly DataContext _context = context;


    public async Task<bool> ValidateCodeAsync(ValidateRequest validateRequest)
    {
        try
        {
            var entity = await _context.VerificationRequests.FirstOrDefaultAsync(X => X.Email == validateRequest.Email && X.Code == validateRequest.Code);
            if (entity != null)
            {
                _context.VerificationRequests.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR: ValidateVerificationCode.ValidateCodeAsync() ::  {ex.Message}");
        }
        return false;
    }

    public async Task<ValidateRequest> UnPackValidateRequestAsync(HttpRequest req)
    {
        try
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            if (!string.IsNullOrEmpty(body))
            {
                var validate = JsonConvert.DeserializeObject<ValidateRequest>(body);
                if (validate != null)
                    return validate;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR: ValidateVerificationCode.UnPackValidateRequestAsync() :: {ex.Message}");
        }

        return null!;


    }


}
