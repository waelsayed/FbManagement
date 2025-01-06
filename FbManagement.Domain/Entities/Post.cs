using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FbManagement.Domain.Entities
{
    public class Post
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string CreatedTime { get; set; }
    }
}
