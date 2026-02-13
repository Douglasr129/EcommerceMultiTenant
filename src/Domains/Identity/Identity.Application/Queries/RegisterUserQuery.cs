using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Queries
{
    public class RegisterUserQuery
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
