using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Identity.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

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

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u=> u.Email == email);
        }
    }
}
