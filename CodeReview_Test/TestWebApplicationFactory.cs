using CodeReview.DAL;
using CodeReview.DAL.Account;
using CodeReview.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CodeReview.Test;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseTestServer();

        builder.ConfigureServices(services =>
        {
            var contextDbDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDbContextOptionsConfiguration<Context>));
            var accountContextDbDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDbContextOptionsConfiguration<AccountContext>));

            var contextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(Context));

            services.Remove(contextDescriptor);
            services.Remove(contextDbDescriptor);
                
            var accountContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(AccountContext));

            services.Remove(accountContextDescriptor);
            services.Remove(accountContextDbDescriptor);

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