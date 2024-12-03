using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models;

public class User
{
    public User() { }

    public User(string accountUserId)
    {
        AccountUserId = accountUserId;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string AccountUserId { get; init; }

    public List<Post> Posts { get; set; } = [];

    public List<Comment> Comments { get; set; } = [];
}