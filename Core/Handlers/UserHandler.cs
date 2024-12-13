using Core.Interfaces;
using Core.Models;

namespace Core.Handlers;

public class UserHandler(IUserService userService)
{
    private Result<User> CreateUser(string applicationUserId)
    {
        var user = new User(applicationUserId);

		userService.Create(user);

        return Result.FromSuccess(user);
    }

    public Result<User> GetUser(string applicationUserId)
    {
        var user = userService.GetByApplicationUserId(applicationUserId);

		return user is null ? CreateUser(applicationUserId) : Result.FromSuccess(user);
    }
}