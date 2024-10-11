using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class User
{
    public User() { }

    public User(string username)
    {
        Username = username;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [EmailAddress]
    [Required]
    public string Username { get; set; }

    public List<Post> Posts { get; set; } = [];
}