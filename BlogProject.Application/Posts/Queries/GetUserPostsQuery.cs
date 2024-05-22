using BlogProject.Application.DTOs;
using BlogProject.Application.Models.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.Posts.Queries
{
    public record GetUserPostsQuery(uint userID, int startIndex, int count) : IRequest<GetUsersPostDTO>;
}
