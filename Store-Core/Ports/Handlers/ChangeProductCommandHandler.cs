using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using Store_Core.Adapters.DataAccess;
using Store_Core.Ports.Commands;
using Store_Core.ReferenceData;

namespace Store_Core.Ports.Handlers
{
    public class ChangeProductCommandHandler: RequestHandler<ChangeProductCommand>
    {
        private readonly IProductsDAO _productsDao;

        public ChangeProductCommandHandler(IProductsDAO productsDao, ILog logger) : base(logger)
        {
            _productsDao = productsDao;
        }

        public override ChangeProductCommand Handle(ChangeProductCommand changeProductCommand)
        {

            ProductReference productReference;
            using (var scope = _productsDao.BeginTransaction())
            {
                productReference = _productsDao.FindById(changeProductCommand.ProductId);
                productReference.ProductName = changeProductCommand.ProductName;
                productReference.ProductDescription = changeProductCommand.ProductDescription;
                productReference.ProductPrice = changeProductCommand.Price;

                _productsDao.Update(productReference);
                scope.Commit();
            }

            return base.Handle(changeProductCommand);
        }
    }
}
