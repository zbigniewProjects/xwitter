using BlogProject.Domain;
using BlogProject.Infrastructure.Common;
using BlogProject.Application;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using BlogProject.Application.Models.Post;

namespace BlogProject.Infrastructure.Posts.Persistance
{
    internal class PostsRepository(PostsDbContext _postsDbContext) : IPostsRepository
    {
        public async Task AddPostAsync(Post post, CancellationToken ctoken)
        {
            await _postsDbContext.Posts!.AddAsync(post, ctoken);
            await _postsDbContext.SaveChangesAsync();
        }

        public async Task AddCommentAsync(Comment comment, CancellationToken ctoken)
        {
            await _postsDbContext.Comments.AddAsync(comment, ctoken);
            await _postsDbContext.SaveChangesAsync();
        }

        public async Task<Post> FindById(uint id)
        {
            return await _postsDbContext.Posts.FindAsync(id);
        }

        public async Task<Post[]> GetPosts(int startID, int count)
        {
           return await _postsDbContext.Posts.OrderByDescending(p => p.Id)!.Skip(startID).Take(count).ToArrayAsync();
        }

        public async Task<Comment[]> GetCommentsFromPost(uint postID, int startIndex, int count)
        {
            Post rawPost = await _postsDbContext.Posts.FindAsync(postID);

            int finalResultLength = Math.Clamp(rawPost.Comments.Count - startIndex, 0, count);

            if (finalResultLength == 0)
                return null!;

            uint[] commentsIDs = new uint[finalResultLength];
            Array.Copy(rawPost.Comments.ToArray(), rawPost.Comments.Count-startIndex- finalResultLength, commentsIDs, 0, finalResultLength);

            Comment[] rawComments = await Task.FromResult(_postsDbContext.Comments.Where(p => commentsIDs.Contains(p.Id)).ToArray());
            return rawComments;
        }

        public async Task<Comment> GetCommentByID(uint commentID)
        {
            return await _postsDbContext.Comments.FirstOrDefaultAsync(x => x.Id == commentID);
        }

        public async Task<bool> RemoveComment(Comment comment)
        {
            _postsDbContext.Comments.Remove(comment);
            await _postsDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            return await _postsDbContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public async Task<PostEntry[]> GetUserPosts(BlogUser user, int startIndex, int count)
        {
            Post[] posts = _postsDbContext.Posts!.OrderByDescending(x => x.Id).Where(x => x.AuthorID == user.Id).Skip(startIndex).Take(count).ToArray();
            PostEntry[] entryPosts = new PostEntry[posts.Length];

            for (int i = 0; i < entryPosts.Length; i++)
            {
                Post post = posts[i];
                PostEntry postEntry = new PostEntry {
                    Id = post.Id,
                    AuthorName = user.UserName,
                    Content = post.Content,
                    Date = (DateTime)post.PostedAt!,
                    NumberOfComments = post.Comments!.Count
                };
                entryPosts[i] = postEntry;
            }
            return entryPosts;
        }

        public async Task<Post> GetPostByID(uint postID)
        {
            return await _postsDbContext.Posts!.FirstAsync(x => x.Id == postID);
        }

        public async Task<bool> RemovePost(Post postToRemove) 
        {
            _postsDbContext.Posts!.Remove(postToRemove);
            await _postsDbContext.SaveChangesAsync();
            return true;
        }
    }
}
