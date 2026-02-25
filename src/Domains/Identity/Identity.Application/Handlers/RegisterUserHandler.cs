using Identity.Application.Commands;
using Identity.Application.Queries;
using Identity.Domain.Entities;
using Identity.Domain.Exceptions;
using Identity.Domain.Interfaces;
using Identity.Infrastructure.Messaging;
using Identity.Infrastructure.Security;

namespace Identity.Application.Handlers
{
    public class RegisterUserHandler(IUnitOfWork uow, IPasswordHasher hasher, UserCreatedPublisher publisher, ITokenService tokenService)
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IPasswordHasher _hasher = hasher;
        private readonly ITokenService _tokenService = tokenService;
        private readonly UserCreatedPublisher _publisher = publisher;

        public async Task<RegisterUserQuery> Handle(RegisterUserCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Email))
                throw new ArgumentNullException(nameof(command.Email), "O e-mail é obrigatório.");

            if (string.IsNullOrWhiteSpace(command.Password))
                throw new ArgumentNullException(nameof(command.Password), "A senha é obrigatória.");
            var existingUser = await _uow.Users.GetByEmailAsync(command.Email);
            if (existingUser != null)
            {
                throw new ConflictException("Este e-mail já está cadastrado.");
            }
            var hash = _hasher.Hash(command.Password);
            var user = new User(command.Name, command.Email, hash, command.Role ?? "Customer");
            await _uow.Users.AddAsync(user);
            var success = await _uow.CommitAsync();

            if (!success)
                throw new InvalidOperationException("Não foi possível realizar o cadastro no momento.");
            _publisher.Publish(user);

            return new RegisterUserQuery
            {
                UserId = user.Id,
                UserName = user.Name,
                Email = user.Email,
                Role = user.Role,
                Token = _tokenService.GenerateToken(user)
            };
        }
    }
}
