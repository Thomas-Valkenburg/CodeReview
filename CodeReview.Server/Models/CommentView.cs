namespace CodeReview.Server.Models;

public class CommentView(int id, int authorId, string authorUsername, string content, int postId, int likes)
{
    public int Id { get; set; } = id;

    public int AuthorId { get; set; } = authorId;

    public string AuthorUsername { get; set; } = authorUsername;

    public string Content { get; set; } = content;

    public int PostId { get; set; } = postId;

    public int Likes { get; set; } = likes;
}