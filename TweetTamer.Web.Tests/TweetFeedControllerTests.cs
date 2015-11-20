namespace TweetTamer.Web.Tests
{
    using System.Collections.Generic;
    using System.ServiceModel.Syndication;
    using System.Text;
    using System.Xml;

    using Core;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;

    [TestClass]
    public class TweetFeedControllerTests
    {
        private const string ExpectedRssText = @"<?xml version=""1.0"" encoding=""utf-16""?><rss xmlns:a10=""http://www.w3.org/2005/Atom"" version=""2.0""><channel><title /><description /><item><link>http://localhost/</link><title>title</title><description>content</description></item></channel></rss>";

        [TestMethod]
        public void RssGetsTweetsAndReturnsRssFeed()
        {
            var tweetRetriever = new Mock<ITweetRetriever>();
            var feedCreator = new Mock<IFeedCreator>();

            var tweets = new List<Tweet>() { new Tweet(), new Tweet() };
            tweetRetriever.Setup(x => x.Retrieve()).Returns(tweets);
            SyndicationFeed feed = CreateFeed();
            feedCreator.Setup(x => x.Create(tweets)).Returns(feed);

            var controller = new TweetFeedController(tweetRetriever.Object, feedCreator.Object);
            string rss = controller.Rss();

            tweetRetriever.Verify(x => x.Retrieve(), Times.Once);
            feedCreator.Verify(x => x.Create(tweets), Times.Once);
            rss.Should().Be(ExpectedRssText);
        }

        private static SyndicationFeed CreateFeed()
        {
            var feed = new SyndicationFeed();
            List<SyndicationItem> items = new List<SyndicationItem>();
            items.Add(new SyndicationItem("title", "content", new Uri("http://localhost")));

            feed.Items = items;
            return feed;
        }
    }
}
