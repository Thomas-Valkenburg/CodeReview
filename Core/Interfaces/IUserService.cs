using Core.Models;

namespace Core.Interfaces;

public interface IUserService
{
    User? GetById(int id);

    void Create(User user);

    void Update(User user);

    void Delete(int id);

    void Delete(User user);

    void SaveChanges();
}