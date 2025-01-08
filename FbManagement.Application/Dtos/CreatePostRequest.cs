using FbManagement.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FbManagement.Application.Dtos
{
    public class CreatePostRequest
    {
        public string PageId { get; set; }
        public string Message { get; set; }
        public string Link { get; set; } // For link posts
        public PostType Type { get; set; }
        public PostStatus Status { get; set; }
        public DateTime? ScheduledPublishTime { get; set; }
        public List<MediaFile> MediaFiles { get; set; } = new(); // Uploaded files
    }
}
