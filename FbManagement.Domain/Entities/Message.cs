using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FbManagement.Domain.Entities
{
    public class Message
    {
        public string Id { get; set; }
        public string PageId { get; set; }
        public string Content { get; set; }
        public string RecipientId { get; set; }
        public string CreatedTime { get; set; }
    }
}
