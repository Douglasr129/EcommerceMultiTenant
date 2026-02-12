using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Identity.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Identity.Domain.Exceptions;

namespace Identity.Infrastructure.Repisitories
{
    public class UserRepository(IdentityDbContext context) : IUserRepository
    {
        private readonly IdentityDbContext _context = context;

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Collection<User>> GetAllUserAsync()
        {
            var usersList = await _context.Users
                .Where(u => u.Active)
                .ToListAsync();
            return new Collection<User>(usersList);
        }
        public async Task<Collection<User>> GetAllUserByRolesAsync(string role)
        {
            var users = await _context.Users
                .Where(u => u.Active && u.Role == role)
                .ToListAsync();

            return new Collection<User>(users);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                     .FirstOrDefaultAsync(u => u.Email == email && u.Active);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                     .FirstOrDefaultAsync(u => u.Name == username && u.Active);
        }

        public async Task<User?> UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users
             .FirstOrDefaultAsync(u => u.Id == user.Id && u.Active);
            if (existingUser == null || !existingUser.Active) throw new DomainException("Usuário não encontrado");


            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task DeleteUserAsync(User user)
        {
            var existingUser = await _context.Users
                         .FirstOrDefaultAsync(u => u.Id == user.Id && u.Active);
            if (existingUser == null || !existingUser.Active) throw new DomainException("Usuário não encontrado");
            
            existingUser.ChangeSituation();
            _context.Users.Update(existingUser!);
            await _context.SaveChangesAsync();
        }
    }
}
