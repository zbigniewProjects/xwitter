using BlogProject.Domain;

namespace BlogProject.Application.DTO
{
    public class GetPostRangeMainFeedDTO
    {
        public Post[]? Posts { get; set; }
        public int PostsLeft { get; set; }
    }
}
