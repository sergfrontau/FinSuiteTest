using System;
using System.Collections.Generic;
using System.Text;
using DomainModel;
using ServiceLayer.DTO;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public interface IRulesEngine
    {
        Cart CalculateCartPrices(List<CartItem> cartItems, List<ProductItem> products);
    }
}
