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
            var twitter = new Twitter();

            var credential = new BearerTokenCredential(ConsumerKey, ConsumerSecret);
            Twitter.IBearerToken token= twitter.RequestBearerToken(credential);

            Console.WriteLine(token);

            var tweets = twitter.GetTweets(token, "JohnRentoul");

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

