using BasketTestLib.Exceptions;
using BasketTestLib.Extensions;
using BasketTestLib.Interfaces;
using BasketTestLib.Strategies;
using BasketTestLib.Validators;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasketTestLib.Models
{
    public class Basket : IBasket
    {
        #region Auto implemented Properties
        public List<Product> BasketContents { get; set; }
        public List<IVoucher> AppliedVouchers { get; set; }
        public decimal BasketNetTotal { get; set; }
        public decimal BasketDiscount { get; set; }
        public Guid BasketGuid { get; set; }
        #endregion

        #region Local Fields
        ICodeCheckService _codeCheckService;
        #endregion

        #region Constructors
        public Basket(ICodeCheckService codeCheckService)
        {
            BasketContents = new List<Product>();
            AppliedVouchers = new List<IVoucher>();
            _codeCheckService = codeCheckService;
        }
        #endregion

        #region Instance methods
        
        public void AddProduct(Product product)
        {
            var validator = new ProductValidator();
            ValidationResult resultOfValidation = validator.Validate(product);

            if (resultOfValidation.IsValid)
            {
                BasketContents.Add(product);
                BasketNetTotal += product.UnitPrice;
            }
            else
            {
                throw new InvalidProductException("Validation failed with message: " + resultOfValidation.ToString());
            }
        }

        public void RemoveProduct(Product product, out string message)
        {
            BasketContents.Remove(product);

            RecalculateDiscount(out message);
        }

        private void RecalculateDiscount(out string compositeMessage)
        {
            BasketDiscount = 0.0m;            
            var sb = new StringBuilder();
            List<IVoucher> tempVoucherList = AppliedVouchers.DeepCopy();
            AppliedVouchers.Clear();

            foreach (var voucher in tempVoucherList)
            {
                if (!ApplyVoucher(voucher, out string message))
                {
                    sb.Append(message);
                }                
            }

            compositeMessage = sb.ToString();
        }

        public decimal GetBasketFinalValue()
        {
            var discountableBasketTotal = GetNonVoucherNetTotal();
            var undiscountableAmount = BasketNetTotal - discountableBasketTotal;
            var finalAmount = 0m;
            var resultOfDiscount = discountableBasketTotal - BasketDiscount;

            if (resultOfDiscount > 0)
            {
                finalAmount += resultOfDiscount;
            }

            finalAmount += undiscountableAmount;
            return finalAmount;
        }

        public decimal GetNonVoucherNetTotal()
        {
            decimal runningTotal = 0m;

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
            ValidateVoucher(voucher);
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

        private static void ValidateVoucher(IVoucher voucher)
        {
            var validator = new VoucherValidator();
            ValidationResult resultOfValidation = validator.Validate(voucher);

            if (!resultOfValidation.IsValid)
            {
                throw new InvalidVoucherException("Validation failed with message: " + resultOfValidation.ToString());
            }
        }

        public decimal GetTotalValueForType(Type applicableType)
        {
            var productsOfType = BasketContents.Where(item => item.GetType() == applicableType || item.GetType().IsSubclassOf(applicableType)).ToList();
            var subTotal = 0.0m;

            foreach (var item in productsOfType)
            {
                subTotal += item.UnitPrice;
            }

            return subTotal;
        }
        #endregion
    }
}
