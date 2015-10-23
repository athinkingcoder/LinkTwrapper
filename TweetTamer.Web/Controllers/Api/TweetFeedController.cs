using System;
using System.Net.Http;
using TweetTamer.Core;

namespace TweetTamer.Web
{
    public class TweetFeedController
    {
        private readonly ITweetRetriever tweetRetriever;
        private readonly IFeedCreator feedCreator;

        public TweetFeedController(ITweetRetriever tweetRetriever, IFeedCreator feedCreator)
        {
            this.tweetRetriever = tweetRetriever;
            this.feedCreator = feedCreator;
        }

        public string Rss()
        {
            var tweets = this.tweetRetriever.Retrieve();
            var feed = this.feedCreator.Create(tweets);

            return string.Empty;
        }
    }
}