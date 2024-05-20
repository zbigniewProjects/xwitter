using BlogProject.Domain;
using System.Security.Claims;

namespace BlogProject.Application
{
    public interface IUsersRepository
    {
        Task AddUser(BlogUser user, CancellationToken ctoken);
        Task<bool> CheckIfEmailIsAvailable(string email, CancellationToken ctoken);
        Task<bool> CheckIfUsernameIsAvailable(string username, CancellationToken ctoken);

        Task<BlogUser> GetUserByID(uint id);

        Task<BlogUser> GetUserByClaimPrincipal(ClaimsPrincipal claimsPrincipal);
        BlogUser[] GetUsersByIDs(uint[] uints);
        Task<BlogUser> GetUserByTag(string tag);
    }
}