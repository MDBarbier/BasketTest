using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketTestLib.Extensions
{
    public static class List
    {
        public static List<T> DeepCopy<T>(this List<T> l)
        {
            List<T> newList = new List<T>();

            foreach (var item in l)
            {
                newList.Add(item);
            }

            return newList;
        }
    }
}
