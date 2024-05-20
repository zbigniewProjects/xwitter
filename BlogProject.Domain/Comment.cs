using System.ComponentModel.DataAnnotations;

namespace BlogProject.Domain
{
    public class Comment
    {
        [Key]
        public uint Id { get; set; }
        public uint AuthorID { get; set; }
        public string? Content { get; set; }
        public uint[]? LikesID { get; set; }
        public DateTime? PostedAt { get; set; }
    }
}
