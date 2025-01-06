using FbManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FbManagement.Infrastructure.FacebookApi
{
    public class FacebookApiClient : IFacebookApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _accessToken;
        private readonly string _apiVersion;
        public FacebookApiClient(HttpClient httpClient, string accessToken, string apiVersion)
        {
            _httpClient = httpClient;
            _accessToken = accessToken;
            _apiVersion = apiVersion;
        }

        #region Comment management
        public async Task<FacebookResponse<Comment>> GetCommentsAsync(string postId, string ? afterCursor = null)
        {
            var endpoint = $"https://graph.facebook.com/{_apiVersion}/{postId}/comments";
            var query = $"?access_token={_accessToken}" +
                        (afterCursor != null ? $"&after={afterCursor}" : "");

            var response = await _httpClient.GetAsync(endpoint + query);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<FacebookResponse<Comment>>(content);
        }
       
        public async Task ReplyToCommentAsync(string commentId, string message)
        {
            var endpoint = $"https://graph.facebook.com/{_apiVersion}/{commentId}/comments";
            var payload = new { message };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint + $"?access_token={_accessToken}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task LikeCommentAsync(string commentId)
        {
            var endpoint = $"https://graph.facebook.com/{_apiVersion}/{commentId}/likes";
            var response = await _httpClient.PostAsync(endpoint + $"?access_token={_accessToken}", null);
            response.EnsureSuccessStatusCode();
        }
        #endregion

        #region Post management
        public async Task<IEnumerable<Post>> GetPostsInPeriodAsync(string since, string until, int limit = 25)
        {
            var endpoint = $"https://graph.facebook.com/{_apiVersion}/me/posts";
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

                if (result?.Data != null)
                {
                    posts.AddRange(result.Data);
                }

                nextPage = result?.Paging?.Next;
            }

            return posts.OrderByDescending(p => p.CreatedTime);
        }



        public async Task CreatePostAsync(string pageId, string message)
        {
            var endpoint = $"https://graph.facebook.com/{_apiVersion}/{pageId}/feed";
            var payload = new { message };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{endpoint}?access_token={_accessToken}", content);
            response.EnsureSuccessStatusCode();
        }
        #endregion
    }
}
