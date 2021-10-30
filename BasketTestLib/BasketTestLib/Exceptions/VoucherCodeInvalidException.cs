using System;

namespace BasketTestLib.Exceptions
{
    [Serializable]
    class VoucherCodeInvalidException : Exception
    {
        public VoucherCodeInvalidException() { }

        public VoucherCodeInvalidException(string message)
            : base(message)
        {

        }
    }
}
