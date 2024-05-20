using BlogProject.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.Posts.Commands.AddPost
{
    public class PostCommandHandler : 
        IRequestHandler<PostCommand, Post>
    {

        private readonly IPostsRepository _postsRepository;
        private readonly UserManager<BlogUser> _userManager;
        private readonly IUsersRepository _usersRepositor;

        public PostCommandHandler(IPostsRepository repository, IUsersRepository usersRepository, UserManager<BlogUser> userManager)
        {
            _postsRepository = repository;
            _userManager = userManager;
            _usersRepositor = usersRepository; 
        }

        public async Task<Post> Handle(PostCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(request.claimsPrincipal);

            Post post = new()
            {
               AuthorID = (uint)user?.Id!,
               Content = request.content,
               PostedAt = DateTime.UtcNow,
            };

            //PostCommand cmd = new PostCommand((uint)user?.Id, request.content);

            await _postsRepository.AddPostAsync(post, cancellationToken);//_postDbContext.AddAsync(post);

            return post;
        }
    }
}
