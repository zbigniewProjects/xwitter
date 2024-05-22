using BlogProject.Application.DTO;
using MediatR;

namespace BlogProject.Application.Posts.Queries
{
    public record GetPostsRangeQuery (int startIndex, int count) : IRequest<GetPostRangeMainFeedDTO>;
}
