using System.Collections.Generic;

namespace BasketTestLib.Extensions
{
    public static class List
    {
        public static List<T> DeepCopy<T>(this List<T> l)
        {
            List<T> newList = new();

            foreach (var item in l)
            {
                newList.Add(item);
            }

            return newList;
        }
    }
}
