using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodeReview.DAL.Account;

public class AccountContext(DbContextOptions<AccountContext> options) : IdentityDbContext(options)
{
}