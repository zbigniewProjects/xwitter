namespace BlogProject.Application.Models.Post
{
    public class CommentSupplyVM
    {
        public bool IsAuthenticated;
        public uint UserID;
        public uint PostID;
        public PostCommentVM[]? CommentsVMs { get; set; }

        public int CommentsLeft;
    }
}
