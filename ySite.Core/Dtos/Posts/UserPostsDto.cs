using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.Core.Dtos.Post
{
    public class UserPostsDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public byte[]? Image { get; set; }
        public int CommentsCount { get; set; }
        public int LikeCount { get; set; }
        public int LoveCount { get; set; }
        public int SadCount { get; set; }
        public int AngryCount { get; set; }


        public int ReactsCount { get; set; }
    }
}
