using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodeReview.Server.Controllers;

[Route("[action]")]
[ApiController]
public class AccountController(SignInManager<IdentityUser> signInManager) : ControllerBase
{
	[Authorize]
	[HttpPost]
	[ActionName("Logout")]
	public async Task<ActionResult> LogoutPostAsync(string? returnUrl = null)
	{
		await signInManager.SignOutAsync();

		if (returnUrl is not null)
		{
			return Redirect(returnUrl);
		}

		return Ok();
	}
}