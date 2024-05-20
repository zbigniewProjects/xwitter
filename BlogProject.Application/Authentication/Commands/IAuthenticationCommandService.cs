using BlogProject.Application.Authentication.Common;

namespace BlogProject.Application.Authentication.Commands
{
    internal interface IAuthenticationCommandService
    {
        AuthenticationResult RegisterUser(string username, string email, string password);
    }
}