using CodeReview.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodeReview.DAL.Account;

public class AccountContext(DbContextOptions<AccountContext> options) : IdentityDbContext<AccountUser>(options)
{
    public DbSet<AccountUser> AccountUsers { get; set; }
}