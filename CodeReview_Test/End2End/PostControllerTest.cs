using CodeReview.Core.Models;
using CodeReview.DAL;
using CodeReview.DAL.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CodeReview.Test.End2End;

public class PostControllerTest
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
    public async Task GetPostList_ShouldThrow_NoException()
    {
        await using var application = new TestWebApplicationFactory();
        using var client = application.CreateClient();

        await using var scope = application.Services.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<Context>();

        context.Posts.Add(new Post(new User("1"), "Title", "Content"));

        await context.SaveChangesAsync();

        Assert.DoesNotThrowAsync(async () =>
        {
            var response = await client.GetAsync("/api/Post/list");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        });
    }

    [Test]
    public async Task GetPost_Found_ShouldThrow_NoException()
    {
        await using var application = new TestWebApplicationFactory();
        using var client = application.CreateClient();

        await using var scope = application.Services.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<Context>();
        var accountContext = scope.ServiceProvider.GetRequiredService<AccountContext>();

        var accountEntity = accountContext.Users.Add(new IdentityUser("1"));

        await accountContext.SaveChangesAsync();

        var entityEntry = context.Posts.Add(new Post(new User(accountEntity.Entity.Id), "Title", "Content"));

        await context.SaveChangesAsync();

        Assert.DoesNotThrowAsync(async () =>
        {
            var response = await client.GetAsync($"/api/Post/{entityEntry.Entity.Id}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        });
    }

    [Test]
    public async Task GetPost_NotFound_ShouldThrow_NotFound()
    {
        await using var application = new TestWebApplicationFactory();
        using var client = application.CreateClient();

        const string id = "1";

        Assert.DoesNotThrowAsync(async () =>
        {
            var response = await client.GetAsync($"/api/Post/{id}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        });
    }

    [Test]
    public async Task CreatePost_ShouldThrow_NotAuthorized()
    {
        await using var application = new TestWebApplicationFactory();
        using var client = application.CreateClient();

        await using var scope = application.Services.CreateAsyncScope();

        Assert.DoesNotThrowAsync(async () =>
        {
            var response = await client.PostAsync("/api/Post", new StringContent(JsonSerializer.Serialize(new { title = "Title", editorContent = "Content" }), Encoding.UTF8, "application/json"));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        });
    }
}