using BlogProject.Application.DTOs;
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
    internal class GetUserPostsHandler(IPostsRepository _postsRepo, IUsersRepository _usersRepo) :
        IRequestHandler<GetUserPostsQuery, GetUsersPostDTO>
    {
        public async Task<GetUsersPostDTO> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
        {
            GetUsersPostDTO posts = await _postsRepo.GetUserPosts(request.userID, request.startIndex, request.count);
            return posts;
        }
    }
}
