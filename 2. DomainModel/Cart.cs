using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class Cart
    {
        public List<CartItem> CartItems { get; set; }

        decimal _total;
        public decimal Total {
            get { return _total; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Total cannot be less then zero.");
                _total = value;
            }
        }
    }
}
