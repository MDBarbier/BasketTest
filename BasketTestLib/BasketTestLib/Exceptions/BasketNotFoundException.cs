using System;

namespace BasketTestLib.Exceptions
{
    [Serializable]
    class BasketNotFoundException : Exception
    {
        public BasketNotFoundException() { }

        public BasketNotFoundException(string message)
            : base(message)
        {

        }
    }
}
