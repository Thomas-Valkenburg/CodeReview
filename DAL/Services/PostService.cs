using CodeReview.Core;
using CodeReview.Core.Interfaces;
using CodeReview.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeReview.DAL.Services;

public class PostService(Context context) : IPostService
{
    public Post? Get(int id) => context.Posts.Include(post => post.Author).Include(post => post.Comments).FirstOrDefault(x => x.Id == id);

    public List<Post>? GetAllFromUser(int ownerId)
    {
        var user = context.Users.Include(x => x.Posts).FirstOrDefault(x => x.Id == ownerId);

        return user?.Posts;
    }

    public List<Post> Take(int amount, SortOrder sortOrder, params List<string>? filter)
    {
		var posts = context.Posts.Include(post => post.Author).Include(post => post.Comments).ToList();

		if (filter?.Count > 0)
		{
			posts = posts.Where(post => filter.Any(keyword => post.Title.Contains(keyword) || post.Content.Contains(keyword))).ToList();
		}

		posts = sortOrder switch
		{
			SortOrder.Alphabetical         => posts.OrderBy(post => post.Title).ToList(),
			SortOrder.AlphabeticalInverted => posts.OrderByDescending(post => post.Title).ToList(),
			SortOrder.Newest               => posts.OrderBy(post => post.CreatedAt).ToList(),
			SortOrder.Oldest               => posts.OrderByDescending(post => post.CreatedAt).ToList(),
			SortOrder.TopRated             => posts.OrderBy(post => post.Likes).ToList(),
			_                              => throw new ArgumentOutOfRangeException(nameof(sortOrder), sortOrder, null)
		};

		return posts.Take(amount).ToList();
	}

	public void Create(Post post)
	{
		if (post.Author == null)
		{
			throw new NullReferenceException("Post author not found");
		}

		context.Posts.Add(post);
	}

	public void Update(Post post) => context.Posts.Update(post);

    public void Delete(int id) => context.Posts.Remove(Get(id) ?? throw new NullReferenceException("Post not found"));

    public void Delete(Post post) => context.Posts.Remove(post);

    public void SaveChanges() => context.SaveChanges();
}