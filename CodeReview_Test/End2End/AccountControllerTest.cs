using CodeReview.Server.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace CodeReview.Test.End2End;

public class AccountControllerTest
{
	private readonly AccountController accountController = ServiceProviderHandler.GetScope().ServiceProvider.GetRequiredService<AccountController>();

	[SetUp]
	public void Setup()
	{
	}

	// Removed because doesn't work without a valid SignInManager
	/*[Test]
	public void LogoutPost_ReturnUrlNull_ShouldThrow_NoException()
	{
		Assert.DoesNotThrow(() =>
		{
			var result = accountController.LogoutPostAsync(null).GetAwaiter().GetResult();
		});
	}*/
}