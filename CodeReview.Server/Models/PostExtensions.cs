using Core.Models;

namespace CodeReview.Server.Models;

public static class PostExtensions
{
    public static PostView CreatePostView(this Post post) => new(post.Id, post.Author?.Id ?? -1, post.Title, post.Content, post.Comments.Select(comment => comment.Id), post.CreatedAt, post.Likes);
}