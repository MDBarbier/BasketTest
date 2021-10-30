using BasketTestLib.Exceptions;
using BasketTestLib.Interfaces;
using BasketTestLib.Models;

namespace BasketTestLib.Strategies
{
    public class GiftVoucherStrategy : IVoucherStrategy
    {
        public bool ApplyVoucher(ICodeCheckService codeCheckService, IVoucher voucher, IBasket basket, out string message)
        {
            message = string.Empty;
            if (!codeCheckService.CheckCodeValidity(voucher.VoucherCode))
            {
                throw new VoucherCodeInvalidException($"Provided voucher code {voucher.VoucherCode} was not recognised");
            }

            bool successfullyApplied;
            
            if (voucher.CheckValidity(basket.BasketContents, out string giftCheckMessage))
            {
                var unDiscountable = basket.GetTotalValueForType(typeof(GiftVoucher));
                var maxDiscountable = basket.BasketNetTotal - unDiscountable;
                var actualDiscount = maxDiscountable - voucher.DiscountAmount > 0 ? voucher.DiscountAmount : maxDiscountable;
                basket.BasketDiscount += actualDiscount;
                successfullyApplied = true;
                basket.AppliedVouchers.Add(voucher);
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
