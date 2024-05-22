using BlogProject.Application.DTO;
using BlogProject.Application.Posts.Queries;
using MediatR;

namespace BlogProject.Application.Posts.Handlers
{
    public class GetPostRangeHandler(IPostsRepository _postsRepository) : IRequestHandler<GetPostsRangeQuery, GetPostRangeMainFeedDTO>
    {
        public async Task<GetPostRangeMainFeedDTO> Handle(GetPostsRangeQuery request, CancellationToken cancellationToken)
        {
            return await _postsRepository.GetPostRangeMainFeed(request.startIndex, request.count);
        }
    }
}
