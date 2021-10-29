using BasketTestLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketTestLib.Models
{
    public class OfferVoucher : IVoucher
    {
        public float DiscountAmount { get; }
        public float ThresholdToActivate { get; }
        public Type ApplicableProductType { get; }

        public OfferVoucher(float discount, float thresholdToActivate, Type applicableProductType)
        {
            DiscountAmount = discount;
            ThresholdToActivate = thresholdToActivate;
            ApplicableProductType = applicableProductType;
        }        
    }
}
