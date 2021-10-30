namespace BasketTestLib.Models
{
    public class Product
    {
        public float UnitPrice { get; set; }

        public Product(float unitPrice)
        {
            UnitPrice = unitPrice;
        }
    }
}
