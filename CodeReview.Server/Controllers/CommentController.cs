using CodeReview.Core.Interfaces;
using CodeReview.Core.Models;
using CodeReview.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodeReview.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentController(ICommentService commentService) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentView>> GetComment(int id)
    {
        return Ok(commentService.GetById(id)?.CreateCommentView());
    }

    [HttpPost]
    public async Task<ActionResult<CommentView>> CreateComment([FromBody] Comment comment)
    {
        commentService.Create(comment);
        commentService.SaveChanges();
        return Ok(comment);
    }
}