using CodeReview.Core.Interfaces;
using CodeReview.Core.Models;

namespace CodeReview.Core.Handlers;

public class UserHandler(IUserService userService)
{
	public Result CreateUser(User user)
	{
		if (user is null) return Result.FromException();

		userService.Create(user);

		userService.SaveChanges();

		return Result.FromSuccess();
	}

	public Result<User> GetUser(int id)
	{
		var user = userService.GetById(id);

		return user is not null ? Result.FromSuccess(user) : Result.FromException<User>("User not found");
	}

	public Result<User> GetUser(string id)
	{
		var user = userService.GetByAccountUserId(id);

		return user is not null ? Result.FromSuccess(user) : Result.FromException<User>("User not found");
	}

	/// <summary>
	/// Only to be used when it is certain a user has an identity account
	/// </summary>
	/// <param name="id">The id of the identity account</param>
	/// <returns>A new user object</returns>
	public virtual Result<User> GetOrCreateUser(string id)
	{
		var result = GetUser(id);

		if (result is {Success: true, Value: not null})
		{
			return result;
		}

		var newUser = new User(id);

		CreateUser(newUser);

		return Result.FromSuccess(newUser);
	}
}