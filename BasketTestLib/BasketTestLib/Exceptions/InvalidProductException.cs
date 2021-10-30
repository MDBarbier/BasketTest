using System;

namespace BasketTestLib.Exceptions
{
    [Serializable]
    public class InvalidProductException : Exception
    {
        public InvalidProductException() { }

        public InvalidProductException(string message)
            : base(message)
        {

        }
    }
}
