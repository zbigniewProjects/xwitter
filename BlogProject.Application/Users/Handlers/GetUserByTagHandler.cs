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
    public class GetUserByTagHandler(IUsersRepository _usersRepository) : IRequestHandler<GetUserByTagQuery, BlogUser>
    {
        public async Task<BlogUser> Handle(GetUserByTagQuery request, CancellationToken cancellationToken)
        {
            return await _usersRepository.GetUserByTag(request.tag);
        }
    }
}
