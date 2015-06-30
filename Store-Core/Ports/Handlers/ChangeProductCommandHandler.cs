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

            Product product;
            using (var scope = _productsDao.BeginTransaction())
            {
                product = _productsDao.FindById(changeProductCommand.ProductId);
                product.ProductName = changeProductCommand.ProductName;
                product.ProductDescription = changeProductCommand.ProductDescription;
                product.ProductPrice = changeProductCommand.Price;

                _productsDao.Update(product);
                scope.Commit();
            }

            return base.Handle(changeProductCommand);
        }
    }
}
