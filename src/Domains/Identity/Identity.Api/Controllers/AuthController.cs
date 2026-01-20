using Identity.Application.Commands;
using Identity.Application.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(RegisterUserHandler resgisterHandler, LoginUserHandler loginUserHandler) : ControllerBase
    {
        private readonly RegisterUserHandler _registerHandler = resgisterHandler;
        private readonly LoginUserHandler _loginHandler = loginUserHandler;
        [HttpPost("register")]
        //[ProducesResponseType(200, Type = typeof(Object))]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var id = await _registerHandler.Handle(command);

            var response = new
            {
                UserId = id,
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


    }
}
