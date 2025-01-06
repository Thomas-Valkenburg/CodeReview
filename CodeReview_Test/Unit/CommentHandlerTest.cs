using CodeReview.Core;
using CodeReview.Core.Handlers;
using CodeReview.Core.Interfaces;
using CodeReview.Core.Models;
using Moq;

namespace CodeReview.Test.Unit;

internal class CommentHandlerTest
{
	[SetUp]
	public void Setup()
	{

	}

	[Test]
	public void GetComment_Id_ShouldThrow_NoException()
	{
		var mock = new Mock<ICommentService>();
		mock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Comment());

		var commentHandler = new CommentHandler(null!, null!, mock.Object);

		Assert.DoesNotThrow(() =>
		{
			var result = commentHandler.Get(1);
			Assert.That(result is { Success: true, Value: not null });
		});
	}

	[Test]
	public void GetComment_Id_ShouldThrow_ResultException()
	{
		var mock = new Mock<ICommentService>();
		mock.Setup(x => x.GetById(It.IsAny<int>())).Returns(() => null).Verifiable();

		var commentHandler = new CommentHandler(null!, null!, mock.Object);

		Assert.DoesNotThrow(() =>
		{
			var result = commentHandler.Get(1);
			Assert.That(result is { Success: false, Value: null });
		});
	}

	[Test]
	public void GetCommentList_UserId_ShouldThrow_NoException()
	{
		var mock = new Mock<ICommentService>();
		mock.Setup(x => x.GetAll(It.IsAny<int>())).Returns([]);

		var commentHandler = new CommentHandler(null!, null!, mock.Object);

		Assert.DoesNotThrow(() =>
		{
			var result = commentHandler.GetAllUserComments(1);
			Assert.That(result is { Success: true, Value: not null });
		});
	}

	[Test]
	public void GetCommentList_UserId_ShouldThrow_ResultException()
	{
		var mock = new Mock<ICommentService>();
		mock.Setup(x => x.GetAll(It.IsAny<int>())).Returns(() => null).Verifiable();

		var commentHandler = new CommentHandler(null!, null!, mock.Object);

		Assert.DoesNotThrow(() =>
		{
			var result = commentHandler.GetAllUserComments(1);
			Assert.That(result is { Success: false, Value: null });
		});
	}

	[Test]
	public void CreateComment_ShouldThrow_NoException()
	{
		var userHandlerMock = new Mock<UserHandler>(null!);
		userHandlerMock.Setup(x => x.GetOrCreateUser(It.IsAny<string>())).Returns(Result.FromSuccess(new User())).Verifiable();

		var postHandlerMock = new Mock<PostHandler>(null!);
		postHandlerMock.Setup(x => x.GetPost(It.IsAny<int>())).Returns(Result.FromSuccess(new Post())).Verifiable();

		var commentServiceMock = new Mock<ICommentService>();
		commentServiceMock.Setup(x => x.Create(It.IsAny<Comment>())).Verifiable();
		commentServiceMock.Setup(x => x.SaveChanges()).Verifiable();

		var commentHandler = new CommentHandler(userHandlerMock.Object, postHandlerMock.Object, commentServiceMock.Object);

		Assert.DoesNotThrow(() =>
		{
			var result = commentHandler.Create("1", 1, "content");
			Assert.That(result is { Success: true });
		});
	}

	[Test]
	public void CreateComment_ShouldThrow_ResultException_UserNotFound()
	{
		var userHandlerMock = new Mock<UserHandler>(null!);
		userHandlerMock.Setup(x => x.GetOrCreateUser(It.IsAny<string>())).Returns(Result.FromException<User>()).Verifiable();

		var postHandlerMock = new Mock<PostHandler>(null!);
		postHandlerMock.Setup(x => x.GetPost(It.IsAny<int>())).Returns(Result.FromSuccess(new Post())).Verifiable();

		var commentServiceMock = new Mock<ICommentService>();
		commentServiceMock.Setup(x => x.Create(It.IsAny<Comment>())).Verifiable();
		commentServiceMock.Setup(x => x.SaveChanges()).Verifiable();

		var commentHandler = new CommentHandler(userHandlerMock.Object, postHandlerMock.Object, commentServiceMock.Object);

		Assert.DoesNotThrow(() => {
			var result = commentHandler.Create("1", 1, "content");
			Assert.That(result is { Success: false });
		});
	}

	[Test]
	public void CreateComment_ShouldThrow_ResultException_PostNotFound()
	{
		var userHandlerMock = new Mock<UserHandler>(null!);
		userHandlerMock.Setup(x => x.GetOrCreateUser(It.IsAny<string>())).Returns(Result.FromSuccess(new User())).Verifiable();

		var postHandlerMock = new Mock<PostHandler>(null!);
		postHandlerMock.Setup(x => x.GetPost(It.IsAny<int>())).Returns(Result.FromException<Post>()).Verifiable();

		var commentServiceMock = new Mock<ICommentService>();
		commentServiceMock.Setup(x => x.Create(It.IsAny<Comment>())).Verifiable();
		commentServiceMock.Setup(x => x.SaveChanges()).Verifiable();

		var commentHandler = new CommentHandler(userHandlerMock.Object, postHandlerMock.Object, commentServiceMock.Object);

		Assert.DoesNotThrow(() =>
		{
			var result = commentHandler.Create("1", 1, "content");
			Assert.That(result is { Success: false });
		});
	}
}