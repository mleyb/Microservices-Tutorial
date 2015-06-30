using System.Collections.Generic;
using Store_Core.ReferenceData;

namespace Store_Core.Adapters.DataAccess
{
    public interface IProductsDAO
    {
        dynamic BeginTransaction();
        ProductReference Add(ProductReference newProductReference);
        void Clear();
        void Delete(int productId);
        IEnumerable<ProductReference> FindAll();
        ProductReference FindById(int id);
        void Update(ProductReference productReference);
    }
}
