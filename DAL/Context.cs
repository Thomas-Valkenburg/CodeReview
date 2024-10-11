using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class Context(DbContextOptions options) : DbContext(options), IDbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DbSet<Comment> Comments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}