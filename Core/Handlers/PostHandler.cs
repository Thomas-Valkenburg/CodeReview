using CodeReview.Core.Interfaces;
using CodeReview.Core.Models;

namespace CodeReview.Core.Handlers;

public class PostHandler(IPostService postService)
{
	public Result<List<Post>> GetPostList(int amount, SortOrder sortOrder = SortOrder.Newest, params List<string>? filterStrings) => 
		Result.FromSuccess(postService.Take(amount, sortOrder, filterStrings));

	public Result<Post> GetPost(int id)
	{
		var post = postService.GetById(id);

		return post is null ? Result.FromException<Post>("Post not found") : Result.FromSuccess(post);
	}
}