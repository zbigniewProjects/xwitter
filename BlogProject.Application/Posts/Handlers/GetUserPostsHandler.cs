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
    internal class GetUserPostsHandler(IPostsRepository _postsRepo, IUsersRepository _usersRepo) : IRequestHandler<GetUserPostsQuery, PostEntry[]>
    {
        public async Task<PostEntry[]> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
        {
            BlogUser user = await _usersRepo.GetUserByID(request.userID);
            PostEntry[] posts = await _postsRepo.GetUserPosts(user, request.startIndex, request.count);
            return posts;
        }
    }
}
