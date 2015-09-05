namespace LinkTwrapper.Domain
{
    using System.Collections.Generic;
    using System.Net.Http;

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
