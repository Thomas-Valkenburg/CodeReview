using CodeReview.Core.Handlers;
using CodeReview.Core.Interfaces;
using CodeReview.Core.Models;
using Moq;

namespace CodeReview.Test.Unit;

internal class UserHandlerTest
{
	[SetUp]
	public void Setup()
	{

	}

	[Test]
	public void CreateUser_ShouldThrow_NoException()
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

	[Test]
	public void CreateUser_ShouldThrow_ResultException()
	{
		var moch = new Mock<IUserService>();
		moch.Setup(x => x.Create(It.IsAny<User>())).Verifiable();

		var userHandler = new UserHandler(moch.Object);

		Assert.DoesNotThrow(() =>
		{
			User? user = null;
			var result = userHandler.CreateUser(user!);

			Assert.That(!result.Success);
		});
	}

	[Test]
	public void GetUser_ID_ShouldThrow_NoException()
	{
		var mock = new Mock<IUserService>();
		mock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User()).Verifiable();

		var userHandler = new UserHandler(mock.Object);

		Assert.DoesNotThrow(() =>
		{
			var result = userHandler.GetUser(1);

			Assert.That(result is { Success: true, Value: not null });
		});
	}

	[Test]
	public void GetUser_ID_ShouldThrow_ResultException()
	{
		var mock = new Mock<IUserService>();
		mock.Setup(x => x.GetById(It.IsAny<int>())).Returns(() => null).Verifiable();

		var userHandler = new UserHandler(mock.Object);

		Assert.DoesNotThrow(() =>
		{
			var result = userHandler.GetUser(1);
			Assert.That(result is { Success: false, Value: null });
		});
	}

	[Test]
	public void GetUser_AccountUserId_ShouldThrow_NoException()
	{
		var mock = new Mock<IUserService>();
		mock.Setup(x => x.GetByAccountUserId(It.IsAny<string>())).Returns(new User()).Verifiable();

		var userHandler = new UserHandler(mock.Object);

		Assert.DoesNotThrow(() =>
		{
			var result = userHandler.GetUser("1");
			Assert.That(result is { Success: true, Value: not null });
		});
	}

	[Test]
	public void GetUser_AccountUserId_ShouldThrow_ResultException()
	{
		var mock = new Mock<IUserService>();
		mock.Setup(x => x.GetByAccountUserId(It.IsAny<string>())).Returns(() => null).Verifiable();

		var userHandler = new UserHandler(mock.Object);

		Assert.DoesNotThrow(() =>
		{
			var result = userHandler.GetUser("1");
			Assert.That(result is { Success: false, Value: null });
		});
	}

	[Test]
	public void GetOrCreateUser_Create_ShouldThrow_NoException()
	{
		var mock = new Mock<IUserService>();
		mock.Setup(x => x.GetByAccountUserId(It.IsAny<string>())).Returns(() => null).Verifiable();
		mock.Setup(x => x.Create(It.IsAny<User>())).Verifiable();

		var userHandler = new UserHandler(mock.Object);

		Assert.DoesNotThrow(() =>
		{
			var result = userHandler.GetOrCreateUser("1");
			Assert.That(result is { Success: true, Value: not null });
		});
	}

	[Test]
	public void GetOrCreateUser_Get_ShouldThrow_NoException()
	{
		var mock = new Mock<IUserService>();
		mock.Setup(x => x.GetByAccountUserId(It.IsAny<string>())).Returns(() => new User()).Verifiable();

		var userHandler = new UserHandler(mock.Object);

		Assert.DoesNotThrow(() =>
		{
			var result = userHandler.GetOrCreateUser("1");
			Assert.That(result is { Success: true, Value: not null });
		});
	}
}