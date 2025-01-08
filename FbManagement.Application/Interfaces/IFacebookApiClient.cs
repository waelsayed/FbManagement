using FbManagement.Domain.Entities;
using FbManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace FbManagement.Application.Interfaces
{
    public  interface IFacebookApiClient
    {
        // Comments
        Task<FacebookResponse<Comment>> GetCommentsAsync(string pageId,string postId, string ? afterCursor = null);
        Task ReplyToCommentAsync(string postId, string commentId, string message);
        Task LikeCommentAsync(string postId, string commentId);
        // Posts
        Task<IEnumerable<Post>> GetPostsInPeriodAsync(string pageId,string since, string until, int limit = 25);
        // we have more types of posts here (photo, video, link, etc.)
        //Task CreatePostAsync(string pageId, string message);
        // scheduled posts here
        Task<string> UploadMediaAsync(string pageId,Stream content, string fileName, string contentType);
        Task<string> CreatePostAsync(string pageId,string message, string link, List<string> mediaIds, PostType postType, PostStatus status, DateTime? scheduledTime);
      

        // Messages
        //Task<IEnumerable<Message>> GetMessagesAsync(string pageId);
        //Task SendMessageAsync(string recipientId, string message);

        // Reactions
        //Task<IEnumerable<Reaction>> GetReactionsAsync(string postId);
        //Task ReactToPostAsync(string postId, string reactionType);

        //// Page Insights
        //Task<PageInsights> GetPageInsightsAsync(string pageId);

        //// User/Page Details
        //Task<PageDetails> GetPageDetailsAsync(string pageId);
        //Task<UserProfile> GetUserProfileAsync(string userId);
    }
    
}
