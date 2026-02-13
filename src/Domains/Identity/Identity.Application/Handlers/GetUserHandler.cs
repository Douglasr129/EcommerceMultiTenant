using Identity.Application.Queries;
using Identity.Domain.Entities;
using Identity.Domain.Exceptions;
using Identity.Domain.Interfaces;


namespace Identity.Application.Handlers
{
    public class GetUserHandler(IUnitOfWork uow)
    {
        private readonly IUnitOfWork _uow = uow;

        public async Task<GetUserQuery> Handle(string email)
        {
            // 1. Validação básica de entrada
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email), "O e-mail deve ser informado.");

            // 2. Acesso ao repositório via Unit of Work
            var user = await _uow.Users.GetByEmailAsync(email);

            // 3. Validação de existência (Lança 404 via Middleware)
            if (user == null)
                throw new NotFoundException($"O usuário com e-mail {email} não foi encontrado ou está inativo.");

            // 4. Mapeamento para o DTO de resposta
            return new GetUserQuery
            {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.Name,
                Role = user.Role
            };
        }
    }
}
