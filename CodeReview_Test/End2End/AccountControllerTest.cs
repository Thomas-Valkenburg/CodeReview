using System.Net;
using CodeReview.DAL;
using CodeReview.DAL.Account;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace CodeReview.Test.End2End;

public class AccountControllerTest
{
	[SetUp]
	public async Task Setup()
	{
		await using var application = new TestWebApplicationFactory();
		await using var scope       = application.Services.CreateAsyncScope();

		var context = scope.ServiceProvider.GetRequiredService<Context>();

		await context.Database.EnsureCreatedAsync();

		var accountContext = scope.ServiceProvider.GetRequiredService<AccountContext>();

		await accountContext.Database.EnsureCreatedAsync();
	}

	[TearDown]
	public async Task TearDown()
	{
		await using var application = new TestWebApplicationFactory();
		await using var scope       = application.Services.CreateAsyncScope();

		var context = scope.ServiceProvider.GetRequiredService<Context>();

		context.ChangeTracker.Clear();

		await context.Database.EnsureDeletedAsync();

		var accountContext = scope.ServiceProvider.GetRequiredService<AccountContext>();

		accountContext.ChangeTracker.Clear();

		await accountContext.Database.EnsureDeletedAsync();
	}

	[Test]
	public async Task LogoutPost_ReturnUrlNull_ShouldReturn_200OK()
	{
		await using var application = new TestWebApplicationFactory().WithWebHostBuilder(builder =>
		{
			builder.ConfigureServices(services =>
			{
				services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
			});
		});

		using var client = application.CreateClient();

		Assert.DoesNotThrowAsync(async () =>
		{
			var response = await client.PostAsync("/Logout", null);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		});
	}

	[Test]
	public async Task LogoutPost_ReturnUrlNull_ShouldReturn_401Unauthorized()
	{
		await using var application = new TestWebApplicationFactory();

		using var client = application.CreateClient();

		Assert.DoesNotThrowAsync(async () =>
		{
			var response = await client.PostAsync("/Logout", null);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
		});
	}
}