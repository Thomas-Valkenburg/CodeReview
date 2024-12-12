using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services;

public class CommentService(Context context) : ICommentService
{
    public Comment? GetById(int id) => context.Comments.Include(comment => comment.Author).Include(comment => comment.Post).FirstOrDefault(comment => comment.Id == id);

    public List<Comment>? GetAll(int postId)
    {
        var post = context.Posts.Include(x => x.Comments).FirstOrDefault(x => x.Id == postId);
        return post?.Comments;
    }

    public void Create(Comment comment) => context.Comments.Add(comment);

    public void Update(Comment comment) => context.Comments.Update(comment);

    public void Delete(int id) => context.Comments.Remove(GetById(id) ?? throw new NullReferenceException("Comment not found"));

    public void Delete(Comment comment) => context.Comments.Remove(comment);

    public void SaveChanges() => context.SaveChanges();
}