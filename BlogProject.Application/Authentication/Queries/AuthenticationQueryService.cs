using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogProject.Application.Authentication.Common;

namespace BlogProject.Application.Authentication.Queries
{
    public class AuthenticationQueryService : IAuthenticationQueryService
    {
        public AuthenticationResult Login(string email, string password)
        {
            return new AuthenticationResult(email, password);
        }
    }
}
