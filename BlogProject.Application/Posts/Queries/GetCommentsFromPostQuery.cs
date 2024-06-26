﻿using BlogProject.Application.DTO;
using BlogProject.Application.Models.Post;
using BlogProject.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.Posts.Queries
{
    public record GetCommentsFromPostQuery (uint postID, int startIndex, int count) : IRequest<GetCommentsFromPostDTO>;
}
