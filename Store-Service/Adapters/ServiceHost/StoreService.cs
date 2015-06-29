using System;
using Microsoft.Practices.Unity;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using Polly;
using Store_Core.Adapters.DataAccess;
using Topshelf;

namespace Store_Service.Adapters.ServiceHost
{
    class StoreService: ServiceControl
    {

        public StoreService()
        {
            log4net.Config.XmlConfigurator.Configure();

            var logger = LogProvider.For<StoreService>();

            var container = new UnityContainer();
            container.RegisterInstance(typeof(ILog), LogProvider.For<StoreService>(), new ContainerControlledLifetimeManager());
            //container.RegisterType<AddOrderCommandMessageMapper>();
            container.RegisterType<ProductsDAO>();

            var handlerFactory = new UnityHandlerFactory(container);

            var subscriberRegistry = new SubscriberRegistry();
            //subscriberRegistry.Register<AddOrderCommand, AddOrderCommandHandler>();

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


        }

        public bool Start(HostControl hostControl)
        {
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            return true;
        }

        public void Shutdown(HostControl hostcontrol)
        {
        }
    }
}
