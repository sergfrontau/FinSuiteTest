using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FinTest.ViewModels;
using System.Web.Mvc;
using ServiceLayer;
using ServiceLayer.DTO;
using System.Threading.Tasks;

namespace FinTest.APIControllers
{
    public class CartController : ApiController
    {
        public CartController(IProductService productService)
        {
            _productService = productService;
        }


        IProductService _productService;

        /// <summary>
        /// Adds a new product item to cart
        /// </summary>       
        public async Task<IHttpActionResult> Post(string id, List<CartItemDTO> existingCartItems)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Product code is mandatory");

            if (existingCartItems == null)
                existingCartItems = new List<CartItemDTO>();

            if (existingCartItems.Where(x => x == null).Any())
                throw new ArgumentException("Cart Item cannot be null");
           
            
            CartDTO cart = await _productService.AddProductToCart(id, existingCartItems);

            List<ProductItemDTO> productDTOs = await _productService.FindAllProducts();          

            List<DiscountViewModel> listOfDiscounts = CalculateDiscounts(productDTOs, cart);

            ProductSelectionViewModel res = new ProductSelectionViewModel()
            {
                Products = productDTOs.Select(x => new ProductViewModel() { Code = x.Code,
                    Description = x.Description,
                    Price = x.Price,
                    AmountInCart = (cart.CartItems.Where(c => c.Product.Code == x.Code).FirstOrDefault() != null ? cart.CartItems.Where(c => c.Product.Code == x.Code).FirstOrDefault().Quantity : 0) }).ToList(),
                Total = cart.Total,
                Discounts = listOfDiscounts,
                Cart = cart
            };            


            foreach (ProductViewModel product in res.Products)
            {
                CartItemDTO cartItem = cart.CartItems.Where(x => x.Product.Code == product.Code).FirstOrDefault();

                if (cartItem != null)
                    product.Available = product.AmountInCart < cartItem.Product.AmountInStock;
                else                    
                    product.Available = productDTOs.Where(x => x.Code == product.Code).FirstOrDefault().AmountInStock > 0;
            }

            return Json(res);
        }       


        private List<DiscountViewModel> CalculateDiscounts(List<ProductItemDTO> productDTOs, CartDTO cart)
        {
            List<DiscountViewModel> listOfDiscounts = cart.CartItems.Where(x=> x.Product.Price * x.Quantity > x.DiscountedPrice) .Select(x => 
            new DiscountViewModel() {
                Product = x.Product.Description + ": ",
                Discount = x.Product.Price * x.Quantity - x.DiscountedPrice
            }).ToList();          

            decimal totalDiscountedPrice = cart.CartItems.Select(x => x.DiscountedPrice).Sum();

            if (cart.Total < totalDiscountedPrice)
                listOfDiscounts.Add(new DiscountViewModel() { Product = "Discount on Total: ", Discount = totalDiscountedPrice - cart.Total });

            return listOfDiscounts;
        }
    }
}