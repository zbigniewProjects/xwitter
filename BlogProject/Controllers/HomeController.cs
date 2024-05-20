using BlogProject.Application;
using BlogProject.Application.Models.Post;
using BlogProject.Application.Posts.Queries;
using BlogProject.Domain;
using BlogProject.MVC.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index()
        {
            PostEntry[] posts = await _mediatr.Send(new GetPostsRangeQuery(0, 2));

            BlogUser user = await _signInManager.UserManager.GetUserAsync(User);

            return View(new IndexVM { Posts = posts, Username = (user != null ? user.UserName : null) });
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

            PostEntry[] posts = await _mediatr.Send(new GetPostsRangeQuery(numberOfPostsToLoadAtATime * cycles, numberOfPostsToLoadAtATime)); //await _postsRepository.GetPosts(0,2);

            return PartialView("_FeedPartial", posts);
        }

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
