using BasketTestLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketTestLib.Models
{
    public class GiftVoucher : Product, IVoucher
    {
        public GiftVoucher(float unitPrice) : base(unitPrice)
        {

        }
    }
}
