using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Commands
{
    public record UpdateRoleCommand(string UserEmail, string NewRole);
}
