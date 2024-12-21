namespace CodeReview.Server.Models;

public class UserView(int id, string username, IEnumerable<int> posts, IEnumerable<int> comments)
{
    public int Id { get; set; } = id;

    public string Username { get; set; } = username;

    public IEnumerable<int> Posts { get; init; } = posts;

    public IEnumerable<int> Comments { get; init; } = comments;
}