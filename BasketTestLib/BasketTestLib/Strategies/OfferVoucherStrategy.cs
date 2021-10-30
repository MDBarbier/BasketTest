using BasketTestLib.Exceptions;
using BasketTestLib.Interfaces;
using BasketTestLib.Models;

namespace BasketTestLib.Strategies
{
    public class OfferVoucherStrategy : IVoucherStrategy
    {
        public bool ApplyVoucher(ICodeCheckService codeCheckService, IVoucher voucher, IBasket basket, out string message)
        {
            message = string.Empty;
            if (!codeCheckService.CheckCodeValidity(voucher.VoucherCode))
            {
                throw new VoucherCodeInvalidException($"Provided voucher code {voucher.VoucherCode} was not recognised");
            }

            bool successfullyApplied;

            if (voucher.CheckValidity(basket.BasketContents, out string offerCheckMessage))
            {
                var offerVoucher = (OfferVoucher)voucher;
                var maxDiscountable = basket.GetTotalValueForType(offerVoucher.ApplicableProductType);
                var actualDiscount = maxDiscountable - voucher.DiscountAmount > 0 ? voucher.DiscountAmount : maxDiscountable;
                basket.BasketDiscount += actualDiscount;
                successfullyApplied = true;
                basket.AppliedVouchers.Add(voucher);
            }
            else
            {
                message = offerCheckMessage;
                successfullyApplied = false;
            }

            return successfullyApplied;
        }
    }
}
