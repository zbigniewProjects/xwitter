using BlogProject.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.Authentication.Queries
{
    public record GetUserQuery(uint userID) : IRequest<BlogUser>;

}
