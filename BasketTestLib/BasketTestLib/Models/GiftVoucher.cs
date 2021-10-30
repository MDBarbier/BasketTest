using BasketTestLib.Exceptions;
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
            if (!codeCheckService.CheckCodeValidity(VoucherCode))
            {
                throw new VoucherCodeInvalidException($"Provided voucher code {VoucherCode} was not recognised");
            }

            bool successfullyApplied;

            if (CheckValidity(basket.BasketContents, codeCheckService, out string giftCheckMessage))
            {
                var unDiscountable = basket.GetTotalValueForType(typeof(GiftVoucher));
                var maxDiscountable = basket.BasketNetTotal - unDiscountable;
                var actualDiscount = maxDiscountable - DiscountAmount > 0 ? DiscountAmount : maxDiscountable;
                basket.IncrementBasketDiscount(actualDiscount);
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
