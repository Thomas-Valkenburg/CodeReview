using Core.Models;

namespace CodeReview.Server.Models;

public class PostView(int id, int authorId, string title, string content, DateTime createdAt, int likes)
{
    public int Id { get; set; } = id;

    public int? AuthorId { get; set; } = authorId;

    public string Title { get; set; } = title;

    public string Content { get; set; } = content;

    public DateTime CreatedAt { get; set; } = createdAt;

    public int Likes { get; set; } = likes;
}