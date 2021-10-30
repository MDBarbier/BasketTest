namespace BasketTestLib.Models
{
    public class Product
    {
        public decimal UnitPrice { get; set; }

        public Product(decimal unitPrice)
        {
            UnitPrice = unitPrice;
        }
    }
}
