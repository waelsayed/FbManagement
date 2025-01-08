using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FbManagement.Domain.Enums
{
    public enum PostType
    {
        Text,
        Image,
        Video,
        Link,
        MixedMedia
    }

    public enum PostStatus
    {
        Immediate,
        Scheduled
    }
}
