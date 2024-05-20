using BlogProject.Application;
using BlogProject.Domain;
using BlogProject.Infrastructure.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Infrastructure.Users.Persistance
{
    public class UsersRepository(UsersDbContext _usersDbContext, UserManager<BlogUser> _userManager) : IUsersRepository
    {
        public async Task AddUser(BlogUser user, CancellationToken ctoken)
        {
            await _usersDbContext.BlogUsers!.AddAsync(user, ctoken);
        }

        public async Task<bool> CheckIfEmailIsAvailable(string email, CancellationToken ctoken)
        {
            BlogUser? user = await _usersDbContext.BlogUsers!.FirstOrDefaultAsync(x => x.Email == email);
            return user == null;
        }
        public async Task<bool> CheckIfUsernameIsAvailable(string username, CancellationToken ctoken)
        {
            BlogUser? user = await _usersDbContext.BlogUsers!.FirstOrDefaultAsync(x => x.UserName == username);
            return user == null;
        }

        public async Task<BlogUser> GetUserByClaimPrincipal(ClaimsPrincipal? claimsPrincipal)
        {
            return await _userManager.GetUserAsync(claimsPrincipal);
        }

        public async Task<BlogUser> GetUserByID(uint id)
        {
            return await _usersDbContext.Users.FindAsync(id);
        }

        public async Task<BlogUser> GetUserByTag(string tag)
        {
            return await _usersDbContext.Users.FirstOrDefaultAsync(x => x.UserName == tag);
        }

        public BlogUser[] GetUsersByIDs(uint[] uints)
        {
            return _usersDbContext.Users.Where(p => uints.Contains(p.Id)).ToArray();
        }
    }
}
