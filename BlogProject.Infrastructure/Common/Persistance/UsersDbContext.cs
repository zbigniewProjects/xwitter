using BlogProject.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BlogProject.Infrastructure.Common
{
    public class UsersDbContext : IdentityDbContext<BlogUser, IdentityRole<uint>, uint>
    {
        protected readonly IConfiguration _configuration;
        public UsersDbContext(IConfiguration configuration, DbContextOptions<UsersDbContext> options) : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("UserbaseConnection"));
        } 

        public DbSet<BlogUser>? BlogUsers { get; set; }
    }
}
