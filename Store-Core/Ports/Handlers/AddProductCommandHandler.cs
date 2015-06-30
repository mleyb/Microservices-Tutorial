using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using paramore.brighter.commandprocessor.policy.Attributes;
using Store_Core.Adapters.DataAccess;
using Store_Core.Ports.Commands;
using Store_Core.ReferenceData;

namespace Store_Core.Ports.Handlers
{
    public class AddProductCommandHandler: RequestHandler<AddProductCommand>
    {
        private readonly IProductsDAO _productsDao;

        public AddProductCommandHandler(IProductsDAO productsDao,  ILog logger) : base(logger)
        {
            _productsDao = productsDao;
        }

        [RequestLogging(step: 1, timing: HandlerTiming.Before)]
        [UsePolicy(CommandProcessor.RETRYPOLICY, step: 3)]
        public override AddProductCommand Handle(AddProductCommand addProductCommand)
        {
            using (var scope = _productsDao.BeginTransaction())
            {
                ProductReference insertedProductReference = _productsDao.Add(
                    new ProductReference(
                        productId: addProductCommand.ProductId,
                        productName: addProductCommand.ProductName,
                        productDescription: addProductCommand.ProductDescription,
                        productPrice: addProductCommand.ProductPrice)
                    );

                scope.Commit();

                addProductCommand.ProductId = insertedProductReference.Id;

            }

            return base.Handle(addProductCommand);
        }
    }
}
