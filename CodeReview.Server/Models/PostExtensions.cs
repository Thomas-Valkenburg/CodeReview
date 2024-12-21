using CodeReview.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace CodeReview.Server.Models;

public static class PostExtensions
{
    public static PostView CreatePostView(this Post post, IdentityUser identityUser) => new(post.Id, post.Author?.Id ?? -1, identityUser.UserName ?? identityUser.Email ?? identityUser.Id, post.Title, post.Content, post.Comments.Select(comment => comment.Id), post.CreatedAt, post.Likes);
    
    public static PostView CreatePostView(this Post post, string identityUserUsername) => new(post.Id, post.Author?.Id ?? -1, identityUserUsername, post.Title, post.Content, post.Comments.Select(comment => comment.Id), post.CreatedAt, post.Likes);
}