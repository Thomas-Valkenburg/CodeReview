using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CodeReview_Test.Services.PostService;

public class PostServiceTest
{
    private readonly IUserService _userService =
        ServiceProviderHandler.GetScope().ServiceProvider.GetRequiredService<IUserService>();

    private readonly IPostService _postService =
        ServiceProviderHandler.GetScope().ServiceProvider.GetRequiredService<IPostService>();

    [SetUp]
    public void Setup()
    {
        _userService.Create(new User());
    }

    [Test]
    public void CreatePost_ValidPost_NoError()
    {
        var user = _userService.GetById(1);

        if (user is null) Assert.Inconclusive("User not found");

        Assert.DoesNotThrow(() =>
        {
            var post = new Post(user, "Test Title", "Test Content");
            _postService.Create(post);
        });
    }

    [Test]
    public void CreatePost_InvalidUser_ShouldThrow()
    {
        User? user = null;

        Assert.Throws<Exception>(() =>
        {
            var post = new Post(user!, "Test Title", "Test Content");
            _postService.Create(post);
        });
    }

    [Test]
    public void CreatePost_InvalidTitle_ShouldThrow()
    {
        var user = _userService.GetById(1);
        if (user is null) Assert.Inconclusive("User not found");

        Assert.Throws<Exception>(() =>
        {
            var post = new Post(user, "", "Test Content");
            _postService.Create(post);
        });
    }

    [Test]
    public void CreatePost_InvalidContent_ShouldThrow()
    {
        var user = _userService.GetById(1);
        if (user is null) Assert.Inconclusive("User not found");

        Assert.Throws<Exception>(() =>
        {
            var post = new Post(user, "Test Title", "");
            _postService.Create(post);
        });
    }
}