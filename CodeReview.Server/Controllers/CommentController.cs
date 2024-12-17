using CodeReview.Core.Handlers;
using CodeReview.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Security.Claims;

namespace CodeReview.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentController(CommentHandler commentHandler) : ControllerBase
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<ActionResult<CommentView>> GetComment(int id)
    {
        var result = commentHandler.Get(id);

		if (!result.Success || result.Value is null) return Task.FromResult<ActionResult<CommentView>>(NotFound());

		return Task.FromResult<ActionResult<CommentView>>(Ok(result.Value.CreateCommentView()));
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