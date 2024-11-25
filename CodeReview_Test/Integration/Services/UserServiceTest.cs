using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CodeReview_Test.Integration.Services;

public class UserServiceTest
{
    private readonly IUserService _userService = ServiceProviderHandler.GetScope().ServiceProvider.GetRequiredService<IUserService>();

    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void UserService_CreateUser_NoError()
    {
        Assert.DoesNotThrow(() =>
        {
            var user = new User();

            _userService.Create(user);
        });
    }
}