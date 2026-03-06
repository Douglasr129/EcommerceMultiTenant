using Identity.Application.Commands;
using Identity.Application.Queries;
using Identity.Domain.Interfaces;



namespace Identity.Application.Handlers
{
    public class LoginUserHandler(IUnitOfWork uow, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IPasswordHasher _hasher = passwordHasher;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<RegisterUserQuery> Handle(LoginUserCommand command)
        {
            var user = await _uow.Users.GetByEmailAsync(command.Email);
            if (user == null || !_hasher.Verify(command.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("E-mail ou senha incorretos.");
            }
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
