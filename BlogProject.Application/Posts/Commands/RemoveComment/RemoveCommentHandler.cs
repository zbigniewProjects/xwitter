using BlogProject.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.Posts.Commands.RemoveComment
{
    public class RemoveCommentHandler(IPostsRepository _postsRepository, IUsersRepository _usersRepository) :
        IRequestHandler<RemoveCommentCommand, bool>
    {
        public async Task<bool> Handle(RemoveCommentCommand request, CancellationToken cancellationToken)
        {
            BlogUser user = await _usersRepository.GetUserByClaimPrincipal(request.userKey);

            if (user == null) 
                return false;

            Post post = await _postsRepository.FindById(request.postID);

            if (post == null)
                return false;

            Comment com = await _postsRepository.GetCommentByID(request.commentID);

            if(com == null)
                return false;

            if (com.AuthorID != user.Id)
                return false;

            string sql = @"
            UPDATE ""Posts""
            SET ""Comments"" = array_remove(""Comments"", {0})
            WHERE ""Id"" = {1} AND {0} = ANY(""Comments"")";

            int code = await _postsRepository.ExecuteSqlRawAsync(sql, com.Id, post.Id);

            return await _postsRepository.RemoveComment(com);
        }
    }
}
