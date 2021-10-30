/// <summary>
/// Represents a strategy for handling vouchers, allowing customisation of the ApplyVoucher functionality.
/// </summary>
namespace BasketTestLib.Interfaces
{
    public interface IVoucherStrategy
    {
        bool ApplyVoucher(ICodeCheckService codeCheckService, IVoucher voucher, IBasket basket, out string message);
    }
}
