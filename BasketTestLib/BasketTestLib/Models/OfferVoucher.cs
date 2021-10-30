using BasketTestLib.Interfaces;
using System;
using System.Collections.Generic;

namespace BasketTestLib.Models
{
    public class OfferVoucher : IVoucher
    {
        public decimal DiscountAmount { get; }
        public decimal ThresholdToActivate { get; }
        public Type ApplicableProductType { get; }
        public string VoucherCode { get; set; }

        public OfferVoucher(decimal discount, decimal thresholdToActivate, string voucherCode, Type applicableProductType)
        {
            DiscountAmount = discount;
            VoucherCode = voucherCode;
            ThresholdToActivate = thresholdToActivate;
            ApplicableProductType = applicableProductType;
        }        

        public bool CheckValidity(List<Product> basketContents, out string message)
        {
            message = string.Empty;
            bool foundValidProduct = false;
            decimal totalPriceOfBasket = 0m;
            ProcessBasketItems(basketContents, ref foundValidProduct, ref totalPriceOfBasket);

            if (!foundValidProduct)
            {
                message = $"There are no products in your basket applicable to Offer Voucher {VoucherCode}.";
                return false;
            }
            else if (foundValidProduct && totalPriceOfBasket < ThresholdToActivate)
            {
                message = $"You have not reached the spend threshold for Gift Voucher {VoucherCode}. Spend another £{ThresholdToActivate - totalPriceOfBasket + 0.01m} to receive £{DiscountAmount.ToString(".00##")} discount from your basket total.";
                return false;
            }

            return true;
        }

        private void ProcessBasketItems(List<Product> basketContents, ref bool foundValidProduct, ref decimal totalPriceOfBasket)
        {
            foreach (var item in basketContents)
            {
                if (item.GetType() != typeof(GiftVoucher))
                {
                    totalPriceOfBasket += item.UnitPrice;
                }

                if (item.GetType() == ApplicableProductType || item.GetType().IsSubclassOf(ApplicableProductType))
                {
                    foundValidProduct = true;
                }
            }
        }
    }
}
