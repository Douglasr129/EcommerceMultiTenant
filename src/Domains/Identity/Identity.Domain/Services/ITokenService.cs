using Identity.Domain.Entities;

namespace Identity.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }

}
