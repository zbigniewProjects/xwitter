using BlogProject.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.Users.Queries
{
    public record GetUserByTagQuery(string tag) : IRequest<BlogUser>;
}
