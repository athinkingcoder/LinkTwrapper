namespace LinkTwrapper.Domain
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Net.Http;

    public class Twitter
    {
        private readonly HttpClient httpClient;

        public Twitter()
        {
            this.httpClient = CreateHttpClient();
        }

        private HttpClient CreateHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();

            return client;
        }

        public IBearerToken RequestBearerToken(BearerTokenRequest request)
        {
            var response = this.httpClient.SendAsync(request.HttpRequest).Result;
            var tokenResponse = response.Content.ReadAsAsync<BearerTokenRepsonse>().Result;

            return new BearerToken(tokenResponse.access_token);
        }

        public List<Tweet> GetTweets(TweetRequest request)
        {
            var response = this.httpClient.SendAsync(request.HttpRequest).Result;
            var json = response.Content.ReadAsStringAsync().Result;
            var tweets = JsonConvert.DeserializeObject<List<Tweet>>(json);

            return tweets;
        }

        private class BearerToken : IBearerToken
        {
            public BearerToken(string value)
            {
                this.Value = value;
            }

            public string Value
            {
                get;
            }
        }

        private class BearerTokenRepsonse
        {
            public string token_type { get; set; }

            public string access_token { get; set; }
        }
    }
}
