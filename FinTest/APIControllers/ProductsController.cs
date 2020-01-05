using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using FinTest.ViewModels;
using System.Web.Mvc;
using ServiceLayer;
using ServiceLayer.DTO;

namespace FinTest.APIControllers
{
    public class ProductsController : ApiController
    {

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        IProductService _productService;

     
      /// <summary>
      /// Returns list of available products
      /// </summary>      
        public async Task<IHttpActionResult> Get()
        {           
            List<ProductItemDTO> productDTOs = await _productService.FindAllProducts();
            CartDTO cart = new CartDTO() { CartItems = new List<CartItemDTO>(), Total = 0 };           

            ProductSelectionViewModel result = new ProductSelectionViewModel()
            {
                Products = productDTOs.Select(x => new ProductViewModel() {
                    Code = x.Code,
                    Description = x.Description,
                    Price = x.Price,
                    AmountInCart = 0,
                    Available = x.AmountInStock > 0 }
                ).ToList(),
                Total = cart.Total,
                Discounts = new List<DiscountViewModel>(),
                Cart = new CartDTO() { CartItems = new List<CartItemDTO>(), Total = 0 }
            };

            return Json(result);
        }
      
    }
}