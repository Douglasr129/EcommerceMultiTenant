using Identity.Application.Commands;
using Identity.Domain.Interfaces;



namespace Identity.Application.Handlers
{
    public class LoginUserHandler(IUnitOfWork uow, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IPasswordHasher _hasher = passwordHasher;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<string> Handle(LoginUserCommand command)
        {
            // 1. Busca o usuário via Unit of Work
            // O repositório já filtra por Active = true
            var user = await _uow.Users.GetByEmailAsync(command.Email);

            // 2. Validação de segurança (E-mail e Senha)
            // Se o usuário não existe OU a senha está errada, lançamos a mesma exceção
            if (user == null || !_hasher.Verify(command.Password, user.PasswordHash))
            {
                // O Middleware global transformará isso em um HTTP 401 ou 403
                throw new UnauthorizedAccessException("E-mail ou senha incorretos.");
            }

            // 3. Geração do Token
            // Passamos o objeto 'user' que já contém o Nome e a Role para o Token
            return _tokenService.GenerateToken(user);
        }
    }
}
