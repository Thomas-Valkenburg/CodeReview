using Core.Models;

namespace Core.Interfaces;

public interface IPostService
{
    Post? GetById(int id);

    List<Post>? GetAllFromUser(int ownerId);

    List<Post> Take(int amount);

    void Create(Post post);

    void Update(Post post);

    void Delete(int id);

    void Delete(Post post);

    void SaveChanges();
}