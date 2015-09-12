namespace LinkTwrapper.Domain
{
    using System;
    using System.Net.Http;

    public class TweetRequest
    {
        private readonly IBearerToken bearerToken;

        private readonly Uri userTimelineUri;

        private readonly string screenName;

        public TweetRequest(IBearerToken bearerToken, Uri userTimelineUri, string screenName)
        {
            this.bearerToken = bearerToken;
            this.userTimelineUri = userTimelineUri;
            this.screenName = screenName;
        }

        internal HttpRequestMessage HttpRequest
        {
            get
            {
                HttpRequestMessage request = CreateRequest();
                AddAuthorizationHeader(request);

                return request;
            }
        }

        private HttpRequestMessage CreateRequest()
        {
            Uri tweetsUri = GetParameterisedUri();
            return new HttpRequestMessage(HttpMethod.Get, tweetsUri);
        }

        private Uri GetParameterisedUri()
        {
            string fullUriString = 
                string.Format("{0}?screen_name={1}&count=20", this.userTimelineUri.AbsoluteUri, this.screenName);

            return new Uri(fullUriString);
        }

        private void AddAuthorizationHeader(HttpRequestMessage request)
        {
            string authorizationHeaderValue = string.Format("Bearer {0}", this.bearerToken.Value);
            request.Headers.Add("Authorization", authorizationHeaderValue);
        }
    }
}
