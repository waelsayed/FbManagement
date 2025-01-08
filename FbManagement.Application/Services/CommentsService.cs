using FbManagement.Application.Interfaces;
using FbManagement.Domain.Entities;

namespace FbManagement.Application.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly IFacebookApiClient _facebookApiClient;

        public CommentsService(IFacebookApiClient facebookApiClient)
        {
            _facebookApiClient = facebookApiClient;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(string pageId, string postId)
        {
            var allComments = new List<Comment>();
            string? afterCursor = null;

            do
            {
                var response = await _facebookApiClient.GetCommentsAsync(pageId, postId, afterCursor);
                if (response?.data != null)
                {
                    allComments.AddRange(response.data);
                }
                afterCursor = response?.paging?.cursors?.after;
            } while (!string.IsNullOrEmpty(afterCursor));

            return allComments;
        }

        public async Task<IEnumerable<Post>> GetPostsWithCommentsInPeriodAsync(string pageId, string since, string until, int limit = 25)
        {
            return await _facebookApiClient.GetPostsInPeriodAsync(pageId, since, until, limit);
        }

        public async Task ReplyToCommentAsync(string postId, string commentId, string message)
        {
            await _facebookApiClient.ReplyToCommentAsync(postId, commentId, message);
        }

        public async Task LikeCommentAsync(string postId, string commentId)
        {
            await _facebookApiClient.LikeCommentAsync(postId, commentId);
        }
    }
}
