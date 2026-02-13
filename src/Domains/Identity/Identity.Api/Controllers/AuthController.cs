using Identity.Application.Commands;
using Identity.Application.Handlers;
using Identity.Domain.Entities;
using Identity.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
                        RegisterUserHandler resgisterHandler,
                        LoginUserHandler loginUserHandler,
                        UpdateRoleHandler updateRoleHandler,
                        GetUserHandler getUserHandle,
                        UpdateUserHandler updateUserHandler,
                        DeleteUserHandler deleteUserHandler,
                        GetAllUserHandler getAllUserHandler) : ControllerBase
    {
        private readonly RegisterUserHandler _registerHandler = resgisterHandler;
        private readonly LoginUserHandler _loginHandler = loginUserHandler;
        private readonly UpdateRoleHandler _updateRoleHandler = updateRoleHandler;
        private readonly UpdateUserHandler _updateUserHandler = updateUserHandler;
        private readonly GetUserHandler _getUserHandler = getUserHandle;
        private readonly DeleteUserHandler _deleteUserHandler = deleteUserHandler;
        private readonly GetAllUserHandler _getAllUserHandler = getAllUserHandler;
        [HttpPost("register")]
        //[ProducesResponseType(200, Type = typeof(Object))]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var id = await _registerHandler.Handle(command);
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return BadRequest(new { error = "" });
            }
            var commandlogin = new LoginUserCommand
            {
                Email = command.Email,
                Password = command.Password
            };
            var token = await _loginHandler.Handle(commandlogin);
            var response = new
            {
                UserId = id,
                Token = token,
                Links = new[]
                {
                    new { Rel = "self", Href = $"/api/auth/register" },
                    new { Rel = "login", Href = $"/api/auth/login" }
                }
            };

            return Ok(response);

        }
        [HttpPost("login")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var token = await _loginHandler.Handle(command);
            var response = new
            {
                Token = token,
                Links = new[]
                {
                    new { Rel = "self", Href = $"/api/auth/login" },
                    new { Rel = "admin-only", Href = $"/api/auth/admin-only" },
                    new { Rel = "manager-only", Href = $"/api/auth/manager-only" }
                }
            };

            return Ok(response);

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        [ProducesResponseType(200)]
        public IActionResult AdminEndpoint()
        {
            return Ok("Acesso permitido apenas para Admin.");
        }

        [Authorize(Policy = "ManagerPolicy")]
        [HttpGet("manager-only")]
        [ProducesResponseType(200)]
        public IActionResult ManagerEndpoint()
        {
            return Ok("Acesso permitido apenas para Manager.");
        }

        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "manager")]
        [HttpPut("update-role")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleCommand command)
        {
            // Pega o e-mail do usuário logado através do Token JWT
            var executorEmail = User.Identity?.Name;
            var executor = await _getUserHandler.Handle(executorEmail!);

            var result = await _updateRoleHandler.Handle(command, new User(executor.UserName, executor.Email, executor.Email, executor.Role));
            return Ok(result);
        }
        [HttpPut("update")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
        {
            var executorEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(executorEmail)) return Unauthorized();
            var executor = await _getUserHandler.Handle(executorEmail);
            if (executor == null) return Unauthorized();
            var target = await _getUserHandler.Handle(command.CurrentEmail);
            if (target == null) throw new NotFoundException("Usuário alvo não encontrado.");
            bool canModify = false;
            if (executor.Role == "Admin")
                canModify = true;
            else if (executor.UserId == target.UserId)
                canModify = true;
            else if (executor.Role == "Gerente" && target.Role == "Vendedor")
            {
                canModify = true;
            }
            if (!canModify)
            {
                return Forbid("Você não tem permissão para alterar este usuário.");
            }
            var result = await _updateUserHandler.Handle(command);
            return Ok(result);
        }
        [Authorize]
        [HttpDelete("delete")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete([FromBody] string command)
        {
            // 1. Identifica quem está logado
            var executorEmail = User.Identity?.Name;
            var executor = await _getUserHandler.Handle(executorEmail!);

            if (executor == null) return Unauthorized();

            // 2. Chama o Handler passando o comando e quem está executando
             await _deleteUserHandler.Handle(command, new User(executor.UserName, executor.Email, executor.Email, executor.Role));

            return NoContent();
        }
        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            // 1. Identifica o executor através do Token
            var executorEmail = User.Identity?.Name;
            var executor = await _getUserHandler.Handle(executorEmail!);

            if (executor == null) return Unauthorized();

            // 2. Chama o Handler
            var result = await _getAllUserHandler.Handle(new User(executor.UserName, executor.Email, executor.Email, executor.Role));

            return Ok(result);
        }


    }
}
