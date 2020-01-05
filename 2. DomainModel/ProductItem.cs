using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel
{
    public class ProductItem
    {        
        string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Code is mandatory.");
                _code = value;
            }
        }

        public string Description { get; set; }
       

        decimal _price;
        public decimal Price
        {
            get { return _price; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Price cannot be less then zero.");
                _price = value;
            }
        }


        int _amountInStock;
        public int AmountInStock
        {
            get { return _amountInStock; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Amount in stock cannot be less then zero.");
                _amountInStock = value;
            }
        }

    }
}
