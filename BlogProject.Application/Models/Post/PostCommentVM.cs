namespace BlogProject.Application.Models.Post
{
    public class PostCommentVM
    {
        public string? Content { get; set; }
        public string? AuthorName { get; set; }
        public DateTime? PostedAt { get; set; }
        public uint CommentID { get; set; }
        public uint AuthorID { get; set; }
        
    }
}
