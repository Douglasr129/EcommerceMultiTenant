using Identity.Domain.Interfaces;
using Identity.Infrastructure.Configurations;
using Identity.Infrastructure.Repositories;

namespace Identity.Infrastructure.Repisitories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IdentityDbContext _context;
        private IUserRepository _userRepository;

        public UnitOfWork(IdentityDbContext context)
        {
            _context = context;
        }

        // Usamos o padrão "Lazy Loading" para o repositório
        public IUserRepository Users => _userRepository ??= new UserRepository(_context);

        public async Task<bool> CommitAsync()
        {
            // Retorna true se pelo menos uma linha foi afetada no banco
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
