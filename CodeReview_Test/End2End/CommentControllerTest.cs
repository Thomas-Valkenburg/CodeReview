using CodeReview.Core.Models;
using CodeReview.DAL;
using CodeReview.DAL.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace CodeReview.Test.End2End;

public class CommentControllerTest
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
    public async Task GetComment_Found_ShouldThrow_NoException()
    {
        await using var application = new TestWebApplicationFactory();
        using var client = application.CreateClient();

        await using var scope = application.Services.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<Context>();
        var accountContext = scope.ServiceProvider.GetRequiredService<AccountContext>();

        var identityEntry = accountContext.Users.Add(new IdentityUser("1"));

        await accountContext.SaveChangesAsync();

        var user = new User(identityEntry.Entity.Id);

        var entity = context.Comments.Add(new Comment(user, new Post(user, "Title", "Content"), "Content"));

        await context.SaveChangesAsync();

        Assert.DoesNotThrowAsync(async () =>
        {
            var response = await client.GetAsync($"/api/comment/{entity.Entity.Id}");

            response.EnsureSuccessStatusCode();
        });
    }

    [Test]
    public async Task GetComment_NotFound_ShouldThrow_NotFound()
    {
        await using var application = new TestWebApplicationFactory();
        using var client = application.CreateClient();

        Assert.DoesNotThrowAsync(async () =>
        {
            var response = await client.GetAsync("/api/comment/1");

            Assert.That(response.StatusCode is HttpStatusCode.NotFound);
        });
    }

    [Test]
    public async Task GetComment_UserNotFound_ShouldThrow_ServerError()
    {
        await using var application = new TestWebApplicationFactory();
        using var client = application.CreateClient();

        await using var scope = application.Services.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<Context>();

        var entity =
            context.Comments.Add(new Comment(new User("1"), new Post(new User("1"), "Title", "Content"), "Content"));

        await context.SaveChangesAsync();

        Assert.DoesNotThrowAsync(async () =>
        {
            var response = await client.GetAsync($"/api/comment/{entity.Entity.Id}");

            Assert.That(response.StatusCode is HttpStatusCode.InternalServerError);
        });
    }

    [Test]
    public async Task CreateComment_ShouldThrow_UnAuthorized()
    {
        await using var application = new TestWebApplicationFactory();
        using var client = application.CreateClient();

        await using var scope = application.Services.CreateAsyncScope();

        var accountContext = scope.ServiceProvider.GetRequiredService<AccountContext>();

        var identityEntry = accountContext.Users.Add(new IdentityUser("1"));

        await accountContext.SaveChangesAsync();

        var context = scope.ServiceProvider.GetRequiredService<Context>();

        var user = new User(identityEntry.Entity.Id);

        var comment = new Comment(user, new Post(user, "Title", "Content"), "Content");

        await context.SaveChangesAsync();

        Assert.DoesNotThrowAsync(async () =>
        {
            var response = await client.PostAsJsonAsync("/api/comment", new { postId = comment.Post.Id, content = comment.Content });

            Assert.That(response.StatusCode is HttpStatusCode.Unauthorized);
        });
    }
}