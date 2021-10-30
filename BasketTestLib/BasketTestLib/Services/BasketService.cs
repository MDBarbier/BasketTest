using BasketTestLib.Exceptions;
using BasketTestLib.Extensions;
using BasketTestLib.Interfaces;
using BasketTestLib.Models;
using BasketTestLib.Validators;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasketTestLib.Services
{
    public class BasketService : IBasketService
    {
        #region Auto implemented Properties

        public List<Product> BasketContents { get; private set; }
        public List<IVoucher> AppliedVouchers { get; private set; }
        public decimal BasketNetTotal { get; private set; }
        public decimal BasketDiscount { get; private set; }
        public Guid BasketGuid { get; private set; }

        #endregion

        #region Local Fields

        readonly ICodeCheckService _codeCheckService;

        #endregion

        #region Constructors

        public BasketService(ICodeCheckService codeCheckService)
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
            message = string.Empty;
            if (BasketContents.Remove(product))
            {
                BasketNetTotal -= product.UnitPrice;
                RecalculateDiscount(out message);
            }
        }

        private void RecalculateDiscount(out string compositeMessage)
        {
            BasketDiscount = 0.0m;            
            var sb = new StringBuilder();
            var tempVoucherList = AppliedVouchers.DeepCopy();
            AppliedVouchers.Clear();

            foreach (var voucher in tempVoucherList)
            {
                if (!voucher.ApplyVoucher(_codeCheckService, this, out string message))
                {
                    sb.Append(message);
                }                
            }

            compositeMessage = sb.ToString();
        }

        public decimal GetBasketFinalValue()
        {
            var finalAmount = 0m;
            var discountableBasketTotal = GetNonVoucherNetTotal();
            var unDiscountableAmount = BasketNetTotal - discountableBasketTotal;           
            var resultOfDiscount = discountableBasketTotal - BasketDiscount;

            if (resultOfDiscount > 0)
            {
                finalAmount += resultOfDiscount;
            }

            finalAmount += unDiscountableAmount;
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

        public decimal GetTotalValueForType(Type applicableType)
        {
            var productsOfType = BasketContents.Where(item => item.GetType() == applicableType
                                                              || item.GetType().IsSubclassOf(applicableType)).ToList();
            var subTotal = 0.0m;

            foreach (var item in productsOfType)
            {
                subTotal += item.UnitPrice;
            }

            return subTotal;
        }

        void IBasketService.IncrementBasketDiscount(decimal amount)
        {
            BasketDiscount += amount;
        }

        #endregion
    }
}
