using CodeReview.Core.Interfaces;
using CodeReview.Core.Models;

namespace CodeReview.DAL.Services;

public class UserService(Context context) : IUserService
{
    public User? GetById(int id) => context.Users.Find(id);

    public User? GetByAccountUserId(string accountUserId) => context.Users.FirstOrDefault(x => x.ApplicationUserId == accountUserId);

    public void Create(User user) => context.Users.Add(user);

    public void Update(User user) => context.Users.Update(user);

    public void Delete(int id) => context.Users.Remove(GetById(id) ?? throw new NullReferenceException("User not found"));

    public void Delete(User user) => context.Users.Remove(user);

    public void SaveChanges() => context.SaveChanges();
}