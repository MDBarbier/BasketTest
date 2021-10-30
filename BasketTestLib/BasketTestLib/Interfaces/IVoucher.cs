using BasketTestLib.Models;
using System.Collections.Generic;

/// <summary>
/// Represents a generic voucher
/// </summary>
namespace BasketTestLib.Interfaces
{
    public interface IVoucher
    {
        public decimal DiscountAmount { get; }        
        public string VoucherCode { get; set; }
        public decimal ThresholdToActivate { get; }

        bool CheckValidity(List<Product> basketContents, out string message);
    }
}
