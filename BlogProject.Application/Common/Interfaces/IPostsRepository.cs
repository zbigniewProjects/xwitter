using BlogProject.Application.Models.Post;
using BlogProject.Domain;

namespace BlogProject.Application
{
    public interface IPostsRepository
    {
        Task AddCommentAsync(Comment comment, CancellationToken ctoken);
        Task AddPostAsync(Post post, CancellationToken ctoken);
        Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);
        Task<Post> FindById(uint id);
        Task<Comment[]> GetCommentsFromPost(uint postID, int startIndex, int count);
        Task <Post[]> GetPosts(int startIndex, int count);
        Task<Comment> GetCommentByID(uint commentID);
        Task<bool> RemoveComment(Comment comment);
        Task<PostEntry[]> GetUserPosts(BlogUser user, int startIndex, int count);
        Task<Post> GetPostByID(uint postID);
        Task<bool> RemovePost(Post postToRemove);
    }
}