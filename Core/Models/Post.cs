using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models;

public class Post
{
    public Post() { }

    public Post(User author, string title, string content)
    {
        Author = author;
        Title = title;
        Content = content;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [ForeignKey("AuthorId")]
    [Required]
    public User Author { get; init; }

    [StringLength(100)]
    public string Title { get; set; }

    [StringLength(1000)]
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public List<Comment> Comments { get; init; } = [];

    public int Likes { get; set; } = 0;
}