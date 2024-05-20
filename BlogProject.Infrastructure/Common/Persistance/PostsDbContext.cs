using BlogProject.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Xml;

namespace BlogProject.Infrastructure.Common;

public class PostsDbContext : DbContext
{
    protected readonly IConfiguration _configuration;
    public PostsDbContext(IConfiguration configuration, DbContextOptions<PostsDbContext> options) : base(options) 
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostsConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>()
            .Property(e => e.LikesID)
            .HasColumnType("bigint[]"); // Specify the PostgreSQL array type

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Post> ? Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
}
