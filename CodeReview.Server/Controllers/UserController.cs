using CodeReview.Core.Handlers;
using CodeReview.DAL.Account;
using CodeReview.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodeReview.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(UserHandler userHandler, AccountContext accountContext) : ControllerBase
{
	[HttpGet("{id:int}")]
	public Task<ActionResult<UserView>> GetUser(int id)
	{
		var result = userHandler.GetUser(id);

		if (!result.Success || result.Value is null) return Task.FromResult<ActionResult<UserView>>(NotFound());

		return Task.FromResult<ActionResult<UserView>>(Ok(result.Value.CreateUserView(accountContext)));
	}
}