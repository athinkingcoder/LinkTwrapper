namespace TweetTamer.Web
{
    using System.ServiceModel.Syndication;
    using System.Text;
    using System.Xml;
    using Core;
    using System.Web.Http;

    public class TweetFeedController : ApiController
    {
        private readonly ITweetRetriever tweetRetriever;
        private readonly IFeedCreator feedCreator;

        public TweetFeedController(ITweetRetriever tweetRetriever, IFeedCreator feedCreator)
        {
            this.tweetRetriever = tweetRetriever;
            this.feedCreator = feedCreator;
        }

        [HttpGet]
        public string Rss()
        {
            var tweets = this.tweetRetriever.Retrieve();
            var feed = this.feedCreator.Create(tweets);

            return WriteFeedToRssString(feed);
        }
        
        private static string WriteFeedToRssString(SyndicationFeed feed)
        {
            StringBuilder stringBuilder = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(stringBuilder);
            feed.SaveAsRss20(xmlWriter);
            xmlWriter.Flush();
            return stringBuilder.ToString();
        }
    }
}