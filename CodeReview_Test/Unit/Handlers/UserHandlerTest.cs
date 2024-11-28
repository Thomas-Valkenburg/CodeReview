using Core.Handlers;
using Core.Interfaces;
using Core.Models;
using Moq;

namespace CodeReview_Test.Unit.Handlers;

internal class UserHandlerTest
{
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void UserHandler_CreateUser_NoError()
    {
        var mock = new Mock<IUserService>();
        mock.Setup(x => x.Create(It.IsAny<User>())).Verifiable();

        var userHandler = new UserHandler(mock.Object);

        Assert.DoesNotThrow(() =>
        {
            var user = new User();
            userHandler.CreateUser(user);
        });
    }

    /*[Test]
    public void UserHandler_GetUser_NoError()
    {
        var mock = new Mock<IUserService>();
        mock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User()).Verifiable();

        var userHandler = new UserHandler(mock.Object);

        Assert.DoesNotThrow(() =>
        {
            userHandler.GetUser(1);
        });
    }*/
}