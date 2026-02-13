using Identity.Application.Queries;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Handlers
{
    public class GetAllUserHandler(IUnitOfWork uow)
    {
        private readonly IUnitOfWork _uow = uow;

        public async Task<IEnumerable<GetUserQuery>> Handle(User executor)
        {
            IEnumerable<User> users;

            // 1. Regra de Negócio e Hierarquia
            if (executor.Role == "Admin")
            {
                // Admin lista todos os ativos (o filtro global já cuida do Active)
                users = await _uow.Users.GetAllUserAsync();
            }
            else if (executor.Role == "Gerente")
            {
                // Gerente lista apenas Vendedores (e ativos)
                users = await _uow.Users.GetAllUserByRolesAsync("Vendedor");
            }
            else
            {
                // Cliente ou qualquer outra role não permitida
                throw new UnauthorizedAccessException("Você não tem permissão para listar usuários.");
            }

            // 2. Mapeamento para o DTO (GetUserQuery)
            return users.Select(user => new GetUserQuery
            {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.Name,
                Role = user.Role
            });
        }
    }
}
