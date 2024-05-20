using BlogProject.Application.Models.Post;
using BlogProject.Application.Posts.Commands.AddComment;
using BlogProject.Application.Posts.Commands.AddPost;
using BlogProject.Application.Posts.Commands.RemoveComment;
using BlogProject.Application.Posts.Commands.RemovePost;
using BlogProject.Application.Posts.Queries;
using BlogProject.Contracts.Post;
using BlogProject.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BlogProject.Controllers
{

    public class PostController : Controller
    {
        private readonly IMediator _mediator;
        private readonly SignInManager<BlogUser> _signInManager;

        public PostController(IMediator mediator, SignInManager<BlogUser> signInManager)
        {
            _mediator = mediator;
            _signInManager = signInManager;
        }

        /// <summary>
        /// direct url to post, from here user can read comments
        /// </summary>
        [HttpGet("[Controller]/{value}")]
        public async Task<IActionResult> Post(string value)
        {
            uint postID;
            try
            {
                postID = System.Convert.ToUInt32(value);
            }
            catch
            {
                return View("PostNotFound");
            }


            PostEntry post = await _mediator.Send(new GetPostByIdQuery(postID));

            if (post == null)
                return View("PostNotFound");

            PostCommentVM[] coms = await _mediator.Send(new GetCommentsFromPostQuery(postID, 0, 10));
            BlogUser user = await _signInManager.UserManager.GetUserAsync(User);
            CommentSupplyVM comsSupply = new()
            {
                CommentsVMs = coms,
                IsAuthenticated = User != null && User.Identity!.IsAuthenticated,
                UserID = user != null ? user!.Id : 0,
                PostID = postID
            };

            ViewResult mainPage = View(new PostVM(post, comsSupply, user != null && post.AuthorName == user.UserName));

            return mainPage;
        }

        [HttpGet("[Controller]/getMoreComments/{reqPostID}:{reqCurrentComsCount}")]
        public async Task<IActionResult> GetMoreComments(string reqPostID, string reqCurrentComsCount)
        {
            uint postID;
            int currentCount;
            try
            {
                postID = System.Convert.ToUInt32(reqPostID);
                currentCount = System.Convert.ToInt32(reqCurrentComsCount);
            }
            catch
            {
                return BadRequest();
            }

            BlogUser user = await _signInManager.UserManager.GetUserAsync(User);

            PostCommentVM[] coms = await _mediator.Send(new GetCommentsFromPostQuery(postID, currentCount * 10, 10));

            PartialViewResult partialView = PartialView("_CommentsPartial", new CommentSupplyVM()
            {
                CommentsVMs = coms,
                IsAuthenticated = User != null && User.Identity!.IsAuthenticated,
                UserID = user != null ? user!.Id : 0,
                PostID = postID
            });

            return partialView;
        }
        [HttpGet("[Controller]/getUserPosts/{userIDString}:{cycleString}")]
        public async Task<IActionResult> GetUserPosts(string userIDString, string cycleString) 
        {
            uint userID;
            int cycle;
            try
            {
                userID = System.Convert.ToUInt32(userIDString);
                cycle = System.Convert.ToInt32(cycleString);
            }
            catch
            {
                return BadRequest();
            }

            PostEntry[] entries = await _mediator.Send(new GetUserPostsQuery(userID, cycle * 4, 4));
            return PartialView("_FeedPartial", entries);
        }


        [Authorize]
        [HttpGet("/publish")]
        public IActionResult Publish() 
        {
            return View("PostPost");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PublishPostRequest model)
        {
            if (User.Identity!.IsAuthenticated == false)
                return Json(new { success = false, errors = "You must be logged in to post" });

            if (ModelState.IsValid == false)
            {
                var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
                return Json(new { success = false, errors = errors });
            }

            Post post = await _mediator.Send(new PostCommand(User, model.Content));
            return Json(new { success = true, redirectUrl = $"/Post/{post.Id}" });
        }

        [HttpPost]
        public async Task<IActionResult> PostComment([FromBody]PublishCommentRequest model)
        {
            if (ModelState.IsValid == false)
            {
                var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
                return Json(new { success = false, errors = errors });
            }

            Comment comment = await _mediator.Send(new AddCommentCommand(User, model.postID, model.Content));

            Response.Headers.Append("Location", $"{model.postID}");

            if (comment == null)
                return RedirectToAction("Index", "Home", 302);

            return Json(new { success = true,  redirectUrl = $"/Post/{model.postID}" });
        }

        
        [HttpDelete("[Controller]/deletePost/{postIDValue}")]
        public async Task<IActionResult> DeletePost(string postIDValue)
        {
            uint postID;
            try
            {
                postID = System.Convert.ToUInt32(postIDValue);
            }
            catch
            {
                return BadRequest();
            }

            BlogUser user = await _signInManager.UserManager.GetUserAsync(User);
            bool success = await _mediator.Send(new RemovePostCommand(user!, postID));

            return Json(new { success = true, redirectUrl = $"/{user!.UserName}" });
        }

        [HttpDelete("[Controller]/deletecom/{postIDValue}:{comIDValue}")]
        public async Task<IActionResult> DeleteComment(string postIDValue, string comIDValue)
        {
            uint postID;
            uint comID;
            try
            {
                postID = System.Convert.ToUInt32(postIDValue);
                comID = System.Convert.ToUInt32(comIDValue);
            }
            catch 
            {
                return BadRequest();
            }

            await _mediator.Send(new RemoveCommentCommand(User, postID, comID));

            return Ok();
            //return RedirectToAction($"{postID}", "Post");
        }
    }
}
