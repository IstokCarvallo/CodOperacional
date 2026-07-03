using FrontCodOperacional.Models.Account;

namespace FrontCodOperacional.Services.Auth
{
    public interface IAccountService
    {
        Task<bool> ChangePasswordAsync(ChangePasswordRequest request);
    }
}
