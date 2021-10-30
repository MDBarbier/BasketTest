using BasketTestLib.Models;
using System;
using System.Collections.Generic;

namespace BasketTestLib.Interfaces
{
    public interface IBasketService
    {
        public List<Product> BasketContents { get; }
        public List<IVoucher> AppliedVouchers { get; }
        public decimal BasketNetTotal { get; }
        public decimal BasketDiscount { get; }
        public Guid BasketGuid { get; }


        void AddProduct(Product product);
        decimal GetBasketFinalValue();
        decimal GetNonVoucherNetTotal();
        decimal GetTotalValueForType(Type applicableType);
        void RemoveProduct(Product product, out string message);
        internal void IncrementBasketDiscount(decimal amount);
    }
}
