namespace BlogProject.Application.Models.Post
{
    public class PostEntry
    {
        /// <summary>
        /// this will be useful for setting direct link to post
        /// </summary>
        public uint Id { get; set; }
        public string? AuthorName { get; set; }
        public string? Content { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfComments { get; set; }
    }
}
