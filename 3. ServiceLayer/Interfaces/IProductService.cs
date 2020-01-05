using System;
using System.Collections.Generic;
using System.Text;
using DomainModel;
using ServiceLayer.DTO;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public interface IProductService
    {
        Task<List<ProductItemDTO>> FindAllProducts();
        Task<CartDTO> AddProductToCart(string id, List<CartItemDTO> cartItems);
    }
}
