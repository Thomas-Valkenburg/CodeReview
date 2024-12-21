namespace CodeReview.Server.Models;

public class PostView(int id, int authorId, string authorUsername, string title, string content, IEnumerable<int> comments, DateTime createdAt, int likes)
{
    public int Id { get; set; } = id;

    public int? AuthorId { get; set; } = authorId;

    public string AuthorUsername { get; set; } = authorUsername;

    public string Title { get; set; } = title;

    public string Content { get; set; } = content;

    public IEnumerable<int> Comments { get; set; } = comments;

    public DateTime CreatedAt { get; set; } = createdAt;

    public int Likes { get; set; } = likes;
}