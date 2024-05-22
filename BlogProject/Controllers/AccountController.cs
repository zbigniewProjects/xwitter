using BlogProject.Application.Users.Queries;
using BlogProject.Domain;
using BlogProject.MVC.Models;
using BlogProject.MVC.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<BlogUser> _signInManager;
        private readonly UserManager<BlogUser> _userManager;
        private readonly IMediator _mediatR;

        public AccountController(SignInManager<BlogUser> manager, UserManager<BlogUser> userManager, IMediator mediatR)
        {
            _signInManager = manager;
            _userManager = userManager;
            _mediatR = mediatR;
        }

        #region login section
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username!, model.Password!, model.RememberMe, false);

                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");

                ModelState.AddModelError("", "Invalid login attempt");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        #endregion
        #region register section
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterVM model)
        {
            if (!ModelState.IsValid) return View(model);

            if (User.Identity!.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }

            //check if user with desired username does exist already, if so return with proper feedback for client
            BlogUser userWithSameUsername = await _mediatR.Send(new GetUserByTagQuery(model.Username!));
            if (userWithSameUsername != null)
            {
                ModelState.AddModelError("", $"Username {model.Username} is already taken.");
                return View(model);
            }

            BlogUser user = new BlogUser
            {
                UserName = model.Username,
                Email = model.Email,
                ShortInfo = ""
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password!);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return View(model);
        }
        #endregion

        /// <summary>
        /// Draws and account page for user based on given url
        /// </summary>
        [HttpGet("/{value}")]
        public async Task<IActionResult> Account(string value) 
        {
            BlogUser user = await _mediatR.Send(new GetUserByTagQuery(value));
            BlogUser authorizedUser = await _signInManager.UserManager.GetUserAsync(User);

            if (user == null)
                return View("AccountNotFound");

            return View(new AccountPageVM { 
                IsAuthenticated = authorizedUser != null && User.Identity!.IsAuthenticated && user.Id == authorizedUser.Id, 
                ShortInfo = user.ShortInfo, 
                Username = user.UserName,
                UserId = user!.Id,
            });
        }
    }
}