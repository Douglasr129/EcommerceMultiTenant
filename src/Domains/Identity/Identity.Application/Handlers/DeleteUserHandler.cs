using Identity.Domain.Entities;
using Identity.Domain.Exceptions;
using Identity.Domain.Interfaces;

namespace Identity.Application.Handlers
{
    public class DeleteUserHandler(IUnitOfWork uow)
    {
        private readonly IUnitOfWork _uow = uow;

        public async Task Handle(string email, User executor)
        {
            // 1. Busca o alvo
            var target = await _uow.Users.GetByEmailAsync(email)
                ?? throw new NotFoundException($"Usuário com e-mail {email} não encontrado.");

            // 2. Validação de permissão (Sua lógica está perfeita aqui)
            bool canDelete = false;

            if (executor.Role == "Admin")
                canDelete = true;
            else if (executor.Role == "Gerente")
            {
                if (target.Id == executor.Id || target.Role == "Vendedor")
                    canDelete = true;
            }
            else if (target.Id == executor.Id)
            {
                canDelete = true;
            }

            if (!canDelete)
                throw new UnauthorizedAccessException("Você não tem permissão para desativar este usuário.");

            // 3. Prepara a alteração no contexto
            await _uow.Users.DeleteUserAsync(target);

            // 4. ESSENCIAL: Salva as alterações no banco de dados
            var success = await _uow.CommitAsync();

            if (!success)
                throw new InvalidOperationException("Não foi possível persistir a desativação no banco de dados.");
        }
    }
}
