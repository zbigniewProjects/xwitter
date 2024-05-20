namespace BlogProject.Application.Models.Post
{
    public class PostVM
    {
        /// <summary>
        /// this will be useful for setting direct link to post
        /// </summary>
        public uint Id { get; set; }
        public string? AuthorName { get; set; }
        public string? Content { get; set; }

        public CommentSupplyVM? Comments { get; set; }

        public DateTime Date { get; set; }

        public int NumberOfComments { get; set; }

        public bool IsAuthenticated { get; set; }

        public PostVM(PostEntry post, CommentSupplyVM comments, bool isAuthenticated)
        {
            Id = post.Id;
            AuthorName = post.AuthorName;
            Content = post.Content;
            Date = post.Date;
            NumberOfComments = post.NumberOfComments;
            IsAuthenticated = isAuthenticated;

            Comments = comments;
        }
    }
}
