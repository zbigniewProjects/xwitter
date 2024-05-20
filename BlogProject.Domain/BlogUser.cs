using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlogProject.Domain
{
    public class BlogUser : IdentityUser<uint>
    {
        [StringLength(50)]
        [MaxLength(50)]
        [Required]
        public string ? ShortInfo { get; set; }
        DateTime? createdAt { get; set; }
    }
}