using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeReview.Core.Models;

[Table("Users")]
public class User
{
    public User() { }

	public User(string applicationUserId)
    {
		ApplicationUserId = applicationUserId;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; init; }

	public string ApplicationUserId { get; set; }

    public List<Post> Posts { get; set; } = [];

    public List<Comment> Comments { get; set; } = [];

}