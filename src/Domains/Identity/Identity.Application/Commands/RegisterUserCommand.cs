using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Commands
{
    public class RegisterUserCommand
    {
        public string Name { get; set; }
        public required string Email { get; set;  }
        public required string Password { get; set; }
        public string? Role { get; set; }  // Admin, Manager, Seller, Customer
    }
}
