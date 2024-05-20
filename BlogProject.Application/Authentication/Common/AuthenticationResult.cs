using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.Authentication.Common
{
    public record AuthenticationResult(
        string Username,
        string Email
        );
}
