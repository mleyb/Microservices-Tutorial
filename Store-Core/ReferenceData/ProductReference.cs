namespace Store_Core.ReferenceData
{
    public class ProductReference
    {

        public ProductReference() { /*Required by Simple.Data*/}

        public ProductReference(int productId, string productName, string productDescription, double productPrice)
        {
            ProductId = productId;
            ProductName = productName;
            ProductDescription = productDescription;
            ProductPrice = productPrice;
        }

        public string ProductDescription { get; set; }
        //Identity of the reference data row
        public int Id { get; set; }
        //Shared idenity of the product 
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
    }
}
