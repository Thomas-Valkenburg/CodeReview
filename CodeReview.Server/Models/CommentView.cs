namespace CodeReview.Server.Models;

public class CommentView(int id, int authorId, string content, int postId)
{
    public int Id { get; set; } = id;

    public int AuthorId { get; set; } = authorId;

    public string Content { get; set; } = content;

    public int PostId { get; set; } = postId;
}