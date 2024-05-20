using BlogProject.Application.Models.Post;
using BlogProject.Application.Posts.Queries;
using BlogProject.Domain;
using MediatR;

namespace BlogProject.Application.Posts.Handlers
{
    public class GetPostRangeHandler(IPostsRepository _postsRepository, IUsersRepository _usersRepository) : IRequestHandler<GetPostsRangeQuery, PostEntry[]>
    {
        public async Task<PostEntry[]> Handle(GetPostsRangeQuery request, CancellationToken cancellationToken)
        {
            Post[] rawPosts = await _postsRepository.GetPosts(request.startIndex, request.count);

            uint[] authorsIDs = new uint[rawPosts.Length];
            for (int i = 0; i < rawPosts.Length; i++) 
            {
                authorsIDs[i] = rawPosts[i].AuthorID;
            }

            BlogUser[] rawUsers = _usersRepository.GetUsersByIDs(authorsIDs);
            PostEntry[] postsVM = new PostEntry[rawPosts.Length];

            for (int i = 0; i < postsVM.Length; i++)
            {
                PostEntry postVM = new PostEntry();
                Post rawPost = rawPosts[i];

                postVM.AuthorName = rawUsers.First(x=> x.Id == rawPost.AuthorID).UserName;
                postVM.Content = rawPost.Content;
                postVM.Date = (DateTime)rawPost.PostedAt!;
                postVM.Id = rawPost.Id;
                postVM.NumberOfComments = rawPost.Comments!.Count;
                postsVM[i] = postVM;
            }

            return postsVM;
        }
    }
}
