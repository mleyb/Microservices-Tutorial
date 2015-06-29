using System;
using System.IO;
using Grean.AtomEventStore;
using Microsoft.Practices.Unity;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using Products_Core.Adapters.Atom;
using Products_Core.Ports.Events;
using Product_Service;

namespace Products_Core.Ports.Handlers
{
    public class ProductAddedEventHandler : RequestHandler<ProductAddedEvent>
    {
        private readonly IObserver<ProductEntry> _observer;

        //Allows injection of observer for tests
        public ProductAddedEventHandler(IObserver<ProductEntry> observer, ILog logger) : base(logger)
        {
            _observer = observer;
        }

        [InjectionConstructor]
        public ProductAddedEventHandler(ILog logger) : base(logger)
        {
            var storage = new AtomEventsInFiles(new DirectoryInfo(Globals.StoragePath));
            var serializer = new DataContractContentSerializer(
                DataContractContentSerializer
                    .CreateTypeResolver(typeof (ProductEntry).Assembly)
                );

            _observer= new AtomEventObserver<ProductEntry>(
                Globals.EventStreamId,
                25,
                storage,
                serializer
                );
        }

        public override ProductAddedEvent Handle(ProductAddedEvent productAddedEvent)
        {
            _observer.OnNext(new ProductEntry(
                type: ProductEntryType.Created,
                productId: productAddedEvent.ProductId,
                productDescription: productAddedEvent.ProductDescription, 
                productName: productAddedEvent.ProductName, 
                productPrice: productAddedEvent.ProductPrice));

            return base.Handle(productAddedEvent);
        }
    }
}
