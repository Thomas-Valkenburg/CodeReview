using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodeReview.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController(IPostService postService) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public Task<ActionResult<IEnumerable<Post>>> GetPostList()
    {
        return Task.FromResult<ActionResult<IEnumerable<Post>>>(Ok(postService.Take(10)));
    }

    [HttpGet]
    public Task<ActionResult<Post>> GetPost(int id)
    {
        var post = postService.GetById(id);

        if (post is null)
        {
            return Task.FromResult<ActionResult<Post>>(NotFound());
        }

        return Task.FromResult<ActionResult<Post>>(Ok(post));
    }
}