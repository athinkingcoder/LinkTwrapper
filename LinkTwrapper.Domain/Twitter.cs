namespace LinkTwrapper.Domain
{
    using System.Collections.Generic;
    using System.Net.Http;
    using Newtonsoft.Json;

    public class Twitter
    {
        public IBearerToken RequestBearerToken(BearerTokenCredential credential)
        {
            HttpClient client = new HttpClient();
            string authorizationHeaderValue = string.Format("Basic {0}", credential.EncodedValue);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", authorizationHeaderValue);

            HttpContent payload = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });
            payload.Headers.Clear();
            payload.Headers.Add("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8");

            BearerTokenRepsonse tokenResponse = null;
            string tokenUri = "https://api.twitter.com/oauth2/token";
            var responseTask = client.PostAsync(tokenUri, payload).ContinueWith(
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
            HttpClient client = new HttpClient();
            string authorizationHeaderValue = string.Format("Bearer {0}", bearerToken.Value);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", authorizationHeaderValue);

            //string response = null;
            string tweetsUri = string.Format("https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={0}&count=20", screenName);

            var response = client.GetAsync(tweetsUri).Result;
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
