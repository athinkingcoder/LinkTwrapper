namespace ThinkingCoder.LinkTwrapper.TestConsole
{
    using global::LinkTwrapper.Domain;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
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
            BearerToken token = GetTwitterBearerToken();

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

        private static BearerToken GetTwitterBearerToken()
        {
            var credential = new BearerTokenCredential(ConsumerKey, ConsumerSecret);
            return new BearerToken(credential);
        }

        private static async Task<List<Tweet>> GetTweets(BearerToken bearerToken, string screenName)
        {
            HttpClient client = new HttpClient();
            string authorizationHeaderValue = string.Format("Bearer {0}", bearerToken.Value);
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
}

