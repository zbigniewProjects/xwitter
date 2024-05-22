using BlogProject.Domain;
using MediatR;

namespace BlogProject.Application.Users.Queries
{
    public record GetUsersQuery(uint[] ids) : IRequest<BlogUser[]>;
}
