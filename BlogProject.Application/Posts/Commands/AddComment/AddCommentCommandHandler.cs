using BlogProject.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.Posts.Commands.AddComment
{
    public class AddCommentCommandHandler(IPostsRepository _postsRepository, UserManager<BlogUser> _userManager) : IRequestHandler<AddCommentCommand, Comment>
    {
        public async Task<Comment> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(request.userSession);

            Comment comment = new()
            {
                AuthorID = user!.Id,
                Content = request.content,
                PostedAt = DateTime.UtcNow,
            };

            await _postsRepository.AddCommentAsync(comment, cancellationToken);

            string sql = @"
            UPDATE ""Posts""
            SET ""Comments"" = array_append(""Comments"", {0})
            WHERE ""Id"" = {1}"
            ;
            await _postsRepository.ExecuteSqlRawAsync(sql, comment.Id, request.postID);

            return comment;
        }
    }
}
