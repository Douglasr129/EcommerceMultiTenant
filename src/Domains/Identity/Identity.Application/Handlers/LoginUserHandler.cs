using Identity.Application.Commands;
using Identity.Domain.Interfaces;



namespace Identity.Application.Handlers
{
    public class LoginUserHandler(IUserRepository repository, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        private readonly IUserRepository _repository = repository;
        private readonly IPasswordHasher _hasher = passwordHasher;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<string> Handle(LoginUserCommand command) 
        { 
            var user = await _repository.GetByEmailAsync(command.Email) ?? throw new UnauthorizedAccessException("Usuário não encontrado");
            if (!_hasher.Verify(command.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Senha inválida.");
            return _tokenService.GenerateToken(user);

        }
    }
}
