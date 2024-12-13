using CodeReview.Core.Models;
using CodeReview.DAL.Account;

namespace CodeReview.Server.Models;

public static class UserExtensions
{
    public static UserView? CreateUserView(this User user, AccountContext accountContext)
    {
        var accountUser = accountContext.Users.FirstOrDefault(accountUser => accountUser.Id == user.ApplicationUserId);

        if (accountUser == null) return null;

        return new UserView(user.Id, accountUser.UserName ?? "Unknown", user.Posts.Select(x => x.Id), user.Comments.Select(x => x.Id));
    }
}