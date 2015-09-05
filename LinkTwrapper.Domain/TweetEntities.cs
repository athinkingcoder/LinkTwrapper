namespace LinkTwrapper.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TweetEntities
    {
        public List<Url> urls;

        public bool ContainsLinks
        {
            get
            {
                return urls.Count > 0;
            }
        }

        public List<Uri> Links
        {
            get
            {
                var links = new List<Uri>();
                links.AddRange(
                    from url in this.urls
                    select new Uri(url.expanded_url)
                    );
                return links;
            }
        }
    }
}