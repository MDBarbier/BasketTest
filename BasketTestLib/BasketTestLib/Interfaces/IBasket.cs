using BasketTestLib.Models;
using System;
using System.Collections.Generic;

namespace BasketTestLib.Interfaces
{
    public interface IBasket
    {
        public List<Product> BasketContents { get; set; }
        public decimal BasketNetTotal { get; set; }
        public decimal BasketDiscount { get; set; }
        public Guid BasketGuid { get; set; }


        void AddProduct(Product product);

        decimal GetBasketFinalValue();

        decimal GetNonVoucherNetTotal();

        bool ApplyVoucher(IVoucher voucher, out string message);

        decimal GetTotalValueForType(Type applicableType);
    }
}
