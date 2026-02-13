using Identity.Application.Commands;
using Identity.Application.Queries;
using Identity.Domain.Entities;
using Identity.Domain.Exceptions;
using Identity.Domain.Interfaces;
namespace Identity.Application.Handlers
{
    public class UpdateRoleHandler(IUnitOfWork uow)
    {
        private readonly IUnitOfWork _uow = uow;

        public async Task<GetUserQuery> Handle(UpdateRoleCommand command, User executor)
        {
            // 1. Validação de Segurança
            if (executor.Role != "Admin")
            {
                throw new UnauthorizedAccessException("Apenas administradores podem alterar cargos de usuários.");
            }

            // 2. Buscar o usuário alvo via Unit of Work
            var targetUser = await _uow.Users.GetByEmailAsync(command.UserEmail)
                ?? throw new NotFoundException($"Usuário {command.UserEmail} não encontrado.");

            // 3. Aplicar a mudança na entidade (Regra de Negócio)
            targetUser.ChangeRule(command.NewRole);

            // 4. Marcar para atualização no Repositório
            await _uow.Users.UpdateUserAsync(targetUser);

            // 5. Persistir no Banco de Dados
            var success = await _uow.CommitAsync();

            if (!success)
            {
                // Se o Admin tentou mudar para a mesma Role que já existia, 
                // o EF pode entender que não houve mudanças e retornar false.
                // Você pode decidir se lança erro ou apenas segue em frente.
                throw new InvalidOperationException("Nenhuma alteração foi realizada no banco de dados.");
            }

            // 6. Retornar o DTO
            return new GetUserQuery
            {
                UserId = targetUser.Id,
                Email = targetUser.Email,
                UserName = targetUser.Name,
                Role = targetUser.Role
            };
        }
    }
}
