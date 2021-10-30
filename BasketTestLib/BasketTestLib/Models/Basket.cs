using BasketTestLib.Exceptions;
using BasketTestLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BasketTestLib.Models
{
    public class Basket : IBasket
    {
        #region Auto implemented Properties
        public List<Product> BasketContents { get; set; }
        public float BasketNetTotal { get; set; }
        public float BasketDiscount { get; set; }
        public Guid BasketGuid { get; set; }
        #endregion

        #region Local Fields
        ICodeCheckService _codeCheckService;
        #endregion

        #region Constructors
        public Basket(ICodeCheckService codeCheckService)
        {
            BasketContents = new List<Product>();
            _codeCheckService = codeCheckService;
        }
        #endregion

        #region Instance methods
        
        public void AddProduct(Product product)
        {
            BasketContents.Add(product);
            BasketNetTotal += product.UnitPrice;
        }

        public float GetBasketFinalValue()
        {
            var discountableBasketTotal = GetNonVoucherNetTotal();
            var undiscountableAmount = BasketNetTotal - discountableBasketTotal;
            var finalAmount = 0f;
            var resultOfDiscount = discountableBasketTotal - BasketDiscount;

            if (resultOfDiscount > 0)
            {
                finalAmount += resultOfDiscount;
            }

            finalAmount += undiscountableAmount;
            return finalAmount;
        }

        public float GetNonVoucherNetTotal()
        {
            float runningTotal = 0f;

            foreach (var product in BasketContents)
            {
                switch (product)
                {
                    case GiftVoucher:
                        break;
                    default:
                        runningTotal += product.UnitPrice;
                        break;
                }
            }

            return runningTotal;
        }

        public bool ApplyVoucher(IVoucher voucher, out string message)
        {
            message = string.Empty;
            if (!_codeCheckService.CheckCodeValidity(voucher.VoucherCode))
            {
                throw new VoucherCodeInvalidException($"Provided voucher code {voucher.VoucherCode} was not recognised");
            }

            var successfullyApplied = voucher switch
            {
                GiftVoucher giftVoucher => ApplyGiftVoucher(ref message, giftVoucher),
                OfferVoucher offerVoucher => ApplyOfferVoucher(ref message, offerVoucher),
                _ => throw new VoucherTypeNotRecognisedException($"The voucher type of '{voucher.GetType()}' was not recognised."),
            };
            return successfullyApplied;
        }

        private bool ApplyOfferVoucher(ref string message, OfferVoucher offerVoucher)
        {
            bool successfullyApplied;
            if (offerVoucher.CheckValidity(BasketContents, out string offerMessage))
            {
                var maxDiscountable = GetTotalValueForType(offerVoucher.ApplicableProductType);
                var actualDiscount = maxDiscountable - offerVoucher.DiscountAmount > 0 ? offerVoucher.DiscountAmount : maxDiscountable;
                BasketDiscount += actualDiscount;
                successfullyApplied = true;
            }
            else
            {
                message = offerMessage;
                successfullyApplied = false;
            }

            return successfullyApplied;
        }

        private bool ApplyGiftVoucher(ref string message, GiftVoucher giftVoucher)
        {
            bool successfullyApplied;
            if (giftVoucher.CheckValidity(BasketContents, out string giftMessage))
            {
                var unDiscountable = GetTotalValueForType(typeof(GiftVoucher));
                var maxDiscountable = BasketNetTotal - unDiscountable;
                var actualDiscount = maxDiscountable - giftVoucher.DiscountAmount > 0 ? giftVoucher.DiscountAmount : maxDiscountable;
                BasketDiscount += actualDiscount;
                successfullyApplied = true;
            }
            else
            {
                message = giftMessage;
                successfullyApplied = false;
            }

            return successfullyApplied;
        }

        public float GetTotalValueForType(Type applicableType)
        {
            var productsOfType = BasketContents.Where(item => item.GetType() == applicableType || item.GetType().IsSubclassOf(applicableType)).ToList();
            var subTotal = 0.0f;

            foreach (var item in productsOfType)
            {
                subTotal += item.UnitPrice;
            }

            return subTotal;
        }
        #endregion
    }
}
