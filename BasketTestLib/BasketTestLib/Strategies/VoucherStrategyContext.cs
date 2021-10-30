using BasketTestLib.Interfaces;

namespace BasketTestLib.Strategies
{
    /// <summary>
    /// Context for managing the strategy used for Vouchers
    /// </summary>
    public class VoucherStrategyContext
    {
        private IVoucherStrategy _voucherStrategy;

        public VoucherStrategyContext() { }

        public VoucherStrategyContext(IVoucherStrategy voucherStrategy)
        {
            _voucherStrategy = voucherStrategy;
        }

        public void SetStrategy(IVoucherStrategy voucherStrategy)
        {
            _voucherStrategy = voucherStrategy;
        }

        public bool ApplyVoucher(ICodeCheckService codeCheckService, IVoucher voucher, IBasket basket, out string message)
        {
            var result = _voucherStrategy.ApplyVoucher(codeCheckService, voucher, basket, out string innerMessage);
            message = innerMessage;
            return result;
        }
    }
}
