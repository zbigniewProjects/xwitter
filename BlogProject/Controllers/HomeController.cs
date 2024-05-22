using BlogProject.Application;
using BlogProject.Application.DTO;
using BlogProject.Application.Models.Post;
using BlogProject.Application.Posts.Queries;
using BlogProject.Application.Users.Queries;
using BlogProject.Domain;
using BlogProject.Infrastructure.Users.Persistance;
using BlogProject.MVC.Utils;
using BlogProject.MVC.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol;
using System.Diagnostics;

namespace BlogProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMediator _mediatr;
        private readonly SignInManager<BlogUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, IMediator mediatr, SignInManager<BlogUser> signInManager)
        {
            _logger = logger;
            _mediatr = mediatr;
            _signInManager = signInManager;
        }

        #region Main feed functionality
        public async Task<IActionResult> Index()
        {
            BlogUser user = await _signInManager.UserManager.GetUserAsync(User);
            return View(new IndexVM { Username = (user != null ? user.UserName : null) });
        }

        [HttpGet("/Home/LoadMorePosts/{value}")]
        public async Task <IActionResult> LoadMorePosts(string value)
        {
            int cycles;
            try
            {
                cycles = Convert.ToInt16(value);
            }
            catch {
                //if user sent here in get request something that is not a number, then throw bad request response
                return BadRequest();
            }

            int numberOfPostsToLoadAtATime = 5;
            GetPostRangeMainFeedDTO posts = await _mediatr.Send(new GetPostsRangeQuery(numberOfPostsToLoadAtATime * cycles, numberOfPostsToLoadAtATime));

            Post[] rawPosts = posts.Posts!;

            uint[] authorsIDs = new uint[rawPosts.Length];
            for (int i = 0; i < rawPosts.Length; i++)
            {
                authorsIDs[i] = rawPosts[i].AuthorID;
            }

            BlogUser[] rawUsers = await _mediatr.Send(new GetUsersQuery(authorsIDs));

            //after posts and their autors are retrieved from db, model them for html rendering
            PostEntry[] postsVM = new PostEntry[rawPosts.Length];
            for (int i = 0; i < postsVM.Length; i++)
            {
                PostEntry postVM = new PostEntry();
                Post rawPost = rawPosts[i];

                BlogUser currentAuthor = rawUsers.FirstOrDefault(x => x.Id == rawPost.AuthorID)!;
                postVM.AuthorName = currentAuthor != null ? currentAuthor.UserName : "DeletedUser";
                postVM.Content = rawPost.Content;
                postVM.Date = (DateTime)rawPost.PostedAt!;
                postVM.Id = rawPost.Id;
                postVM.NumberOfComments = rawPost.Comments!.Count;
                postsVM[i] = postVM;
            }

            string postsString = await this.RenderViewToStringAsync("_FeedPartial", new PostRangeMainFeedVM {
                Entries = postsVM,
                EntriesLeft = posts.PostsLeft
            });

            return Json(new { html = postsString, elementsLeft = posts.PostsLeft});
        }
        #endregion

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorVM { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
