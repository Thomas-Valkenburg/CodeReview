using CodeReview.DAL;
using CodeReview.DAL.Account;
using CodeReview.Server;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace CodeReview.Test;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseTestServer();

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IDbContextOptionsConfiguration<Context>>();
            services.RemoveAll<IDbContextOptionsConfiguration<AccountContext>>();
            services.RemoveAll<IConfigureOptions<AuthenticationOptions>>();

            services.AddDbContext<Context>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            services.AddDbContext<AccountContext>(options =>
            {
                options.UseInMemoryDatabase("AccountTestDb");
            });

			services.AddIdentityApiEndpoints<IdentityUser>(options =>
			{
				options.SignIn.RequireConfirmedEmail    = false;
				options.Password.RequireNonAlphanumeric = false;
				options.User.RequireUniqueEmail         = true;
				options.Password.RequiredLength         = 8;
				options.Password.RequireUppercase       = true;
			}).AddEntityFrameworkStores<AccountContext>()
			.AddSignInManager();

			services.AddAuthorization();
        });
    }
}