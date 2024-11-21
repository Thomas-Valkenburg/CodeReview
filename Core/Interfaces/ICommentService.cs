using Core.Models;

namespace Core.Interfaces;

public interface ICommentService
{
    Comment? GetById(int id);

    List<Comment>? GetAll(int postId);

    void Create(Comment comment);

    void Update(Comment comment);

    void Delete(int id);

    void Delete(Comment comment);

    void SaveChanges();
}