using System.Collections.Generic;
using Store_Core.ReferenceData;

namespace Store_Core.Adapters.DataAccess
{
    public interface IProductsDAO
    {
        dynamic BeginTransaction();
        Product Add(Product newProduct);
        void Clear();
        void Delete(int productId);
        IEnumerable<Product> FindAll();
        Product FindById(int id);
        void Update(Product product);
    }
}
