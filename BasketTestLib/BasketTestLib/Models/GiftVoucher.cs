using BasketTestLib.Interfaces;
using System.Collections.Generic;

namespace BasketTestLib.Models
{
    public class GiftVoucher : Product, IVoucher
    {
        public decimal DiscountAmount { get; }
        public string VoucherCode { get; set; }
        public decimal ThresholdToActivate { get; }

        public GiftVoucher(decimal unitPrice, string voucherCode) : base(unitPrice)
        {
            DiscountAmount = unitPrice;
            VoucherCode = voucherCode;
        }

        public bool CheckValidity(List<Product> basketContents, out string message)
        {
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
    }
}
