using FbManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace FbManagement.Infrastructure.FacebookApi
{
    public  interface IFacebookApiClient
    {
        // Comments
        Task<FacebookResponse<Comment>> GetCommentsAsync(string postId, string ? afterCursor = null);
        Task ReplyToCommentAsync(string commentId, string message);
        Task LikeCommentAsync(string commentId);
        // Posts
        Task<IEnumerable<Post>> GetPostsInPeriodAsync(string since, string until, int limit = 25);
        Task CreatePostAsync(string pageId, string message);
        // scheduled posts here

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
