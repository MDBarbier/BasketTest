namespace BasketTestLib.Models
{
    public abstract class Product
    {
        public decimal UnitPrice { get; set; }

        public Product(decimal unitPrice)
        {
            UnitPrice = unitPrice;
        }
    }
}
