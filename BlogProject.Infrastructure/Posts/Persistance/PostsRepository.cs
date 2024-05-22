using BlogProject.Domain;
using BlogProject.Infrastructure.Common;
using BlogProject.Application;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using BlogProject.Application.Models.Post;
using BlogProject.Application.DTO;
using BlogProject.Application.DTOs;

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

        public async Task<GetPostRangeMainFeedDTO> GetPostRangeMainFeed(int startID, int count)
        {
            Post[] posts = await _postsDbContext.Posts!.OrderByDescending(p => p.Id)!.Skip(startID).Take(count).ToArrayAsync();
            int postsLeft = await _postsDbContext.Posts!.OrderByDescending(p => p.Id)!.Skip(startID + posts.Length).CountAsync();

            return new GetPostRangeMainFeedDTO { Posts = posts, PostsLeft = postsLeft };
        }

        public async Task<GetCommentsFromPostDTO> GetCommentsFromPost(uint postID, int startIndex, int count)
        {
            Post rawPost = await _postsDbContext.Posts!.FindAsync(postID);

            int finalResultLength = Math.Clamp(rawPost.Comments!.Count - startIndex, 0, count);

            if (finalResultLength == 0)
                return new GetCommentsFromPostDTO { CommentsLeft = 0};

            uint[] commentsIDs = new uint[finalResultLength];
            Array.Copy(rawPost.Comments.ToArray(), rawPost.Comments.Count-startIndex- finalResultLength, commentsIDs, 0, finalResultLength);

            Comment[] rawComments = await Task.FromResult(_postsDbContext.Comments.Where(p => commentsIDs.Contains(p.Id)).ToArray());

            return new GetCommentsFromPostDTO { Comments = rawComments, CommentsLeft = rawPost.Comments.Count - (startIndex + rawComments.Length)};
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

        public async Task<GetUsersPostDTO> GetUserPosts(uint userID, int startIndex, int count)
        {
            Post[] posts = _postsDbContext.Posts!.OrderByDescending(x => x.Id).Where(x => x.AuthorID == userID).Skip(startIndex).Take(count).ToArray();
            int remainingPostsCount = await _postsDbContext.Posts!.OrderByDescending(x => x.Id).Where(x => x.AuthorID == userID).Skip(startIndex+posts.Length).CountAsync();

            return new GetUsersPostDTO { Posts = posts, PostsLeft = remainingPostsCount };
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
