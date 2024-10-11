using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

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
    public int Id { get; set; }

    [ForeignKey("AuthorId")]
    [Required]
    public User Author { get; set; }

    [StringLength(100)]
    public string Title { get; set; }

    [StringLength(1000)]
    public string Content { get; set; }
}