using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Commands
{
    public class RegisterUserCommand
    {
        public string? Email { get; set;  }
        public string? Password { get; set; }
        public string? Role { get; set; }  // Admin, Manager, Seller, Customer
    }
}
