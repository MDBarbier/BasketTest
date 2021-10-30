using BasketTestLib.Models;

namespace BasketTestLib.Interfaces
{
    public interface IVoucherStrategy
    {
        bool ApplyVoucher(ICodeCheckService codeCheckService, IVoucher voucher, IBasket basket, out string message);
    }
}
