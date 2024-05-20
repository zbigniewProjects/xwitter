using BlogProject.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.Posts.Commands.RemovePost
{
    public class RemoveCommandHandler(IPostsRepository _postsRepository, IUsersRepository _usersRepository) : IRequestHandler<RemovePostCommand, bool>
    {
        public async Task<bool> Handle(RemovePostCommand request, CancellationToken cancellationToken)
        {
            Post post = await _postsRepository.GetPostByID(request.postID);
            if (post.AuthorID != request.user.Id) 
                return false;

            await _postsRepository.RemovePost(post);
            return true;
        }
    }
}
