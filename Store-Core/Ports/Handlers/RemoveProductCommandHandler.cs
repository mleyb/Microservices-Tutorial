using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using Store_Core.Adapters.DataAccess;
using Store_Core.Ports.Commands;
using Store_Core.ReferenceData;

namespace Store_Core.Ports.Handlers
{
    public class RemoveProductCommandHandler : RequestHandler<RemoveProductCommand>
    {
        private readonly IProductsDAO _productsDao;
        private readonly IAmACommandProcessor _commandProcessor;

        public RemoveProductCommandHandler(IProductsDAO productsDao, IAmACommandProcessor commandProcessor, ILog logger) : base(logger)
        {
            _productsDao = productsDao;
            _commandProcessor = commandProcessor;
        }

        public override RemoveProductCommand Handle(RemoveProductCommand removeProductCommand)
        {
            ProductReference productReference;
            using (var scope = _productsDao.BeginTransaction())
            {
                productReference = _productsDao.FindById(removeProductCommand.ProductId);
                _productsDao.Delete(removeProductCommand.ProductId);

                scope.Commit();
            }

            return base.Handle(removeProductCommand);
        }
    }
}
