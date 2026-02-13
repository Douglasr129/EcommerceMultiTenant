using Identity.Application.Commands;
using Identity.Application.Queries;
using Identity.Domain.Entities;
using Identity.Domain.Exceptions;
using Identity.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Handlers
{
    public class UpdateUserHandler(IUnitOfWork uow)
    {
        private readonly IUnitOfWork _uow = uow;

        public async Task<GetUserQuery> Handle(UpdateUserCommand command)
        {
            // 1. Buscar o usuário atual via UoW
            var user = await _uow.Users.GetByEmailAsync(command.CurrentEmail)
                        ?? throw new NotFoundException($"Usuário com e-mail {command.CurrentEmail} não encontrado.");

            // 2. Validar conflito de e-mail se houver mudança
            if (command.NewEmail != command.CurrentEmail)
            {
                var emailConflict = await _uow.Users.GetByEmailAsync(command.NewEmail);
                if (emailConflict != null)
                {
                    throw new ConflictException("O novo e-mail informado já está em uso por outro usuário.");
                }
            }

            // 3. Aplicar as mudanças na entidade (Regra de Domínio)
            user.ChangeUser(command.Name, command.NewEmail, command.PasswordHash);

            // 4. Marcar para atualização no repositório
            await _uow.Users.UpdateUserAsync(user);

            // 5. Persistir as mudanças de forma atômica
            var success = await _uow.CommitAsync();

            if (!success)
            {
                // Se os dados enviados forem idênticos aos do banco, o EF retorna 0.
                // Dependendo da sua regra, você pode apenas seguir em frente ou avisar o usuário.
                throw new InvalidOperationException("Nenhuma alteração foi detectada ou salva.");
            }

            // 6. Retornar o DTO usando o objeto 'user' que já está atualizado
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
