using System;

namespace BasketTestLib.Exceptions
{
    [Serializable]
    public class VoucherTypeNotRecognisedException : Exception
    {
        public VoucherTypeNotRecognisedException() { }

        public VoucherTypeNotRecognisedException(string message)
            : base(message)
        {

        }
    }
}
