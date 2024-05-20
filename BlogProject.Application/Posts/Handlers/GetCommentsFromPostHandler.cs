using BlogProject.Application.Models.Post;
using BlogProject.Application.Posts.Queries;
using BlogProject.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.Posts.Handlers
{
    public class GetCommentsFromPostHandler(IPostsRepository _postsRepo, IUsersRepository _usersRepo) :
        IRequestHandler<GetCommentsFromPostQuery, PostCommentVM[]>
    {
        public async Task<PostCommentVM[]> Handle(GetCommentsFromPostQuery request, CancellationToken cancellationToken)
        {
            
            Comment[] rawComments = await _postsRepo.GetCommentsFromPost(request.postID, request.startIndex, request.count);

            if (rawComments == null)
                return null!;

            uint[] authorsIDs = new uint[rawComments.Length];
            for (int i = 0; i < rawComments.Length; i++) 
            {
                authorsIDs[i] = rawComments[i].AuthorID;
            }

            BlogUser[] blogUsers = _usersRepo.GetUsersByIDs(authorsIDs);

            //map vm's
            PostCommentVM[] postCommentVMs = new PostCommentVM[rawComments.Length];
            for (int i = 0; i < postCommentVMs.Length; i++)
            {
                PostCommentVM postCommentVM = new PostCommentVM();
                BlogUser user = blogUsers.First(x => x.Id == authorsIDs[i]);
                postCommentVM.AuthorName = user.UserName;
                postCommentVM.Content = rawComments[i].Content;
                postCommentVM.PostedAt = rawComments[i].PostedAt;
                postCommentVM.CommentID = rawComments[i].Id;
                postCommentVM.AuthorID = user.Id;
                
                postCommentVMs[i] = postCommentVM;
            }

            return postCommentVMs;
        }
    }
}
