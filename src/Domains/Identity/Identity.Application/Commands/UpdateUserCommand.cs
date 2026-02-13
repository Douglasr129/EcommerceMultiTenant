using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Commands
{
    public class UpdateUserCommand
    {
        public string CurrentEmail { get; set;}
        public string NewEmail { get; set;}
        public string Name { get; set;}
        public string Role { get; set;}
        public string PasswordHash { get; set; }
    }
}
