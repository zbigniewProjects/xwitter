using BlogProject.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.Posts.Commands.AddComment
{
    public record AddCommentCommand(ClaimsPrincipal userSession, uint postID, string content) : IRequest<Comment>;
}
