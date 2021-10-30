﻿using System;

namespace BasketTestLib.Exceptions
{
    [Serializable]
    public class BasketNotFoundException : Exception
    {
        public BasketNotFoundException() { }

        public BasketNotFoundException(string message)
            : base(message)
        {

        }
    }
}
