using System;
using System.Collections.Generic;
using System.Text;
using DomainModel;
using DAL;
using System.Linq;
using ServiceLayer.DTO;
using System.Threading.Tasks;
using System.Xml;
using RuleEngine;
using RuleEngine.Compiler;
using System.Xml.Linq;
using AutoMapper;

namespace ServiceLayer
{
    public class ProductsService: IProductService
    {
        public ProductsService(IProductRepository repository, IRulesEngine rulesEngine, IMapper mapper)
        {
            _repository = repository;
            _rulesEngine = rulesEngine;
            _mapper = mapper;
        }

        IProductRepository _repository;
        IRulesEngine _rulesEngine;
        IMapper _mapper;

        /// <summary>
        /// Returns all products
        /// </summary>        
        public async Task<List<ProductItemDTO>> FindAllProducts()
        {
            List<ProductItem> products = await Task.Run(() => _repository.GetProducts().ToList());

            return products.Select(_mapper.Map<ProductItemDTO>).ToList();            
        }

        /// <summary>
        /// Add a new item to cart and calculate prices with discounts
        /// </summary>        
        public async Task<CartDTO> AddProductToCart(string id, List<CartItemDTO> cartItemsDTO)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Product code is mandatory");

            if (cartItemsDTO == null)
                cartItemsDTO = new List<CartItemDTO>();

            if (cartItemsDTO.Where(x => x == null).Any())
                throw new ArgumentException("Cart Item cannot be null");


            List<ProductItem> products = await Task.Run(() => _repository.GetProducts().ToList());           
            List<CartItem> cartItems = cartItemsDTO.Select(_mapper.Map<CartItem>).ToList();

            CartItem item = cartItems.Where(x => x.Product.Code == id).FirstOrDefault();

            if (item != null)
                item.Quantity++;
            else
            {
                ProductItem productToAdd = products.Where(x => x.Code == id).FirstOrDefault();

                if (productToAdd != null)
                {
                    ProductItem product = new ProductItem() { Code = productToAdd.Code, Description = productToAdd.Description, Price = productToAdd.Price, AmountInStock = productToAdd.AmountInStock };
                    item = new CartItem() { Product = product, DiscountedPrice = 0, Quantity = 1 };
                    cartItems.Add(item);
                }
                else
                    throw new InvalidOperationException("Sorry, this product is not available.");
            }
            
            Cart cart = _rulesEngine.CalculateCartPrices(cartItems, products);

            CartDTO result = _mapper.Map<CartDTO>(cart);          
            return result;
        }
    }


    



  
}
