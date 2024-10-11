using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeReview.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController(IDbContext context) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<ActionResult<IEnumerable<Post>>> GetPostList()
    {
        return await context.Set<Post>().Take(10).ToListAsync();
    }

    [HttpGet]
    public async Task<ActionResult<Post>> GetPost(int id)
    {
        var post = await context.Set<Post>().FindAsync(id);

        if (post == null)
        {
            return NotFound();
        }

        return post;
    }
}