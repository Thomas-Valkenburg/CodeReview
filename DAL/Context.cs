using CodeReview.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeReview.DAL;

public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DbSet<Comment> Comments { get; set; }
}