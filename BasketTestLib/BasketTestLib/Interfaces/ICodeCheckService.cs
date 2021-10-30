namespace BasketTestLib.Interfaces
{
    /// <summary>
    /// Check a voucher code against an external service to see if it is valid
    /// </summary>
    public interface ICodeCheckService
    {
        bool CheckCodeValidity(string voucherCode);
    }
}
