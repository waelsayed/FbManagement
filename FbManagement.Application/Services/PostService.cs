using FbManagement.Application.Dtos;
using FbManagement.Application.Interfaces;
using FbManagement.Infrastructure.FacebookApi;

namespace FbManagement.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IFacebookApiClient _facebookApiClient;

        public PostService(IFacebookApiClient facebookApiClient)
        {
            _facebookApiClient = facebookApiClient;
        }

        public async Task<string> CreatePostAsync(CreatePostRequest request)
        {
            // Handle media uploads if media files exist
            var mediaIds = new List<string>();
            foreach (var mediaFile in request.MediaFiles)
            {
                var mediaId = await _facebookApiClient.UploadMediaAsync(request.PageId,mediaFile.Content, mediaFile.FileName, mediaFile.ContentType);
                mediaIds.Add(mediaId);
            }

            // Call the Facebook API to create the post
            var postId = await _facebookApiClient.CreatePostAsync(
                pageId: request.PageId,
                message: request.Message,
                link: request.Link,
                mediaIds: mediaIds,
                postType: request.Type,
                status: request.Status,
                scheduledTime: request.ScheduledPublishTime
            );

            return postId;
        }
    }
}
