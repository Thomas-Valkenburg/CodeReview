using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodeReview.Server.Controllers
{
	[Route("[action]")]
	[ApiController]
	public class AccountController(SignInManager<IdentityUser> signInManager) : ControllerBase
	{
		[AllowAnonymous]
		[HttpPost]
		[ActionName("Logout")]
		public async Task<ActionResult> LogoutPost(string? returnUrl = null)
		{
			await signInManager.SignOutAsync();

			if (returnUrl is not null)
			{
				return Redirect(returnUrl);
			}

			return Redirect(Request.GetEncodedUrl());
		}
	}
}
