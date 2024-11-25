﻿using Core.Interfaces;
using Core.Models;

namespace Core.Handlers;

public class UserHandler(IUserService userService)
{
    public Result<User> CreateUser(User user)
    {
        if (user is null) return Result.FromException();

        userService.Create(user);
    }
}