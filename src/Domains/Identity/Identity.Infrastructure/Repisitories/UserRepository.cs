using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Identity.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Identity.Infrastructure.Repositories
{
    public class UserRepository(IdentityDbContext context) : IUserRepository
    {
        private readonly IdentityDbContext _context = context;

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<Collection<User>> GetAllUserAsync()
        {
            // O filtro Active agora é automático pelo DbContext
            var usersList = await _context.Users.ToListAsync();
            return new Collection<User>(usersList);
        }

        public async Task<Collection<User>> GetAllUserByRolesAsync(string role)
        {
            // O filtro Active agora é automático
            var users = await _context.Users
                .Where(u => u.Role == role)
                .ToListAsync();

            return new Collection<User>(users);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            // O filtro Active agora é automático
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            // O filtro Active agora é automático
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Name == username);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await Task.CompletedTask;
        }

        public async Task DeleteUserAsync(User user)
        {
            // Chama o método de domínio que seta Active = false
            user.ChangeSituation();

            _context.Users.Update(user);
            await Task.CompletedTask;
        }
    }
}