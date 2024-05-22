using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.Models.Post
{
    public class PostRangeMainFeedVM
    {
        public PostEntry[]? Entries { get; set; }
        public int EntriesLeft { get; set; }
    }
}
