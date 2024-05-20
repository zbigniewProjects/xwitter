using BlogProject.Application.Authentication.Queries;
using BlogProject.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.Authentication.Handler
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, BlogUser>
    {
        private readonly IUsersRepository _usersRepo;
        public GetUserHandler(IUsersRepository repo)
        {
            _usersRepo = repo;

        }
        public Task<BlogUser> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return _usersRepo.GetUserByID(request.userID);
        }
    }
}
