using Identity.Domain.Entities;
using System.Collections.ObjectModel;

namespace Identity.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<Collection<User>> GetAllUserAsync();
        Task<Collection<User>> GetAllUserByRolesAsync(string role);
        Task<User?> UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);

    }
}
