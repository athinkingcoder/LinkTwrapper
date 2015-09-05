namespace LinkTwrapper.Domain
{
    using System;
    using System.Collections.Generic;

    public class Tweet
    {
        public string id { get; set; }

        public string text { get; set; }

        public TweetEntities entities { get; set; }

        public bool ContainsLinks
        {
            get
            {
                return entities.ContainsLinks;
            }
        }

        public List<Uri> Links
        {
            get
            {
                return entities.Links;
            }
        }
    }
}
