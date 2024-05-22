using BlogProject.Application.Models.Post;
using BlogProject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.DTO
{
    public class GetCommentsFromPostDTO
    {
        public Comment[]? Comments { get; set; }
        public int CommentsLeft { get; set; }
    }
}
