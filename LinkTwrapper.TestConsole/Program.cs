namespace ThinkingCoder.LinkTwrapper.TestConsole
{
    using global::LinkTwrapper.Domain;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.ServiceModel.Syndication;
    using System.Text;
    using System.Xml;

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

            string screenName = "JohnRentoul";
            var tweetsRequest = new TweetRequest(token, userTimelineUri, screenName);

            var tweets = twitter.GetTweets(tweetsRequest);

            Console.WriteLine();
            Console.WriteLine("--------");
            Console.WriteLine();

            List<SyndicationItem> items = new List<SyndicationItem>();
            int counter = 0;
            foreach (var tweet in tweets)
            {
                if (tweet.ContainsLinks)
                {
                    counter++;
                    foreach (var link in tweet.Links)
                    {
                        Console.WriteLine(link.AbsoluteUri);
                    }

                    StringBuilder content = new StringBuilder();
                    content.AppendLine(tweet.text);
                    foreach(var link in tweet.Links)
                    {
                        string linkTag = string.Format(@"<a href='{0}'>{0}</>", link.AbsoluteUri);
                        content.AppendLine(linkTag);
                    }
                    SyndicationItem item = new SyndicationItem(screenName + counter, content.ToString(), new Uri("http://localhost"));
                    items.Add(item);
                }
            }

            var syndicationFeed = new SyndicationFeed("LinkTwrapper", "Tweets containing links", new Uri("http://localhost"))
                { Items = items };
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_hhmmssfff");
            string filePath = string.Format(@"C:\WS\LinkTwrapper\RSSFiles\Tweets{0}.rss", timestamp);
            using (var fileStream = File.Create(filePath))
            {
                using (var xmlWriter = XmlWriter.Create(fileStream))
                {
                    syndicationFeed.SaveAsRss20(xmlWriter);
                    xmlWriter.Flush();
                }
            }

            Console.ReadLine();
        }
    }
}

