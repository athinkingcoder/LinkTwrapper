namespace ThinkingCoder.LinkTwrapper.TestConsole
{
    using global::LinkTwrapper.Domain;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class Program
    {
        private static string ConsumerKey
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ConsumerKey"];
            }
        }

        private static string ConsumerSecret
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ConsumerSecret"];
            }
        }

        public static void Main(string[] args)
        {
            string token = GetTwitterBearerToken();

            Console.WriteLine(token);

            var tweets = GetTweets(token, "JohnRentoul").Result;

            Console.WriteLine();
            Console.WriteLine("--------");
            Console.WriteLine();

           // Console.WriteLine(tweets);

            foreach (var tweet in tweets)
            {
               // Console.WriteLine(tweet.id + ": " + tweet.text);

                if (tweet.ContainsLinks)
                {
                    foreach (var link in tweet.Links)
                    {
                        Console.WriteLine(link.AbsoluteUri);
                    }
                }
            }

            Console.ReadLine();
        }

        private static string GetTwitterBearerToken()
        {
            var bearerTokenCredential = GetBearerTokenCredential();

            HttpClient client = new HttpClient();
            string authorizationHeaderValue = string.Format("Basic {0}", bearerTokenCredential.EncodedValue);
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

        private static BearerTokenCredential GetBearerTokenCredential()
        {
            return new BearerTokenCredential(ConsumerKey, ConsumerSecret);
        }

        private static async Task<List<Tweet>> GetTweets(string bearerTokenValue, string screenName)
        {
            HttpClient client = new HttpClient();
            string authorizationHeaderValue = string.Format("Bearer {0}", bearerTokenValue);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", authorizationHeaderValue);

            //string response = null;
            string tweetsUri = string.Format("https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={0}&count=20", screenName);

            var response = await client.GetAsync(tweetsUri);
            var json = response.Content.ReadAsStringAsync().Result;
            var tweets = JsonConvert.DeserializeObject<List<Tweet>>(json);

            //return response.Content.ReadAsAsync<List<Tweet>>().Result;
            return tweets;
        }
    }

    class BearerTokenRepsonse
    {
        public string token_type { get; set; }

        public string access_token { get; set; }
    }
}

