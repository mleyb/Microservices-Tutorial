using System;
using System.Threading.Tasks;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using Store_Core.Adapters.Atom;
using Store_Core.Ports.Commands;

namespace Store_Core.Adapters.Service
{
    public class Consumer
    {
        private readonly IAmACommandProcessor _commandProcessor;
        private readonly ILog _logger;
        private Task _controlTask;
        private bool _consumeFeed;
        private static readonly int s_delay = 5000;
        private readonly AtomFeedGateway _atomFeedGateway;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Consumer(IAmACommandProcessor commandProcessor, ILog logger)
        {
            _commandProcessor = commandProcessor;
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
                            switch (entry.Type)
                            {
                                case ProductEntryType.Created :
                                    _commandProcessor.Send(new AddProductCommand(entry.ProductName, entry.ProductDescription, entry.Price));
                                    break;
                                case ProductEntryType.Updated :
                                    _commandProcessor.Send(new ChangeProductCommand(entry.ProductId, entry.ProductName, entry.ProductDescription, entry.Price));
                                    break;
                                case ProductEntryType.Deleted :
                                    _commandProcessor.Send(new RemoveProductCommand(entry.ProductId));
                                    break;
                            }
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
