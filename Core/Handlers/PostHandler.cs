using CodeReview.Core.Interfaces;
using CodeReview.Core.Models;

namespace CodeReview.Core.Handlers;

public class PostHandler(IPostService postService)
{
    public static event EventHandler? OnPostCreated;

	public Result<List<Post>> GetPostList(int amount, SortOrder sortOrder = SortOrder.Newest, params List<string>? filterStrings) => 
		Result.FromSuccess(postService.Take(amount, sortOrder, filterStrings));

	public virtual Result<Post> GetPost(int id)
	{
		var post = postService.Get(id);

		return post is null ? Result.FromException<Post>("Post not found") : Result.FromSuccess(post);
	}

	public Result<Post> CreatePost(User user, string title, string content)
	{
		if (user is null) return Result.FromException<Post>("User not found");

		var newPost = new Post(user, title, content);

		postService.Create(newPost);
		postService.SaveChanges();

		var post = postService.GetAllFromUser(user.Id)?.OrderBy(x => x.CreatedAt).Last();

		OnPostCreated?.Invoke(this, EventArgs.Empty);

		return post is not null ? Result.FromSuccess(post) : Result.FromException<Post>("Unknown error");
	}
}