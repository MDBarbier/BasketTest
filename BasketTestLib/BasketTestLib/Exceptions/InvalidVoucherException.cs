using System;

namespace BasketTestLib.Exceptions
{
    [Serializable]
    public class InvalidVoucherException : Exception
    {
        public InvalidVoucherException() { }

        public InvalidVoucherException(string message)
            : base(message)
        {

        }
    }
}
