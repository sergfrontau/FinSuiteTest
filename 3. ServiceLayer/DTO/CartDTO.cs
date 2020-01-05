using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DTO
{
    public class CartDTO
    {
        public List<CartItemDTO> CartItems { get; set; }
        public decimal Total { get; set; }
    }
}
