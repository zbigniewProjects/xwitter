using BlogProject.Application.Authentication.Queries;
using BlogProject.Application.DTO;
using BlogProject.Application.DTOs;
using BlogProject.Application.Models.Post;
using BlogProject.Application.Posts.Commands.AddComment;
using BlogProject.Application.Posts.Commands.AddPost;
using BlogProject.Application.Posts.Commands.RemoveComment;
using BlogProject.Application.Posts.Commands.RemovePost;
using BlogProject.Application.Posts.Queries;
using BlogProject.Application.Users.Queries;
using BlogProject.Contracts.Post;
using BlogProject.Domain;
using BlogProject.MVC.Utils;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace BlogProject.Controllers
{

    public class PostController : Controller
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// The sign-in manager will be useful here to identify callers in some requests and ensure the
        /// right people make the right requests. For example, it will prevent someone from deleting a post made by another user;
        /// a single user can only delete their own posts, not others
        /// </summary>
        private readonly SignInManager<BlogUser> _signInManager;

        public PostController(IMediator mediator, SignInManager<BlogUser> signInManager)
        {
            _mediator = mediator;
            _signInManager = signInManager;
        }

        #region posts
        /// <summary>
        /// Draws post from url, from here user can read post and comments under it
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

            
            BlogUser user = await _signInManager.UserManager.GetUserAsync(User);

            ViewResult mainPage = View(new PostVM(post, user != null && post.AuthorName == user.UserName));
            return mainPage;
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

            GetUsersPostDTO posts = await _mediator.Send(new GetUserPostsQuery(userID, cycle * 5, 5));

            if (posts.Posts == null) 
            {
                return Json(new { elementsLeft = 0 });
            }

            BlogUser currentAuthor = await _mediator.Send(new GetUserQuery(userID));

            //after posts and their autors are retrieved from db, model them for html rendering
            PostEntry[] postsVM = new PostEntry[posts.Posts!.Length];
            for (int i = 0; i < postsVM.Length; i++)
            {
                PostEntry postVM = new PostEntry();
                Post rawPost = posts.Posts[i];

                postVM.AuthorName = currentAuthor != null ? currentAuthor.UserName : "DeletedUser";
                postVM.Content = rawPost.Content;
                postVM.Date = (DateTime)rawPost.PostedAt!;
                postVM.Id = rawPost.Id;
                postVM.NumberOfComments = rawPost.Comments!.Count;
                postsVM[i] = postVM;
            }

            string postsString = await this.RenderViewToStringAsync("_FeedPartial", new PostRangeMainFeedVM
            {
                Entries = postsVM,
                EntriesLeft = posts.PostsLeft
            });

            return Json(new { html = postsString, elementsCount = posts.Posts.Length, elementsLeft = posts.PostsLeft });
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

            //insert post in database
            Post post = await _mediator.Send(new PostCommand(User, model.Content));
            return Json(new { success = true, redirectUrl = $"/Post/{post.Id}" });
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

        #endregion

        #region comments management
        [HttpPost]
        public async Task<IActionResult> PostComment([FromBody] PublishCommentRequest model)
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

            if (comment == null)
                return RedirectToAction("Index", "Home", 302);

            return Json(new { success = true, redirectUrl = $"/Post/{model.postID}" });
        }

        [HttpDelete("[Controller]/deletecom/{postIDValue}:{comIDValue}")]
        public async Task<IActionResult> DeleteComment(string postIDValue, string comIDValue)
        {
            uint postID;
            uint comID;
            try
            {
                postID = Convert.ToUInt32(postIDValue);
                comID = Convert.ToUInt32(comIDValue);
            }
            catch 
            {
                return BadRequest();
            }

            await _mediator.Send(new RemoveCommentCommand(User, postID, comID));

            return Ok();
        }


        [HttpGet("[Controller]/getMoreComments/{reqPostID}:{reqCurrentComsCount}")]
        public async Task<IActionResult> GetMoreComments(string reqPostID, string reqCurrentComsCount)
        {
            uint postID;
            int currentCount;
            try
            {
                postID = Convert.ToUInt32(reqPostID);
                currentCount = Convert.ToInt32(reqCurrentComsCount);
            }
            catch
            {
                return BadRequest();
            }

            //retrieve comments from db 
            GetCommentsFromPostDTO comsRes = await _mediator.Send(new GetCommentsFromPostQuery(postID, currentCount * 10, 10));

            //check to see if comments for specific request exist, since user can always make bad request
            if (comsRes.Comments == null) 
                return Json(new { html = string.Empty, elementsLeft = 0 });
            

            //make array of authors id's for get users query
            uint[] authorsIDs = new uint[comsRes.Comments!.Length];
            for (int i = 0; i < comsRes.Comments.Length; i++)
            {
                authorsIDs[i] = comsRes.Comments[i].AuthorID;
            }

            //after retrieving comments retrieve authors info of those comments
            BlogUser[] commentsAuthors = await _mediator.Send(new GetUsersQuery(authorsIDs));

            //map vm's for html rendering, probably should use some mapping utility here
            PostCommentVM[] postCommentVMs = new PostCommentVM[comsRes.Comments.Length];
            for (int i = 0; i < postCommentVMs.Length; i++)
            {
                PostCommentVM postCommentVM = new PostCommentVM();

                Comment rawComment = comsRes.Comments![i];

                BlogUser user = commentsAuthors.FirstOrDefault(x => x.Id == authorsIDs[i])!;
                postCommentVM.AuthorName = user != null ? user.UserName : "DeletedUser";
                postCommentVM.Content = rawComment.Content;
                postCommentVM.PostedAt = rawComment.PostedAt;
                postCommentVM.CommentID = rawComment.Id;
                postCommentVM.AuthorID = user != null ? user.Id : 0;

                postCommentVMs[i] = postCommentVM;
            }

            BlogUser authenticatedUser = await _signInManager.UserManager.GetUserAsync(User);

            CommentSupplyVM comsVM = new CommentSupplyVM
            {
                CommentsLeft = comsRes.CommentsLeft,
                IsAuthenticated = User != null && User.Identity!.IsAuthenticated,
                UserID = authenticatedUser != null ? authenticatedUser!.Id : 0,
                PostID = postID,
                CommentsVMs = postCommentVMs
            };
            string renderedContentJson = await this.RenderViewToStringAsync("_CommentsPartial", comsVM);

            return Json(new { html = renderedContentJson, elementsLeft = comsVM.CommentsLeft });
        }

        #endregion
    }
}
