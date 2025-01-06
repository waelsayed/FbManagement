using FbManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FbManagement.Application.Interfaces
{
    public interface ICommentsService
    {
        Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(string postId);
        Task<IEnumerable<Post>> GetPostsWithCommentsInPeriodAsync(string since, string until, int limit = 25);
    }
}
