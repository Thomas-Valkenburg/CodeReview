using Core.Interfaces;
using Core.Models;

namespace Core.Handlers;

public class UserHandler(IUserService userService)
{
    public Result CreateUser(User user)
    {
        if (user is null) return Result.FromException();

        userService.Create(user);

        return Result.FromSuccess();
    }

    public Result<User> GetUser(int id)
    {
        var user = userService.GetById(id);

        if (user is not null) return Result.FromSuccess(user);

        return Result.FromException<User>("User not found");
    }

    public Result<User> GetUser(string id)
    {
        var user = userService.GetByAccountUserId(id);

        if (user is not null) return Result.FromSuccess(user);

        var newUser = new User(id);

        CreateUser(newUser);

        return Result.FromSuccess(newUser);
    }
}