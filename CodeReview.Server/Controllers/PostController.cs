using Core.Handlers;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeReview.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController(PostHandler postService) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<ActionResult<IEnumerable<Post>>> GetPostList()
    {
        return Task.FromResult<ActionResult<IEnumerable<Post>>>(Ok(postService.GetPostList(25)));
    }

    [HttpGet]
    public async Task<ActionResult<Post>> GetPost(int id)
    {
        var result = postService.GetPost(id);

        if (!result.Success)
        {
            return NotFound();
        }

        return Task.FromResult<ActionResult<Post>>(Ok(result.Value));
    }
}