using Identity.Domain.Entities;

namespace Identity.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByEmailAsync(string email);

    }
}
