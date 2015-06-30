using System;
using Microsoft.Practices.Unity;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using paramore.brighter.commandprocessor.messagestore.mssql;
using paramore.brighter.commandprocessor.messaginggateway.rmq;
using Polly;
using Store_API.Adapters.Configuration;

namespace Store_API.Adapters.Service
{
    internal static class IoCConfiguration
    {
        public static void Run(UnityContainer container)
        {
            //container.RegisterType<OrdersController>();
            container.RegisterInstance(typeof(ILog), LogProvider.For<StoreService>(), new ContainerControlledLifetimeManager());
            //container.RegisterType<OrdersDAO>();

            var logger = container.Resolve<ILog>();
            var handlerFactory = new UnityHandlerFactory(container);

            var subscriberRegistry = new SubscriberRegistry();
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

            var messageMapperFactory = new UnityMessageMapperFactory(container);
            var messageMapperRegistry = new MessageMapperRegistry(messageMapperFactory);

            var gateway = new RmqMessageProducer(container.Resolve<ILog>());
            const string MessageDbPath = "~\\App_Data\\MessageStore.sdf";
            IAmAMessageStore<Message> sqlMessageStore = new MsSqlMessageStore(new MsSqlMessageStoreConfiguration("DataSource=\"" + MessageDbPath + "\"", "Messages", MsSqlMessageStoreConfiguration.DatabaseType.SqlCe), logger);

            var commandProcessor = CommandProcessorBuilder.With()
                    .Handlers(new HandlerConfiguration(subscriberRegistry, handlerFactory))
                    .Policies(policyRegistry)
                    .Logger(logger)
                    .TaskQueues(new MessagingConfiguration(sqlMessageStore, gateway, messageMapperRegistry))
                    .RequestContextFactory(new InMemoryRequestContextFactory())
                    .Build();

            container.RegisterInstance(typeof(IAmACommandProcessor), commandProcessor);
        }
    }
}
