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
    public class GetPostByIdHandler (IPostsRepository _postRepo, IUsersRepository _usersRepository): IRequestHandler<GetPostByIdQuery, PostEntry>
    {
        public async Task<PostEntry> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            //make raw sql queries
            Post rawPost = await _postRepo.FindById(request.id);
            if (rawPost == null)
                return null!;

            BlogUser rawAuthor = await _usersRepository.GetUserByID(rawPost.AuthorID);

            PostEntry post = new()
            {
                Id = rawPost.Id,
                AuthorName = rawAuthor.UserName,
                Content = rawPost.Content,
                Date = (DateTime)rawPost.PostedAt!,
                NumberOfComments = rawPost.Comments!.Count
            };
            return post;
        }
    }
}
