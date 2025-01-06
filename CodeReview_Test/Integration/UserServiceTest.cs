using CodeReview.Core.Interfaces;
using CodeReview.Core.Models;
using CodeReview.DAL;
using Microsoft.Extensions.DependencyInjection;

namespace CodeReview.Test.Integration;

public class UserServiceTest
{
	private readonly IUserService _userService = ServiceProviderHandler.GetScope().ServiceProvider.GetRequiredService<IUserService>();

	[SetUp]
	public void Setup()
	{
		var context = ServiceProviderHandler.GetScope().ServiceProvider.GetRequiredService<Context>();

		context.Database.EnsureCreated();
	}

	[TearDown]
	public void TearDown()
	{
		var context = ServiceProviderHandler.GetScope().ServiceProvider.GetRequiredService<Context>();

		context.ChangeTracker.Clear();

		context.Database.EnsureDeleted();
	}

	[Test]
	public void GetById_ShouldThrow_NoException()
	{
		_userService.Create(new User("1"));
		_userService.SaveChanges();

		Assert.DoesNotThrow(() =>
		{
			var user = _userService.GetById(1);
			Assert.That(user is { Id: 1 });
		});
	}

	[Test]
	public void GetById_ShouldThrow_NoException_ObjectNull()
	{
		var user = _userService.GetById(1);
		Assert.That(user, Is.Null);
	}

	[Test]
	public void GetByAccountUserId_ShouldThrow_NoException()
	{
		_userService.Create(new User("1"));
		_userService.SaveChanges();

		Assert.DoesNotThrow(() =>
		{
			var user = _userService.GetByAccountUserId("1");
			Assert.That(user is { ApplicationUserId: "1" });
		});
	}

	[Test]
	public void GetByAccountUserId_ShouldThrow_NoException_ObjectNull()
	{
		var user = _userService.GetByAccountUserId("1");
		Assert.That(user, Is.Null);
	}

	[Test]
	public void CreateUser_ShouldThrow_NoException()
	{
		Assert.DoesNotThrow(() =>
		{
			var user = new User("1");

			_userService.Create(user);
			_userService.SaveChanges();
		});

		Assert.That(_userService.GetById(1) is { ApplicationUserId: "1" });
	}

	[Test]
	public void CreateUser_ShouldThrow_NullReferenceException()
	{
		Assert.Throws<NullReferenceException>(() =>
		{
			_userService.Create(null!);
		});
	}

	[Test]
	public void UpdateUser_ShouldThrow_NoException()
	{
		_userService.Create(new User("1"));
		_userService.SaveChanges();

		var user = _userService.GetById(1);

		if (user is null) Assert.Inconclusive("User not found");

		Assert.DoesNotThrow(() =>
		{
			user.ApplicationUserId = "2";

			_userService.Update(user);
			_userService.SaveChanges();
		});

		var updatedUser = _userService.GetById(1);
		Assert.That(updatedUser is { ApplicationUserId: "2" });
	}

	[Test]
	public void UpdateUser_ShouldThrow_NullReferenceException()
	{
		Assert.Throws<NullReferenceException>(() =>
		{
			_userService.Update(null!);
		});
	}

	[Test]
	public void DeleteUser_Id_ShouldThrow_NoException()
	{
		_userService.Create(new User("1"));

		_userService.SaveChanges();

		Assert.DoesNotThrow(() =>
		{
			_userService.Delete(1);
			_userService.SaveChanges();
		});
	}

	[Test]
	public void DeleteUser_Id_ShouldThrow_NullReferenceException()
	{
		Assert.Throws<NullReferenceException>(() =>
		{
			_userService.Delete(1);
		});
	}

	[Test]
	public void DeleteUser_User_ShouldThrow_NoException()
	{
		_userService.Create(new User("1"));

		_userService.SaveChanges();

		var user = _userService.GetById(1);

		if (user is null) Assert.Inconclusive("User not found");

		Assert.DoesNotThrow(() =>
		{
			_userService.Delete(user);
			_userService.SaveChanges();
		});
	}

	[Test]
	public void DeleteUser_User_ShouldThrow_ArgumentNullException()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			_userService.Delete(null!);
		});
	}
}