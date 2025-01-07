using CodeReview.Core;
using CodeReview.Core.Interfaces;
using CodeReview.Core.Models;
using CodeReview.DAL;
using Microsoft.Extensions.DependencyInjection;

namespace CodeReview.Test.Integration;

public class PostServiceTest
{
	private readonly IUserService _userService =
		ServiceProviderHandler.GetScope().ServiceProvider.GetRequiredService<IUserService>();

	private readonly IPostService _postService =
		ServiceProviderHandler.GetScope().ServiceProvider.GetRequiredService<IPostService>();

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
	public void Get_ShouldThrow_NoException()
	{
		var context = ServiceProviderHandler.GetScope().ServiceProvider.GetRequiredService<Context>();

		var user = new User("1");

		var entity = context.Posts.Add(new Post(user, "1", "1"));

		context.SaveChanges();

		Assert.DoesNotThrow(() =>
		{
			var post = _postService.Get(entity.Entity.Id);
			Assert.That(post is { Title: "1", Content: "1" });
		});
	}

	[Test]
	public void Get_ShouldThrow_NoException_ObjectNull()
	{
		var post = _postService.Get(1);

		Assert.That(post, Is.Null);
	}

	[Test]
	public void GetAllFromUser_ShouldThrow_NoException()
	{
		var user = new User("1");

		_userService.Create(user);

		for (var i = 0; i < 10; i++)
		{
			_postService.Create(new Post(user, i.ToString(), i.ToString()));
		}

		_postService.SaveChanges();

		var posts = _postService.GetAllFromUser(user.Id);

		Assert.That(posts?.Count, Is.EqualTo(10));
	}

	[Test]
	public void GetAllFromUser_ShouldThrow_NoException_ObjectNull()
	{
		var posts = _postService.GetAllFromUser(1);
		Assert.That(posts, Is.Null);
	}

	[Test]
	public void Take_ShouldThrow_NoException()
	{
		for (var i = 0; i < 10; i++)
		{
			var user = new User(i.ToString());
			_userService.Create(user);

			var post = new Post(user, ((char)i).ToString(), i.ToString());
			_postService.Create(post);
		}

		_postService.SaveChanges();

		var posts = _postService.Take(10, SortOrder.Alphabetical);

		Assert.That(posts.Count, Is.EqualTo(10));
	}

	[Test]
	public void Take_WithFilter_ShouldThrow_NoException()
	{
		for (var i = 0; i < 10; i++)
		{
			var user = new User(i.ToString());
			_userService.Create(user);

			var post = new Post(user, ((char)i).ToString(), i.ToString());
			_postService.Create(post);
		}

		_postService.SaveChanges();

		var posts = _postService.Take(10, SortOrder.Alphabetical, ((char)0).ToString());

		Assert.That(posts.Count, Is.EqualTo(1));
	}

	[Test]
	public void Take_CycleSortOrders_ShouldThrow_NoException()
	{
		for (var i = 0; i < 10; i++)
		{
			var user = new User(i.ToString());
			_userService.Create(user);

			var post = new Post(user, ((char)i).ToString(), i.ToString());
			_postService.Create(post);
		}

		_postService.SaveChanges();

		var sortOrderValues = Enum.GetValues<SortOrder>();

		foreach (var sortOrder in sortOrderValues)
		{
			var posts = _postService.Take(10, sortOrder);
			Assert.That(posts.Count, Is.EqualTo(10));
		}
	}

	[Test]
	public void Take_ShouldThrow_ArgumentOutOfRangeException()
	{
		for (var i = 0; i < 10; i++)
		{
			var user = new User(i.ToString());
			_userService.Create(user);

			var post = new Post(user, ((char)i).ToString(), i.ToString());
			_postService.Create(post);
		}

		_postService.SaveChanges();

		Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			_postService.Take(5, (SortOrder) int.MinValue);
		});
	}

	[Test]
	public void CreatePost_ShouldThrow_NoException()
	{
		_userService.Create(new User());

		var user = _userService.GetById(1);

		if (user is null) Assert.Inconclusive("User not found");

		Assert.DoesNotThrow(() =>
		{
			var post = new Post(user, "Test Title", "Test Content");
			_postService.Create(post);
		});
	}

	[Test]
	public void CreatePost_ShouldThrow_NullReferenceException()
	{
		User? user = null;

		Assert.Throws<NullReferenceException>(() =>
		{
			var post = new Post(user!, "Test Title", "Test Content");
			_postService.Create(post);
		});
	}

	[Test]
	public void Update_ShouldThrow_NoException()
	{
		_userService.Create(new User());
		var user = _userService.GetById(1);

		if (user is null) Assert.Inconclusive("User not found");

		var post = new Post(user, "Test Title", "Test Content");
		_postService.Create(post);

		post.Title = "Updated Title";
		post.Content = "Updated Content";

		Assert.DoesNotThrow(() =>
		{
			_postService.Update(post);
		});
	}

	[Test]
	public void Delete_Id_ShouldThrow_NoException()
	{
		_userService.Create(new User("1"));
		var user = _userService.GetById(1);

		if (user is null) Assert.Inconclusive("User not found");

		var post = new Post(user, "Test Title", "Test Content");
		_postService.Create(post);

		_postService.SaveChanges();

		Assert.DoesNotThrow(() =>
		{
			_postService.Delete(post.Id);
		});
	}

	[Test]
	public void Delete_Post_ShouldThrow_NoException()
	{
		_userService.Create(new User("1"));
		var user = _userService.GetById(1);

		if (user is null) Assert.Inconclusive("User not found");

		var post = new Post(user, "Test Title", "Test Content");

		_postService.Create(post);

		_postService.SaveChanges();

		Assert.DoesNotThrow(() =>
		{
			_postService.Delete(post);
		});
	}
}