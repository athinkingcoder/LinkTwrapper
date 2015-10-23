namespace TweetTamer.Web
{
    using System.Collections.Generic;
    using System.ServiceModel.Syndication;
    using TweetTamer.Core;

    public interface IFeedCreator
    {
        SyndicationFeed Create(IList<Tweet> tweets);
    }
}