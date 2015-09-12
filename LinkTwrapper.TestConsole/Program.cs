namespace ThinkingCoder.LinkTwrapper.TestConsole
{
    using global::LinkTwrapper.Domain;
    using System;

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
            Uri tokenUri = new Uri("https://api.twitter.com/oauth2/token");
            Uri userTimelineUri = new Uri("https://api.twitter.com/1.1/statuses/user_timeline.json");

            var twitter = new Twitter();

            var credential = new BearerTokenCredential(ConsumerKey, ConsumerSecret);
            var tokenRequest = new BearerTokenRequest(credential, tokenUri);

            IBearerToken token= twitter.RequestBearerToken(tokenRequest);

            Console.WriteLine(token);

            var tweetsRequest = new TweetRequest(token, userTimelineUri, "JohnRentoul");

            var tweets = twitter.GetTweets(tweetsRequest);

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
    }
}

