namespace LinkTwrapper.Domain
{
    using System;

    public class BearerTokenCredential
    {
        private readonly string consumerKey;
        private readonly string consumerSecret;

        public BearerTokenCredential(string consumerKey, string consumerSecret)
        {
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
        }

        public string EncodedValue
        {
            get
            {
                // should RFC 1738 the key and secret individually before concatenating 
                string bearerTokenCredential = string.Format("{0}:{1}", this.consumerKey, this.consumerSecret);

                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(bearerTokenCredential);

                string encodedBearerTokenCredential = Convert.ToBase64String(bytes);

                return encodedBearerTokenCredential;
            }
        }
    }
}
