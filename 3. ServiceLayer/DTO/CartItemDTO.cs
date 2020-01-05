using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DTO
{
    public class CartItemDTO
    {
        public ProductItemDTO Product { get; set; }
        public decimal DiscountedPrice { get; set; }
        public int Quantity { get; set; }
    }
}
