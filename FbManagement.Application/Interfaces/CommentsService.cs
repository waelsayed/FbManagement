using FbManagement.Domain.Entities;
using FbManagement.Infrastructure.FacebookApi;

namespace FbManagement.Application.Interfaces
{
    public class CommentsService : ICommentsService
    {
        private readonly IFacebookApiClient _facebookApiClient;

        public CommentsService(IFacebookApiClient facebookApiClient)
        {
            _facebookApiClient = facebookApiClient;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(string postId)
        {
            var allComments = new List<Comment>();
            string? afterCursor = null;

            do
            {
                var response = await _facebookApiClient.GetCommentsAsync(postId, afterCursor);
                if (response?.Data != null)
                {
                    allComments.AddRange(response.Data);
                }
                afterCursor = response?.Paging?.Cursors?.After;
            } while (!string.IsNullOrEmpty(afterCursor));

            return allComments;
        }

        public async Task<IEnumerable<Post>> GetPostsWithCommentsInPeriodAsync(string since, string until, int limit = 25)
        {
            return await _facebookApiClient.GetPostsInPeriodAsync(since, until, limit);
        }

        public async Task ReplyToCommentAsync(string commentId, string message)
        {
            await _facebookApiClient.ReplyToCommentAsync(commentId, message);
        }

        public async Task LikeCommentAsync(string commentId)
        {
            await _facebookApiClient.LikeCommentAsync(commentId);
        }
    }
}
