﻿using BasketTestLib.Interfaces;
using System;
using System.Collections.Generic;

namespace BasketTestLib.Models
{
    public class OfferVoucher : IVoucher
    {
        public decimal DiscountAmount { get; }
        public decimal ThresholdToActivate { get; }
        public Type ApplicableProductType { get; }
        public string VoucherCode { get; }

        public OfferVoucher(decimal discount, decimal thresholdToActivate, string voucherCode, Type applicableProductType)
        {
            DiscountAmount = discount;
            VoucherCode = voucherCode;
            ThresholdToActivate = thresholdToActivate;
            ApplicableProductType = applicableProductType;
        }        

        public bool CheckValidity(List<Product> basketContents, ICodeCheckService codeCheckService, out string message)
        {
            this.ValidateVoucher(codeCheckService);

            message = string.Empty;
            (bool foundValidProduct, decimal totalPriceOfBasket) = ProcessBasketItems(basketContents);

            if (!foundValidProduct)
            {
                message = $"There are no products in your basket applicable to Offer Voucher {VoucherCode}.";
                return false;
            }
            else if (foundValidProduct && totalPriceOfBasket <= ThresholdToActivate)
            {
                message = $"You have not reached the spend threshold for Gift Voucher {VoucherCode}. Spend another £{ThresholdToActivate - totalPriceOfBasket + 0.01m} to receive £{DiscountAmount:.00##} discount from your basket total.";
                return false;
            }

            return true;
        }

        private ValueTuple<bool, decimal> ProcessBasketItems(List<Product> basketContents)
        {
            bool foundValidProduct = false;
            decimal totalPriceOfBasket = 0.0m;

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

            return (foundValidProduct, totalPriceOfBasket);
        }

        public bool ApplyVoucher(ICodeCheckService codeCheckService, IBasketService basket, out string message)
        {
            message = string.Empty;
            bool successfullyApplied;
            foreach (var appliedVoucher in basket.AppliedVouchers)
            {
                if (appliedVoucher.GetType() == typeof(OfferVoucher))
                {
                    message = "An offer voucher has already been applied, only one offer voucher may be used per transaction";
                    return false;
                }
            }            

            if (CheckValidity(basket.BasketContents, codeCheckService, out string offerCheckMessage))
            {
                var maxDiscountable = basket.GetTotalValueForType(ApplicableProductType);
                basket.IncrementBasketDiscount(maxDiscountable - DiscountAmount > 0 ? DiscountAmount : maxDiscountable);
                successfullyApplied = true;
                basket.AppliedVouchers.Add(this);
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
