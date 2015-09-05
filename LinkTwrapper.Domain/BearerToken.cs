namespace LinkTwrapper.Domain
{
    using System.Collections.Generic;
    using System.Net.Http;

    public class BearerToken
    {
        private readonly BearerTokenCredential credential;

        private string value;

        public BearerToken(BearerTokenCredential credential)
        {
            this.credential = credential;
        }

        public string Value
        {
            get
            {
                if (this.value == null)
                {
                    this.value = GetValueFromTwitter();
                }

                return this.value;
            }
        }

        private string GetValueFromTwitter()
        {
            HttpClient client = new HttpClient();
            string authorizationHeaderValue = string.Format("Basic {0}", this.credential.EncodedValue);
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

            return tokenResponse.access_token;
        }

        private class BearerTokenRepsonse
        {
            public string token_type { get; set; }

            public string access_token { get; set; }
        }
    }
}
