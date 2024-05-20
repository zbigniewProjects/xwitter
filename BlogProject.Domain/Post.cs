using System.ComponentModel.DataAnnotations;

namespace BlogProject.Domain;

/// <summary>
/// Post schema for database
/// </summary>
public class Post
{
    [Key]
    public uint Id { get; set; }
    public uint AuthorID { get; set; } 
    public string? Content { get; set; }
    public List<uint>? Comments { get; set; } = new List<uint>();
    public DateTime? PostedAt { get; set; }
}
