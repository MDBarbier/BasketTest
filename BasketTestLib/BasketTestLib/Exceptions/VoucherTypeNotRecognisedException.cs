using System;

namespace BasketTestLib.Exceptions
{
    [Serializable]
    class VoucherTypeNotRecognisedException : Exception
    {
        public VoucherTypeNotRecognisedException() { }

        public VoucherTypeNotRecognisedException(string message)
            : base(message)
        {

        }
    }
}
