using BlogProject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.DTOs
{
    public class GetUsersPostDTO
    {
        public Post[]? Posts { get; set; }
        public int PostsLeft;
    }
}
