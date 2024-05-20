using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogProject.Domain;
using System.Security.Claims;

namespace BlogProject.Application.Posts.Commands.AddPost
{
    public record PostCommand(ClaimsPrincipal claimsPrincipal, string content) : IRequest<Post>;
}
