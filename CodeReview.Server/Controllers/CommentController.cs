using CodeReview.Core.Handlers;
using CodeReview.DAL.Account;
using CodeReview.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeReview.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentController(CommentHandler commentHandler, AccountContext accountContext) : ControllerBase
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<ActionResult<CommentView>> GetComment(int id)
    {
        var result = commentHandler.Get(id);

		if (!result.Success || result.Value is not { } comment) return Task.FromResult<ActionResult<CommentView>>(NotFound());

		var identityUser = accountContext.Users.Find(comment.Author.ApplicationUserId);

		if (identityUser == null) return Task.FromResult<ActionResult<CommentView>>(StatusCode(500));

		return Task.FromResult<ActionResult<CommentView>>(Ok(comment.CreateCommentView(identityUser)));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Authorize]
    public Task<ActionResult> CreateComment(int postId, string content)
    {
	    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null) return Task.FromResult<ActionResult>(Unauthorized());

		var result = commentHandler.Create(userId, postId, content);

		if (!result.Success) return Task.FromResult<ActionResult>(BadRequest("Unsuccessful"));

		return Task.FromResult<ActionResult>(Created());
    }
}