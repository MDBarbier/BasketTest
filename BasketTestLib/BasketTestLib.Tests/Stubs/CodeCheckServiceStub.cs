using BasketTestLib.Interfaces;

namespace BasketTestLib.Services
{
    /// <summary>
    /// Stub to simulate an external call to a web service of some kind in order to check if a voucher code is valid or not
    /// </summary>
    public class CodeCheckServiceStub : ICodeCheckService
    {
        public bool CheckCodeValidity(string voucherCode)
        {
            if (voucherCode == "XXX-XXX" || voucherCode == "YYY-YYY")
            {
                return true;
            }

            return false;
        }
    }
}
