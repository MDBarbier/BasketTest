using BasketTestLib.Models;
using System.Collections.Generic;

namespace BasketTestLib.Interfaces
{
    public interface IVoucher
    {
        public float DiscountAmount { get; }        
        public string VoucherCode { get; set; }

        bool CheckValidity(List<Product> basketContents, out string message);
    }
}
