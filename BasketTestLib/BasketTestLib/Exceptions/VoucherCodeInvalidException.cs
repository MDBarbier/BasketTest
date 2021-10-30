using System;

namespace BasketTestLib.Exceptions
{
    [Serializable]
    public class VoucherCodeInvalidException : Exception
    {
        public VoucherCodeInvalidException() { }

        public VoucherCodeInvalidException(string message)
            : base(message)
        {

        }
    }
}
