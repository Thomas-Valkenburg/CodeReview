using Microsoft.AspNetCore.Identity;

namespace Core.Models;

public class AccountUser : IdentityUser
{
    public User User { get; init; } = new();
}