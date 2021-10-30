using BasketTestLib.Models;
using System;
using System.Collections.Generic;

namespace BasketTestLib.Interfaces
{
    public interface IBasket
    {
        public List<Product> BasketContents { get; set; }
        public float BasketNetTotal { get; set; }
        public float BasketDiscount { get; set; }
        public Guid BasketGuid { get; set; }


        void AddProduct(Product product);

        float GetBasketFinalValue();

        float GetNonVoucherNetTotal();

        bool ApplyVoucher(IVoucher voucher, out string message);

        float GetTotalValueForType(Type applicableType);
    }
}
