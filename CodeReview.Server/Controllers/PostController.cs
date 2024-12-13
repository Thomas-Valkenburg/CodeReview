using CodeReview.Core.Handlers;
using CodeReview.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CodeReview.Server.Models;

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
    public Task<ActionResult<PostView>> GetPost(int id)
    {
        var result = postHandler.GetPost(id);

        if (!result.Success)
        {
            return Task.FromResult<ActionResult<PostView>>(NotFound());
        }

        return Task.FromResult<ActionResult<PostView>>(Ok(result.Value?.CreatePostView()));
    }
}