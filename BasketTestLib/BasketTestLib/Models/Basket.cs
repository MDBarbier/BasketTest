using BasketTestLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketTestLib.Models
{
    public class Basket
    {        
        public List<Product> Products { get; set; }
        
        public Basket()
        {
            Products = new List<Product>();
        }

        public void AddProduct(Product product)
        {
            Products.Add(product);
        }

        public float GetBasketTotalValue()
        {
            float runningTotal = 0f;

            foreach (var product in Products)
            {
                runningTotal += product.UnitPrice;
            }

            return runningTotal;
        }

        public bool ApplyVoucher(IVoucher voucher)
        {
            switch (voucher)
            {
                case GiftVoucher giftVoucher:
                    //todo - convert basket total to be held in a prop and updated when product added or voucher applied
                    //todo - apply gift voucher to basket total
                    break;
                case OfferVoucher offerVoucher:
                    //todo - check validity of offer voucher
                    //todo - apply offer voucher to basket total
                    break;
                default:
                    break;
            }

            return true;
        }
    }
}
