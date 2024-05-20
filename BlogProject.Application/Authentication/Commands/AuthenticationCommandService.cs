using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogProject.Application.Authentication.Common;

namespace BlogProject.Application.Authentication.Commands
{
    internal class AuthenticationCommandService : IAuthenticationCommandService
    {
        public AuthenticationCommandService() { }

        public AuthenticationResult RegisterUser(string username, string email, string password)
        {
            return new AuthenticationResult(username, email);
        }
    }
}
