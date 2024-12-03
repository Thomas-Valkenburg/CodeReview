using Core.Handlers;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CodeReview.Server.Models;

namespace CodeReview.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController(IPostService postService, UserHandler userHandler) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public Task<ActionResult<IEnumerable<PostView>>> GetPostList()
    {
        return Task.FromResult<ActionResult<IEnumerable<PostView>>>(Ok(postService.Take(25).Select(post => post.CreatePostView())));
    }

    [HttpGet]
    public Task<ActionResult<PostView>> GetPost(int id)
    {
        var post = postService.GetById(id);

        if (post is null)
        {
            return Task.FromResult<ActionResult<PostView>>(NotFound());
        }

        var postView = post.CreatePostView();

        return Task.FromResult<ActionResult<PostView>>(Ok(postView));
    }

    [HttpPost]
    [Authorize]
    public Task<ActionResult<PostView>> CreatePost(string title, string content)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Task.FromResult<ActionResult<PostView>>(Unauthorized());
        }

        var userResult = userHandler.GetUser(userId);

        if (userResult.Value is null)
        {
            return Task.FromResult<ActionResult<PostView>>(Unauthorized());
        }

        var post = new Post(userResult.Value, title, content);

        postService.Create(post);

        var postView = post.CreatePostView();
        return Task.FromResult<ActionResult<PostView>>(Ok(postView));
    }
}