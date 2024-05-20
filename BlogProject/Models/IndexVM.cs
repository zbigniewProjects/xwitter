using BlogProject.Application.Models.Post;

namespace BlogProject.MVC.ViewModels
{
    public class IndexVM
    {
        public string? Username { get; set; }
        public PostEntry[]? Posts { get; set; }
    }
}
