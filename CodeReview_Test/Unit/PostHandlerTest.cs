using CodeReview.Core;
using CodeReview.Core.Handlers;
using CodeReview.Core.Interfaces;
using CodeReview.Core.Models;
using Moq;

namespace CodeReview.Test.Unit;

internal class PostHandlerTest
{
	[SetUp]
	public void Setup()
	{

	}

	[Test]
	public void GetPostList_ShouldThrow_NoException()
	{
		var mock = new Mock<IPostService>();
		mock.Setup(x => x.Take(It.IsAny<int>(), It.IsAny<SortOrder>(), It.IsAny<List<string>>())).Returns(new List<Post>());

		var userHandler = new PostHandler(mock.Object);

		Assert.DoesNotThrow(() =>
		{
			var result = userHandler.GetPostList(1);
			Assert.That(result is { Success: true, Value: not null });
		});
	}

	[Test]
	public void GetPost_ShouldThrow_NoException()
	{
		var mock = new Mock<IPostService>();
		mock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Post());

		var userHandler = new PostHandler(mock.Object);

		Assert.DoesNotThrow(() =>
		{
			var result = userHandler.GetPost(1);

			Assert.That(result is { Success: true, Value: not null });
		});
	}

	[Test]
	public void GetPost_ShouldThrow_ResultException()
	{
		var mock = new Mock<IPostService>();
		mock.Setup(x => x.Get(It.IsAny<int>())).Returns(() => null).Verifiable();

		var userHandler = new PostHandler(mock.Object);

		Assert.DoesNotThrow(() =>
		{
			var result = userHandler.GetPost(1);
			Assert.That(result is { Success: false, Value: null });
		});
	}

	[Test]
	public void CreatePost_ShouldThrow_NoException()
	{
		var mock = new Mock<IPostService>();
		mock.Setup(x => x.Create(It.IsAny<Post>())).Verifiable();
		mock.Setup(x => x.SaveChanges()).Verifiable();
		mock.Setup(x => x.GetAllFromUser(It.IsAny<int>())).Returns(new List<Post> { new Post() });

		var userHandler = new PostHandler(mock.Object);

		Assert.DoesNotThrow(() =>
		{
			var result = userHandler.CreatePost(new User(), "title", "content");
			Assert.That(result is { Success: true, Value: not null });
		});
	}

	[Test]
	public void CreatePost_ShouldThrow_ResultException_UserNotFound()
	{
		var mock = new Mock<IPostService>();
		mock.Setup(x => x.Create(It.IsAny<Post>())).Verifiable();
		mock.Setup(x => x.SaveChanges()).Verifiable();
		mock.Setup(x => x.GetAllFromUser(It.IsAny<int>())).Returns(() => null).Verifiable();

		var userHandler = new PostHandler(mock.Object);

		Assert.DoesNotThrow(() =>
		{
			var result = userHandler.CreatePost(null!, "title", "content");
			Assert.That(result is { Success: false, Value: null, Message: not null });
		});
	}

	[Test]
	public void CreatePost_ShouldThrow_ResultException()
	{
		var mock = new Mock<IPostService>();
		mock.Setup(x => x.Create(It.IsAny<Post>())).Verifiable();
		mock.Setup(x => x.SaveChanges()).Verifiable();
		mock.Setup(x => x.GetAllFromUser(It.IsAny<int>())).Returns(() => null).Verifiable();

		var userHandler = new PostHandler(mock.Object);

		Assert.DoesNotThrow(() =>
		{
			var result = userHandler.CreatePost(new User(), "title", "content");
			Assert.That(result is { Success: false, Value: null });
		});
	}
}