using BasketTestLib.Interfaces;
using System.Collections.Generic;

namespace BasketTestLib.Models
{
    public class GiftVoucher : Product, IVoucher
    {
        public decimal DiscountAmount { get; }
        public string VoucherCode { get; }
        public decimal ThresholdToActivate { get; }

        public GiftVoucher(decimal unitPrice, string voucherCode) : base(unitPrice)
        {
            DiscountAmount = unitPrice;
            VoucherCode = voucherCode;
        }

        public bool CheckValidity(List<Product> basketContents, ICodeCheckService codeCheckService, out string message)
        {
            this.ValidateVoucher(codeCheckService);

            message = string.Empty;
            bool foundValidProduct = false;

            foreach (var item in basketContents)
            {
                if (item.GetType() != typeof(GiftVoucher))
                {
                    foundValidProduct = true;
                    break;
                }
            }

            if (!foundValidProduct)
            {
                message = $"There are no products in your basket applicable to Gift Voucher {VoucherCode}.";
                return false;
            }

            return true;
        }

        public bool ApplyVoucher(ICodeCheckService codeCheckService, IBasketService basket, out string message)
        {
            message = string.Empty;
            bool successfullyApplied;
            
            if (CheckValidity(basket.BasketContents, codeCheckService, out string giftCheckMessage))
            {
                var maxDiscountable = basket.BasketNetTotal - basket.GetTotalValueForType(typeof(GiftVoucher));
                basket.IncrementBasketDiscount(maxDiscountable - DiscountAmount > 0 ? DiscountAmount : maxDiscountable);
                successfullyApplied = true;
                basket.AppliedVouchers.Add(this);
            }
            else
            {
                message = giftCheckMessage;
                successfullyApplied = false;
            }

            return successfullyApplied;
        }
    }
}
