using Identity.Application.Commands;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;

namespace Identity.Application.Handlers
{
    public class RegisterUserHandler(IUserRepository repository, IPasswordHasher hasher)
    {
        private readonly IUserRepository _repository = repository;
        private readonly IPasswordHasher _hasher = hasher;
        public async Task<Guid> Handle(RegisterUserCommand command)
        {
            if(string.IsNullOrWhiteSpace(command.Email))
            {
                var existingUser = await _repository.GetByEmailAsync(command.Email!);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("Usuário já cadastrado.");
                }
            }
            if (string.IsNullOrWhiteSpace(command.Password))
            {
                throw new ArgumentException("Senha é obrigatória");
            }
            var hash = _hasher.Hash(command.Password!);
            var user = new User(command.Email!, hash, command.Role ?? "Custumer");
            await _repository.AddAsync(user);
            return user.Id;
        }
    }
}
