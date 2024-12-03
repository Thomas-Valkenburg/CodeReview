using Core.Models;

namespace CodeReview.Server.Models;

public static class PostExtensions
{
    public static PostView CreatePostView(this Post post) => new(post.Id, post.Author?.Id ?? default, post.Title, post.Content, post.CreatedAt, post.Likes);
}