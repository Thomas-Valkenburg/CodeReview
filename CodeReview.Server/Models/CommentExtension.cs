using Core.Models;

namespace CodeReview.Server.Models;

public static class CommentExtension
{
    public static CommentView CreateCommentView(this Comment comment) => new(comment.Id, comment.Author.Id, comment.Content, comment.Post.Id);
}