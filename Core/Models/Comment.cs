using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeReview.Core.Models;

[Table("Comments")]
public class Comment
{
	public Comment()
	{
	}

	public Comment(User author, Post post, string content)
	{
		Author  = author;
		Post    = post;
		Content = content;
	}

	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	[Required]
	public User Author { get; set; }

	[Required]
	public Post Post { get; set; }

	[StringLength(1000)]
	public string Content { get; set; }

	public int Likes { get; set; } = 0;
}