namespace FbManagement.Infrastructure.FacebookApi
{
    public class FacebookApiClientHandler : DelegatingHandler
    {
        private readonly string _accessToken;
        private readonly string _apiVersion;

        public FacebookApiClientHandler(string accessToken, string apiVersion)
        {
            _accessToken = accessToken;
            _apiVersion = apiVersion;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Append access token and API version to query parameters
            var uriBuilder = new UriBuilder(request.RequestUri);
            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            query["access_token"] = _accessToken;
            query["v"] = _apiVersion; // Optional depending on Facebook's API requirements
            uriBuilder.Query = query.ToString();

            request.RequestUri = uriBuilder.Uri;

            // Call the base handler
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
