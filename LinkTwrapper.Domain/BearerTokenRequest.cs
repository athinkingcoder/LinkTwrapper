namespace LinkTwrapper.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    public class BearerTokenRequest
    {
        private readonly BearerTokenCredential credential;

        private readonly Uri tokenRequestUri;

        public BearerTokenRequest(BearerTokenCredential credential, Uri tokenRequestUri)
        {
            this.credential = credential;
            this.tokenRequestUri = tokenRequestUri;
        }

        internal HttpRequestMessage HttpRequest
        {
            get
            {
                HttpRequestMessage request = CreateRequest();
                AddAuthorizationHeader(request);
                request.Content = GetRequestBody();
                return request;
            }
        }

        private HttpRequestMessage CreateRequest()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, this.tokenRequestUri);
            return request;
        }

        private void AddAuthorizationHeader(HttpRequestMessage request)
        {
            string authorizationHeaderValue = string.Format("Basic {0}", credential.EncodedValue);
            request.Headers.Add("Authorization", authorizationHeaderValue);
        }

        private static HttpContent GetRequestBody()
        {
            HttpContent payload = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                });
            payload.Headers.Clear();
            payload.Headers.Add("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8");

            return payload;
        }
    }
}
