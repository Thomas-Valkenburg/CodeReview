using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services;

public class PostService(Context context) : IPostService
{
    public Post? GetById(int id) => context.Posts.Find(id);

    public List<Post>? GetAllFromUser(int ownerId)
    {
        var user = context.Users.Include(x => x.Posts).FirstOrDefault(x => x.Id == ownerId);

        return user?.Posts;
    }

    public List<Post> Take(int amount, SortOrder sortOrder)
    {
		return sortOrder switch
		{
			SortOrder.Alphabetical         => context.Posts.OrderByDescending(post => post.Title).Take(amount).ToList(),
			SortOrder.AlphabeticalInverted => context.Posts.OrderBy(post => post.Title).Take(amount).ToList(),
			SortOrder.Newest               => context.Posts.OrderByDescending(post => post.CreatedAt).Take(amount).ToList(),
			SortOrder.Oldest               => context.Posts.OrderBy(post => post.CreatedAt).Take(amount).ToList(),
			SortOrder.TopRated             => context.Posts.OrderByDescending(post => post.Likes).Take(amount).ToList(),
			_                              => throw new ArgumentOutOfRangeException(nameof(sortOrder), sortOrder, null)
		};
	}

    public List<Post> Take(int amount, SortOrder sortOrder = SortOrder.Newest, params List<string> filterStrings) =>
	    context.Posts.Where(post =>
			    filterStrings.Any(filterString =>
				    post.Title.Contains(filterString) || post.Content.Contains(filterString)))
		    .ToList();

	public void Create(Post post) => context.Posts.Add(post);

    public void Update(Post post) => context.Posts.Update(post);

    public void Delete(int id) => context.Posts.Remove(GetById(id) ?? throw new NullReferenceException("Post not found"));

    public void Delete(Post post) => context.Posts.Remove(post);

    public void SaveChanges() => context.SaveChanges();
}