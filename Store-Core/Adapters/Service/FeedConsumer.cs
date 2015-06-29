using System;
using System.Threading.Tasks;
using paramore.brighter.commandprocessor.Logging;
using Store_Core.Adapters.Atom;

namespace Store_Core.Adapters.Service
{
    public class Consumer
    {
        private readonly ILog _logger;
        private Task _controlTask;
        private bool _consumeFeed;
        private static readonly int s_delay = 5000;
        private AtomFeedGateway _atomFeedGateway;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Consumer(ILog logger)
        {
            _logger = logger;
            _atomFeedGateway = new AtomFeedGateway(logger);
        }

        public void Consume(Uri uri)
        {
            _consumeFeed = true;
            _controlTask = Task.Factory.StartNew(
                () =>
                {
                    while (_consumeFeed)
                    {
                        foreach (var entry in _atomFeedGateway.GetFeedEntries(uri))
                        {
                            
                        }
                        Task.Delay(s_delay);
                    }
                },
                TaskCreationOptions.LongRunning
            );
        }

        public Task End()
        {
            _consumeFeed = false;

            return _controlTask;
        }
    }
}
