using System;
using System.IO;
using System.Reflection;
using Microsoft.Practices.Unity;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using paramore.brighter.commandprocessor.messagestore.mssql;
using paramore.brighter.commandprocessor.messaginggateway.rmq;
using Polly;
using Store_API.Adapters.Configuration;
using Store_API.Adapters.Controllers;
using Store_Core.Adapters.DataAccess;
using Store_Core.Ports.Commands;
using Store_Core.Ports.Handlers;
using Store_Core.Ports.Mappers;

namespace Store_API.Adapters.Service
{
    internal static class IoCConfiguration
    {
        public static void Run(UnityContainer container)
        {
            container.RegisterType<StoreFrontController>();
            container.RegisterInstance(typeof(ILog), LogProvider.For<StoreService>(), new ContainerControlledLifetimeManager());
            container.RegisterType<AddOrderCommandHandler>();
            container.RegisterType<AddProductCommandHandler>();
            container.RegisterType<ChangeProductCommandHandler>();
            container.RegisterType<RemoveProductCommandHandler>();
            container.RegisterType<IProductsDAO, ProductsDAO>();

            var logger = container.Resolve<ILog>();
            var handlerFactory = new UnityHandlerFactory(container);

            var subscriberRegistry = new SubscriberRegistry();
            subscriberRegistry.Register<AddOrderCommand, AddOrderCommandHandler>();
            subscriberRegistry.Register<AddProductCommand, AddProductCommandHandler>();
            subscriberRegistry.Register<ChangeProductCommand, ChangeProductCommandHandler>();
            subscriberRegistry.Register<RemoveProductCommand, RemoveProductCommandHandler>();

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
            messageMapperRegistry.Register<AddOrderCommand, AddOrderCommandMessageMapper>();

            var gateway = new RmqMessageProducer(container.Resolve<ILog>());
            var dbPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8)), "App_Data\\MessageStore.sdf");

            IAmAMessageStore<Message> sqlMessageStore = new MsSqlMessageStore(new MsSqlMessageStoreConfiguration("DataSource=\"" + dbPath + "\"", "Messages", MsSqlMessageStoreConfiguration.DatabaseType.SqlCe), logger);

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
