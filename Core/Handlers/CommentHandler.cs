using CodeReview.Core.Interfaces;
using CodeReview.Core.Models;

namespace CodeReview.Core.Handlers;

public class CommentHandler(UserHandler userHandler, PostHandler postHandler, ICommentService commentService)
{
	public Result<Comment> Get(int id)
	{
		var comment = commentService.GetById(id);

		return comment is null ? Result.FromException<Comment>("Comment not found") : Result.FromSuccess(comment);
	}

	public Result<List<Comment>> GetAllUserComments(int userId)
	{
		var comments = commentService.GetAll(userId);

		return comments == null ? Result.FromException<List<Comment>>("User not found") : Result.FromSuccess(comments);
	}

	public Result Create(string identityUserId, int postId, string content)
	{
		var userResult = userHandler.GetOrCreateUser(identityUserId);
		var postResult = postHandler.GetPost(postId);

		if (!userResult.Success || userResult.Value is not { } user) return Result.FromException("User not found");
		if (!postResult.Success || postResult.Value is not { } post) return Result.FromException("Post not found");

		//var content = EditorContentHandler.Process(editorContent);

		var newComment = new Comment(user, post, content);

		commentService.Create(newComment);
		commentService.SaveChanges();

		return Result.FromSuccess();
	}
}