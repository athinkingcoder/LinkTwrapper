namespace TweetTamer.Core
{
    using System.Collections.Generic;

    public interface ITweetRetriever
    {
        IList<Tweet> Retrieve();
    }
}