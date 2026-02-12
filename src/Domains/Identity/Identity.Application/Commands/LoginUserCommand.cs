using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Commands
{
    public class LoginUserCommand
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

}
