using System;
using Microsoft.Practices.Unity;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using Polly;
using Store_Core.Adapters.DataAccess;
using Store_Core.Adapters.Service;
using Store_Core.Ports.Commands;
using Store_Core.Ports.Handlers;
using Topshelf;

namespace Store_Service.Adapters.ServiceHost
{
    class StoreService: ServiceControl
    {
        private Consumer _consumer;

        public StoreService()
        {
            log4net.Config.XmlConfigurator.Configure();

            var logger = LogProvider.For<StoreService>();

            var container = new UnityContainer();
            container.RegisterInstance(typeof(ILog), LogProvider.For<StoreService>(), new ContainerControlledLifetimeManager());
            container.RegisterType<ProductsDAO>();
            container.RegisterType<AddProductCommandHandler>();
            container.RegisterType<ChangeProductCommandHandler>();
            container.RegisterType<RemoveProductCommandHandler>();

            var handlerFactory = new UnityHandlerFactory(container);

            var subscriberRegistry = new SubscriberRegistry
            {
                {typeof(AddProductCommand), typeof(AddProductCommandHandler)},
                {typeof(ChangeProductCommand), typeof(ChangeProductCommandHandler)},
                {typeof(RemoveProductCommand), typeof(RemoveProductCommandHandler)},
            };

            //create policies
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(new[]
                {
                    TimeSpan.FromMilliseconds(50),
                    TimeSpan.FromMilliseconds(100),
                    TimeSpan.FromMilliseconds(150)
                });

            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreaker(1, TimeSpan.FromMilliseconds(500));

            var policyRegistry = new PolicyRegistry()
            {
                {CommandProcessor.RETRYPOLICY, retryPolicy},
                {CommandProcessor.CIRCUITBREAKER, circuitBreakerPolicy}
            };

            var commandProcessor = CommandProcessorBuilder.With()
                .Handlers(new HandlerConfiguration(subscriberRegistry, handlerFactory))
                .Policies(policyRegistry)
                .Logger(logger)
                .NoTaskQueues()
                .RequestContextFactory(new InMemoryRequestContextFactory())
                .Build();

            _consumer = new Consumer(commandProcessor, logger);

        }

        public bool Start(HostControl hostControl)
        {
            _consumer.Consume(new Uri("http://localhost:3416"));
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _consumer.End().Wait();
            return true;
        }

        public void Shutdown(HostControl hostcontrol)
        {
            _consumer.End().Wait();
        }
    }
}
