using BlogProject.Application.DTO;
using BlogProject.Application.Posts.Queries;
using MediatR;

namespace BlogProject.Application.Posts.Handlers
{
    public class GetCommentsFromPostHandler(IPostsRepository _postsRepo) :
        IRequestHandler<GetCommentsFromPostQuery, GetCommentsFromPostDTO>
    {
        public async Task<GetCommentsFromPostDTO> Handle(GetCommentsFromPostQuery request, CancellationToken cancellationToken)
        {
            return await _postsRepo.GetCommentsFromPost(request.postID, request.startIndex, request.count);
        }
    }
}
