using BlogProject.Application.Authentication.Common;

namespace BlogProject.Application.Authentication.Queries
{
    public interface IAuthenticationQueryService
    {
        AuthenticationResult Login(string email, string password);
    }
}