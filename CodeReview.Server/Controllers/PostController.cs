using CodeReview.Core;
using CodeReview.Core.Handlers;
using CodeReview.DAL.Account;
using CodeReview.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeReview.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController(UserHandler userHandler, PostHandler postHandler, AccountContext accountContext) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public Task<ActionResult<IEnumerable<PostView>>> GetPostList(string? search = null)
    {
	    var result = postHandler.GetPostList(25, search is null ? SortOrder.Newest : SortOrder.Alphabetical, search?.Split(" ").ToList());

	    if (!result.Success || result.Value is null) return Task.FromResult<ActionResult<IEnumerable<PostView>>>(NotFound("No posts found"));

	    var posts = result.Value.Select(post =>
	    {
            var user = accountContext.Users.Find(post.Author.ApplicationUserId);

			if (user is null) return post.CreatePostView("Unknown");

			return post.CreatePostView(user);
	    });
		
		return Task.FromResult<ActionResult<IEnumerable<PostView>>>(Ok(posts));
    }

    [HttpGet("{id:int}")]
    public Task<ActionResult<PostView>> GetPost(int id)
    {
        var result = postHandler.GetPost(id);

        if (!result.Success || result.Value is not { } post)
        {
            return Task.FromResult<ActionResult<PostView>>(NotFound("Post not found"));
        }

        var user = accountContext.Users.Find(post.Author.ApplicationUserId);

        if (user is null) return Task.FromResult<ActionResult<PostView>>(NotFound("Author not found"));

		return Task.FromResult<ActionResult<PostView>>(Ok(post.CreatePostView(user)));
    }

    [HttpPost]
    [Authorize]
    public Task<ActionResult> CreatePost(string title, string editorContent)
    {
	    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
	    var result = userHandler.GetOrCreateUser(userId ?? string.Empty);

		if (userId is null || !result.Success || result.Value is null) return Task.FromResult<ActionResult>(NotFound());

		var user = result.Value;

		var post = postHandler.CreatePost(user, title, editorContent);
		
        if (!post.Success) return Task.FromResult<ActionResult>(BadRequest());

		return Task.FromResult<ActionResult>(Ok());
    }
}