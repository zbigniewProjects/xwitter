using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.Posts.Commands.RemoveComment
{
    public record RemoveCommentCommand(ClaimsPrincipal userKey, uint postID, uint commentID): IRequest<bool>;
}
