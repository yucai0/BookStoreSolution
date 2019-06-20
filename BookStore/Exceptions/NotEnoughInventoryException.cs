using BookStore.Contract;
using System;
using System.Collections.Generic;

namespace BookStore.Exceptions
{
    public class NotEnoughInventoryException : Exception
    {
        public IEnumerable<INameQuantity> Missing { get; }
        public NotEnoughInventoryException(IEnumerable<INameQuantity> missing)
        {
            Missing = missing;
        }
    }
}
