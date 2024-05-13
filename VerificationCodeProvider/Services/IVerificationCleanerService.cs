
namespace VerificationCodeProvider.Services
{
    public interface IVerificationCleanerService
    {
        Task RemoveExpiredRecordsAsync();
    }
}