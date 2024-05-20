using BlogProject.Contracts.Utils;
using System.ComponentModel.DataAnnotations;
namespace BlogProject.Contracts.Post
{
    public record PublishCommentRequest(
        uint postID,
        [Required(ErrorMessage = "Please enter your comment")]
        [MaxWordLength(30, ErrorMessage = "Words cannot exceed 30 characters.")]
        [MaxLength(600, ErrorMessage = "Comment must not have more than 600 characters in total")]
        string Content
        );
}
