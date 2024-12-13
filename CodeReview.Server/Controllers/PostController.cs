using CodeReview.Core.Handlers;
using CodeReview.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeReview.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController(PostHandler postHandler) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public Task<ActionResult<IEnumerable<Post>>> GetPostList()
    {
        return Task.FromResult<ActionResult<IEnumerable<Post>>>(Ok(postHandler.GetPostList(25)));
    }

    [HttpGet]
    public Task<ActionResult<Post>> GetPost(int id)
    {
        var result = postHandler.GetPost(id);

        if (!result.Success)
        {
            return Task.FromResult<ActionResult<Post>>(NotFound());
        }

        return Task.FromResult<ActionResult<Post>>(Ok(result.Value));
    }
}