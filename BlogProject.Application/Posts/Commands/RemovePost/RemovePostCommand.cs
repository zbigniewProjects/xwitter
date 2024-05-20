using BlogProject.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.Posts.Commands.RemovePost
{
    public record RemovePostCommand (BlogUser user, uint postID) : IRequest<bool>;
}
