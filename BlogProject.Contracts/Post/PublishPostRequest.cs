
using BlogProject.Contracts.Utils;
using System.ComponentModel.DataAnnotations;

namespace BlogProject.Contracts.Post
{

    public record PublishPostRequest(
        [Required(ErrorMessage = "Please enter message")]
        [MaxWordLength(30, ErrorMessage = "Words cannot exceed 30 characters.")]
        [MaxLength(600, ErrorMessage = "Post must not have more than 600 characters in total")]
        string Content
        );
}
