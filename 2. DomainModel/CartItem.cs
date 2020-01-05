using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel
{
    public class CartItem
    {       
        ProductItem _product;
        public ProductItem Product
        {
            get { return _product; }
            set
            {
                if (value == null)
                    throw new ArgumentException("Product Item cannot be null.");
                _product = value;
            }
        }


        decimal _discountedPrice;
        public decimal DiscountedPrice
        {
            get { return _discountedPrice; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Discounted price cannot be less then zero.");
                _discountedPrice = value;
            }
        }

        int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Quantity cannot be less then zero.");
                _quantity = value;
            }
        }

    }
}
