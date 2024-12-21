using CodeReview.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace CodeReview.Server.Models;

public static class CommentExtension
{
    public static CommentView CreateCommentView(this Comment comment, IdentityUser identityUser) => new(comment.Id, comment.Author.Id, identityUser.UserName ?? identityUser.Email ?? identityUser.Id, comment.Content, comment.Post.Id, comment.Likes);
    
    public static CommentView CreateCommentView(this Comment comment, string identityUserUsername) => new(comment.Id, comment.Author.Id, identityUserUsername, comment.Content, comment.Post.Id, comment.Likes);
}