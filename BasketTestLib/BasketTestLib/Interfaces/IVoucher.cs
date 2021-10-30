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
        public string VoucherCode { get; }
        public decimal ThresholdToActivate { get; }

        public bool CheckValidity(List<Product> basketContents, ICodeCheckService codeCheckService, out string message);
        public bool ApplyVoucher(ICodeCheckService codeCheckService, IBasketService basket, out string message);
    }
}
