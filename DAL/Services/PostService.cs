using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services;

public class PostService(Context context) : IPostService
{
    public Post? GetById(int id) => context.Posts.Include(post => post.Author).Include(post => post.Comments).FirstOrDefault(x => x.Id == id);

    public List<Post>? GetAllFromUser(int ownerId)
    {
        var user = context.Users.Include(x => x.Posts).FirstOrDefault(x => x.Id == ownerId);

        return user?.Posts;
    }

    public List<Post> Take(int amount) => context.Posts.Include(post => post.Author).Take(amount).ToList();

    public void Create(Post post) => context.Posts.Add(post);

    public void Update(Post post) => context.Posts.Update(post);

    public void Delete(int id) => context.Posts.Remove(GetById(id) ?? throw new NullReferenceException("Post not found"));

    public void Delete(Post post) => context.Posts.Remove(post);

    public void SaveChanges() => context.SaveChanges();
}