using BasketTestLib.Exceptions;
using BasketTestLib.Interfaces;
using BasketTestLib.Strategies;
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
            var voucherStrategy = new VoucherStrategyContext();

            if (!_codeCheckService.CheckCodeValidity(voucher.VoucherCode))
            {
                throw new VoucherCodeInvalidException($"Provided voucher code {voucher.VoucherCode} was not recognised");
            }

            switch (voucher)
            {
                case GiftVoucher:
                    voucherStrategy.SetStrategy(new GiftVoucherStrategy());
                    break;
                case OfferVoucher:
                    voucherStrategy.SetStrategy(new OfferVoucherStrategy());
                    break;
                default:
                    throw new VoucherTypeNotRecognisedException($"The voucher type of '{voucher.GetType()}' was not recognised.");
            }

            return voucherStrategy.ApplyVoucher(_codeCheckService, voucher, this, out message);            
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
