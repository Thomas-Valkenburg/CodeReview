using CodeReview.Core.Models;
using CodeReview.DAL;
using CodeReview.DAL.Account;
using CodeReview.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace CodeReview.Test.End2End;

public class UserControllerTest
{
    [SetUp]
    public async Task Setup()
    {
        await using var application = new TestWebApplicationFactory();
        await using var scope = application.Services.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<Context>();

        await context.Database.EnsureCreatedAsync();

        var accountContext = scope.ServiceProvider.GetRequiredService<AccountContext>();

        await accountContext.Database.EnsureCreatedAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await using var application = new TestWebApplicationFactory();
        await using var scope = application.Services.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<Context>();

        context.ChangeTracker.Clear();

        await context.Database.EnsureDeletedAsync();

        var accountContext = scope.ServiceProvider.GetRequiredService<AccountContext>();

        accountContext.ChangeTracker.Clear();

        await accountContext.Database.EnsureDeletedAsync();
    }

	[Test]
	public async Task GetUser_Found_ShouldThrow_NoException()
	{
		await using var application = new TestWebApplicationFactory();
		using var       client      = application.CreateClient();

        await using var scope = application.Services.CreateAsyncScope();

		var context = scope.ServiceProvider.GetRequiredService<Context>();
		var accountContext = scope.ServiceProvider.GetRequiredService<AccountContext>();

        var accountEntity = accountContext.Users.Add(new IdentityUser("1"));

        await accountContext.SaveChangesAsync();

        var entityEntry = context.Users.Add(new User(accountEntity.Entity.Id));

        await context.SaveChangesAsync();

        Assert.DoesNotThrowAsync(async () =>
		{
			var response = await client.GetAsync($"/api/User/{entityEntry.Entity.Id}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		});
	}

	[Test]
	public void GetUser_NotFound_ShouldThrow_NoException()
	{
		const string id = "1";

		Assert.DoesNotThrowAsync(async () =>
		{
			await using var application = new WebApplicationFactory<Program>();
			using var       client      = application.CreateClient();

			var response = await client.GetAsync($"/api/User/{id}");

			Assert.That(response.StatusCode is HttpStatusCode.NotFound);
		});
	}
}