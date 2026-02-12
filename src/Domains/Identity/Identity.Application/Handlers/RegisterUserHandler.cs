using Identity.Application.Commands;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Identity.Infrastructure.Messaging;

namespace Identity.Application.Handlers
{
    public class RegisterUserHandler(IUserRepository repository, IPasswordHasher hasher, UserCreatedPublisher publisher)
    {
        private readonly IUserRepository _repository = repository;
        private readonly IPasswordHasher _hasher = hasher;
        private readonly UserCreatedPublisher _publisher = publisher;

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
            var user = new User(command.Name, command.Email!, hash, command.Role ?? "Custumer");
            await _repository.AddAsync(user);

            // Publica evento no RabbitMQ
            _publisher.Publish(user);

            return user.Id;
        }
    }
}
