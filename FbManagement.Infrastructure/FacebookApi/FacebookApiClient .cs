using FbManagement.Domain.Entities;
using FbManagement.Domain.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FbManagement.Application.Interfaces;

namespace FbManagement.Infrastructure.FacebookApi
{
    public class FacebookApiClient : IFacebookApiClient
    {
       
        private readonly HttpClient _httpClient;
        private readonly string _accessToken;
        private readonly string _apiVersion;

        public FacebookApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _accessToken = configuration["FacebookApi:AccessToken"];
            _apiVersion = configuration["FacebookApi:ApiVersion"];
        }

        #region Comment management
        public async Task<FacebookResponse<Comment>> GetCommentsAsync(string pageId,string postId, string ? afterCursor = null)
        {
            var endpoint = $"{_apiVersion}/{pageId}_{postId}/comments";
            var query = $"?access_token={_accessToken}" +
                        (afterCursor != null ? $"&after={afterCursor}" : "");

            var response = await _httpClient.GetAsync(endpoint + query);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<FacebookResponse<Comment>>(content);
        }
       
        public async Task ReplyToCommentAsync(string postId,string commentId, string message)
        {
            //var endpoint = $"{_apiVersion}/{commentId}/comments";
            var endpoint = $"{_apiVersion}/{postId}_{commentId}/comments";
            var payload = new { message };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint + $"?access_token={_accessToken}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task LikeCommentAsync(string postId, string commentId)
        {
            var endpoint = $"{_apiVersion}/{postId}_{commentId}/likes";
            var response = await _httpClient.PostAsync(endpoint + $"?access_token={_accessToken}", null);
            response.EnsureSuccessStatusCode();
        }
        #endregion

        #region Post management
        public async Task<IEnumerable<Post>> GetPostsInPeriodAsync(string pageId,string since, string until, int limit = 25)
        {
            var endpoint = $"{_apiVersion}/{pageId}/posts";
            var query = $"?access_token={_accessToken}&limit={limit}";

            if (!string.IsNullOrEmpty(since))
                query += $"&since={since}";
            if (!string.IsNullOrEmpty(until))
                query += $"&until={until}";

            var posts = new List<Post>();
            string nextPage = endpoint + query;

            while (!string.IsNullOrEmpty(nextPage))
            {
                var response = await _httpClient.GetAsync(nextPage);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<FacebookResponse<Post>>(content);

                if (result?.data != null)
                {
                    posts.AddRange(result.data);
                }

                nextPage = result?.paging?.Next;
            }

            return posts.OrderByDescending(p => p.CreatedTime);
        }

        public async Task<string> UploadMediaAsync(string pageId,Stream content, string fileName, string contentType)
        {
            var endpoint = $"{_apiVersion}/{pageId}/media";
            var formData = new MultipartFormDataContent();

            var streamContent = new StreamContent(content);
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            formData.Add(streamContent, "file", fileName);
            formData.Add(new StringContent(_accessToken), "access_token");

            var response = await _httpClient.PostAsync(endpoint, formData);
            response.EnsureSuccessStatusCode();

            var contentResult = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<dynamic>(contentResult);
            return result.id;
        }
        public async Task<string> CreatePostAsync(string pageId, string message, string link, List<string> mediaIds, PostType postType, PostStatus status, DateTime? scheduledTime)
        {
            var endpoint = $"{_apiVersion}/{pageId}/feed";

            var payload = new
            {
                message,
                link,
                attached_media = mediaIds?.Select(id => new { media_fbid = id }).ToList(),
                publish_status = status.ToString().ToLowerInvariant(),
                scheduled_publish_time = status == PostStatus.Scheduled ? scheduledTime?.ToUniversalTime().ToString("o") : null,
                access_token = _accessToken
            };

            var response = await _httpClient.PostAsync(endpoint, new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var contentResult = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<dynamic>(contentResult);
            return result.id;
        }


        //public async Task CreatePostAsync(string pageId, string message)
        //{
        //    var endpoint = $"{_apiVersion}/{pageId}/feed";
        //    var payload = new { message };
        //    var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        //    var response = await _httpClient.PostAsync($"{endpoint}?access_token={_accessToken}", content);
        //    response.EnsureSuccessStatusCode();
        //}
        #endregion
    }
}
