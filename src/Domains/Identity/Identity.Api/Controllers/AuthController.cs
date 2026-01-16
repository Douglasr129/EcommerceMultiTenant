using Identity.Application.Commands;
using Identity.Application.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(RegisterUserHandler resgisterHandler, LoginUserHandler loginUserHandler) : ControllerBase
    {
        private readonly RegisterUserHandler _resgisterHandler = resgisterHandler;
        private readonly LoginUserHandler _loginHandler = loginUserHandler;
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var id = await _resgisterHandler.Handle(command);
            return Ok(new { UserId = id });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var token = await _loginHandler.Handle(command);
            return Ok(new { Token = token });
        }

    }
}
