using CodeReview.DAL;
using CodeReview.DAL.Account;
using CodeReview.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CodeReview.Test
{
	public class TestWebApplicationFactory : WebApplicationFactory<Program>
	{
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.UseTestServer();

			builder.ConfigureTestServices(services =>
			{
				services.RemoveAll<Context>();
				services.RemoveAll<AccountContext>();

				services.AddDbContext<Context>(options =>
				{
					options.UseInMemoryDatabase("TestDb");
				});

				services.AddDbContext<AccountContext>(options =>
				{
					options.UseInMemoryDatabase("AccountTestDb");
				});
			});
		}
	}
}
