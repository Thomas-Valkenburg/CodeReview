using CodeReview.Core.Handlers;
using CodeReview.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeReview.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController(UserHandler userHandler, PostHandler postHandler) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public Task<ActionResult<IEnumerable<PostView>>> GetPostList()
    {
	    var result = postHandler.GetPostList(25);

	    if (!result.Success || result.Value is null) return Task.FromResult<ActionResult<IEnumerable<PostView>>>(NotFound());

		return Task.FromResult<ActionResult<IEnumerable<PostView>>>(Ok(result.Value.Select(post => post.CreatePostView())));
    }

    [HttpGet("{id:int}")]
    public Task<ActionResult<PostView>> GetPost(int id)
    {
        var result = postHandler.GetPost(id);

        if (!result.Success)
        {
            return Task.FromResult<ActionResult<PostView>>(NotFound());
        }

        return Task.FromResult<ActionResult<PostView>>(Ok(result.Value?.CreatePostView()));
    }

    [HttpPost]
    [Authorize]
    public Task<ActionResult<PostView>> CreatePost(string title, string content)
    {
	    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
	    var result = userHandler.GetOrCreateUser(userId ?? string.Empty);

		if (userId is null || !result.Success || result.Value is null) return Task.FromResult<ActionResult<PostView>>(NotFound());

		var user = result.Value;

		var post = postHandler.CreatePost(user, title, content);
		
        if (!post.Success) return Task.FromResult<ActionResult<PostView>>(BadRequest());

        var postView = post.Value?.CreatePostView();

		return Task.FromResult<ActionResult<PostView>>(Ok(postView));
    }
}