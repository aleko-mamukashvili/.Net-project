using Shared.Models.Auth;

namespace BAL.Infrastructure;

public interface IAuthService
{
    public Task<string> GenerateJwtToken(User user);
}