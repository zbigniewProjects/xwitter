using BlogProject.Application.Users.Queries;
using BlogProject.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.Users.Handlers
{
    public class GetUsersQueryHandler(IUsersRepository _usersRepo) : IRequestHandler<GetUsersQuery, BlogUser[]>
    {
        public Task<BlogUser[]> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_usersRepo.GetUsersByIDs(request.ids));
        }
    }
}
