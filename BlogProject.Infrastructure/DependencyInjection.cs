using BlogProject.Application;
using BlogProject.Domain;
using BlogProject.Infrastructure.Common;
using BlogProject.Infrastructure.Posts.Persistance;
using BlogProject.Infrastructure.Users.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using BlogProject.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace BlogProject.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddClearInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<PostsDbContext>();
            services.AddDbContext<UsersDbContext>();

            services.AddScoped<IPostsRepository, PostsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();

            services.AddIdentity<BlogUser, IdentityRole<uint>>(options =>
            {
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireLowercase = false;
            }
            ).AddEntityFrameworkStores<UsersDbContext>().AddDefaultTokenProviders();
            return services;
        }
    }
}
