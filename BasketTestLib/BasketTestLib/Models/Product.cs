using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
