using System;
using System.Collections.Generic;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using paramore.brighter.commandprocessor.Logging;

namespace Store_Core.Adapters.Atom
{
        public class AtomFeedGateway
        {
        private ThreadLocal<HttpClient> _client;
        private readonly double _timeout;

        public readonly ILog Logger;

        public AtomFeedGateway(ILog logger)
        {
            Logger = logger;
            _timeout = 5000;
        }


       public IEnumerable<ProductEntry> GetFeedEntries(Uri uri)
       {
            try
            {
                var response = Client().GetAsync(uri).Result;
                response.EnsureSuccessStatusCode();
                var reader = new ReferenceDataFeedReader<ProductEntry>(response.Content.ReadAsStringAsync().Result);
                return reader;
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.Flatten().InnerExceptions)
                {
                    Logger.ErrorFormat("Threw exception getting feed from the Server {0}", uri, exception.Message);
                }

                throw new ApplicationException(string.Format("Error retrieving the feed from the server, see log for details"));
            }
       }


        public HttpClient Client()
        {
            _client = new ThreadLocal<HttpClient>(() => CreateClient(_timeout));
            return _client.Value;
        }

  

        private HttpClient CreateClient(double timeout)
        {
            var requestHandler = new WebRequestHandler
            {
                AllowPipelining = true,
                AllowAutoRedirect = true,
                CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Revalidate)
            };
            var client = HttpClientFactory.Create(requestHandler);
            client.Timeout = TimeSpan.FromMilliseconds(timeout);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            return client;
        }
    }
}
