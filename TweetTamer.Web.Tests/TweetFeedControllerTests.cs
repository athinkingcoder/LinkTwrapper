namespace TweetTamer.Web.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Net.Http;
    using FluentAssertions;
    using Core;

    [TestClass]
    public class TweetFeedControllerTests
    {
        [TestMethod]
        public void RssGetsTweetsAndReturnsRssFeed()
        {
            var tweetRetriever = new Mock<ITweetRetriever>();
            var feedCreator = new Mock<IFeedCreator>();
            var controller = new TweetFeedController(tweetRetriever.Object, feedCreator.Object);

            var response = controller.Rss();

            tweetRetriever.Verify(x => x.Retrieve(), Times.Once);

            IList<Tweet> tweets = null;
            feedCreator.Verify(x => x.Create(tweets), Times.Once);

            string rss = response; // .Content.ReadAsStringAsync().Result;
            rss.Should().NotBeNull();
        }
    }
}
