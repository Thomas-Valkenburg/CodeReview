using CodeReview.Core.Models;
using CodeReview.DAL;
using CodeReview.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace CodeReview.Test.End2End;

public class UserControllerTest
{
	[SetUp]
	public void Setup()
	{
		var context = ServiceProviderHandler.GetScope().ServiceProvider.GetRequiredService<Context>();

		context.Database.EnsureCreated();
	}

	[TearDown]
	public void TearDown()
	{
		var context = ServiceProviderHandler.GetScope().ServiceProvider.GetRequiredService<Context>();

		context.ChangeTracker.Clear();

		context.Database.EnsureDeleted();
	}

	/*[Test]
	public async Task GetUser_Found_ShouldThrow_NoException()
	{
		//var context = ServiceProviderHandler.GetScope().ServiceProvider.GetRequiredService<Context>();

		await using var application = new TestWebApplicationFactory();
		using var       client      = application.CreateClient();

		var context = application.Services.GetRequiredService<Context>();

		var entityEntry = context.Users.Add(new User("1"));

		Assert.DoesNotThrowAsync(async () =>
		{
			var response = await client.GetAsync($"/api/User/{entityEntry.Entity.Id}");

			Console.WriteLine(response.StatusCode);

			Assert.That(response.StatusCode == HttpStatusCode.OK);
		});
	}*/

	/*[Test]
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
	}*/
}