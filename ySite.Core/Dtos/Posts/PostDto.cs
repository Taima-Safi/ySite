using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.Core.Dtos.Post
{
    public class PostDto
    {
        public string? Description { get; set; }
       // public string? FileName { get; set; }
        public IFormFile? ClientFile { get; set; }
    }
}
