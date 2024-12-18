﻿using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace DAL_Account;

public class AccountUser : IdentityUser
{
    public User User { get; init; } = new();
}