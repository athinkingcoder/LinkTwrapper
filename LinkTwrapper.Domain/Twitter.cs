namespace LinkTwrapper.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Newtonsoft.Json;

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

        public IBearerToken RequestBearerToken(BearerTokenCredential credential)
        {
            string authorizationHeaderValue = string.Format("Basic {0}", credential.EncodedValue);
            string tokenUri = "https://api.twitter.com/oauth2/token";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, tokenUri);
            request.Headers.Add("Authorization", authorizationHeaderValue);

            HttpContent payload = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });
            payload.Headers.Clear();
            payload.Headers.Add("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8");

            request.Content = payload;

            BearerTokenRepsonse tokenResponse = null;
            var responseTask = this.httpClient.SendAsync(request).ContinueWith(
                (completedTask) =>
                {
                    var response = completedTask.Result;
                    var json = response.Content.ReadAsAsync<BearerTokenRepsonse>();
                    json.Wait();
                    tokenResponse = json.Result;
                });

            responseTask.Wait();

            return new BearerToken(tokenResponse.access_token);
        }

        public List<Tweet> GetTweets(IBearerToken bearerToken, string screenName)
        {
            string authorizationHeaderValue = string.Format("Bearer {0}", bearerToken.Value);
            string tweetsUri = string.Format("https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={0}&count=20", screenName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, tweetsUri);
            request.Headers.Add("Authorization", authorizationHeaderValue);
            

            var response = this.httpClient.SendAsync(request).Result;
            var json = response.Content.ReadAsStringAsync().Result;
            var tweets = JsonConvert.DeserializeObject<List<Tweet>>(json);

            //return response.Content.ReadAsAsync<List<Tweet>>().Result;
            return tweets;
        }

        public interface IBearerToken
        {
            string Value { get; }
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
