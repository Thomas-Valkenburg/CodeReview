using CodeReview.Core.Interfaces;
using CodeReview.Core.Models;
using CodeReview.DAL;
using Microsoft.Extensions.DependencyInjection;

namespace CodeReview.Test.Integration;

public class CommentServiceTest
{
	private readonly ICommentService _commentService =
		ServiceProviderHandler.GetScope().ServiceProvider.GetRequiredService<ICommentService>();
	
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
	public void GetById_ShouldThrow_NoException()
	{
		var user = new User("1");
		var post = new Post(user, "title", "content");

		const string content = "comment content";

		_commentService.Create(new Comment(user, post, content));
		_commentService.SaveChanges();

		Assert.DoesNotThrow(() =>
		{
			var comment = _commentService.GetById(1);
			Assert.That(comment is { Content: content });
		});
	}

	[Test]
	public void GetById_ShouldThrow_NoException_ObjectNull()
	{
		var comment = _commentService.GetById(1);
		Assert.That(comment, Is.Null);
	}

	[Test]
	public void GetAll_ShouldThrow_NoException()
	{
		var user = new User("1");
		var post = new Post(user, "title", "content");

		const string content = "comment content";

		_commentService.Create(new Comment(user, post, content));
		_commentService.SaveChanges();

		Assert.DoesNotThrow(() =>
		{
			var comments = _commentService.GetAll(post.Id);
			Assert.That(comments is { Count: 1 });
		});
	}

	/// <summary>
	/// Post does not exist
	/// </summary>
	/// <returns>null</returns>
	[Test]
	public void GetAll_ShouldThrow_NoException_ObjectNull()
	{
		var comments = _commentService.GetAll(1);
		Assert.That(comments, Is.Null);
	}

	/// <summary>
	/// <see cref="Post"/> does not have any comments
	/// </summary>
	/// <returns><see cref="InvalidOperationException"/></returns>
	[Test]
	public void GetAll_EmptyList_ShouldThrow_InvalidOperationException()
	{
		var user = new User("1");
		var post = new Post(user, "title", "content");

		_postService.Create(post);

		_postService.SaveChanges();

		Assert.Throws<InvalidOperationException>(() =>
		{
			var comments = _commentService.GetAll(post.Id);
			Assert.That(comments?.First() is not null);
		});
	}

	[Test]
	public void Create_ShouldThrow_NoException()
	{
		var user = new User("1");
		var post = new Post(user, "title", "content");

		_postService.Create(post);
		_postService.SaveChanges();

		Assert.DoesNotThrow(() =>
		{
			var comment = new Comment(user, post, "content");
			_commentService.Create(comment);

			_commentService.SaveChanges();
		});
	}

	[Test]
	public void Create_ShouldThrow_NullReferenceException()
	{
		Assert.Throws<NullReferenceException>(() =>
		{
			_commentService.Create(null!);
		});
	}

	[Test]
	public void Update_ShouldThrow_NoException()
	{
		var user = new User("1");
		var post = new Post(user, "title", "content");

		_postService.Create(post);
		_postService.SaveChanges();

		var comment = new Comment(user, post, "content");

		_commentService.Create(comment);
		_commentService.SaveChanges();

		comment.Content = "updated content";

		Assert.DoesNotThrow(() =>
		{
			_commentService.Update(comment);
			_commentService.SaveChanges();
		});
	}

	[Test]
	public void Update_ShouldThrow_NullReferenceException()
	{
		Assert.Throws<NullReferenceException>(() =>
		{
			_commentService.Update(null!);
		});
	}

	[Test]
	public void Delete_Id_ShouldThrow_NoException()
	{
		var user = new User("1");
		var post = new Post(user, "title", "content");

		_postService.Create(post);
		_postService.SaveChanges();

		var comment = new Comment(user, post, "content");

		_commentService.Create(comment);
		_commentService.SaveChanges();

		Assert.DoesNotThrow(() =>
		{
			_commentService.Delete(comment.Id);
			_commentService.SaveChanges();
		});
	}

	[Test]
	public void Delete_Id_ShouldThrow_NullReferenceException()
	{
		Assert.Throws<NullReferenceException>(() =>
		{
			_commentService.Delete(1);
		});
	}

	[Test]
	public void Delete_Comment_ShouldThrow_NoException()
	{
		var user = new User("1");
		var post = new Post(user, "title", "content");

		_postService.Create(post);
		_postService.SaveChanges();

		var comment = new Comment(user, post, "content");

		_commentService.Create(comment);
		_commentService.SaveChanges();

		Assert.DoesNotThrow(() =>
		{
			_commentService.Delete(comment);
			_commentService.SaveChanges();
		});
	}

	[Test]
	public void Delete_Comment_ShouldThrow_ArgumentNullException()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			_commentService.Delete(null!);
		});
	}
}
