using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FinTest;
using FinTest.Controllers;
using Moq;
using ServiceLayer;
using DAL;
using DomainModel;
using AutoMapper;
using System.Threading.Tasks;
using ServiceLayer.DTO;

namespace FinTest.Tests
{
    [TestClass]
    public class ProductServiceTest
    {        

        [TestMethod]
        public void GetAllProducts_Success()
        {
            // Arrange            
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<ProductItemDTO>(It.IsAny<ProductItem>())).Returns(new ProductItemDTO());

            var mockDb = new Mock<IProductRepository>();
            List<ProductItem> result = CreateProductItemsList();
            mockDb.Setup(x => x.GetProducts()).Returns(result.AsQueryable());
                       
            IProductService service = new ProductsService(mockDb.Object, null, mapperMock.Object);

            //Act
            List<ProductItemDTO> list = service.FindAllProducts().Result;
          
            // Assert
            Assert.AreEqual(2, list.Count());
        }



        [TestMethod]
        public void AddNewProductToEmptyCart_Success()
        {
            // Arrange            
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<CartDTO>(It.IsAny<Cart>())).Returns((Cart c) => new CartDTO() { Total = c.Total, CartItems = c.CartItems.Select(z => new CartItemDTO()).ToList() });

            var rulesMock = new Mock<IRulesEngine>();
            rulesMock.Setup(m => m.CalculateCartPrices(It.IsAny<List<CartItem>>(), It.IsAny<List<ProductItem>>())).Returns((List<CartItem> c, List<ProductItem> products) => new Cart() { CartItems = c, Total = 555 });

            var mockDb = new Mock<IProductRepository>();
            List<ProductItem> result = CreateProductItemsList();           
            mockDb.Setup(x => x.GetProducts()).Returns(result.AsQueryable());

            IProductService service = new ProductsService(mockDb.Object, rulesMock.Object, mapperMock.Object);

            //Act
            CartDTO cart = service.AddProductToCart("CRM", new List<CartItemDTO>()).Result;

            // Assert           
            Assert.AreEqual(1, cart.CartItems.Count());
        }


        [TestMethod]
        public void IncreaseExistingProductInCart_Success()
        {
            // Arrange            
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<CartDTO>(It.IsAny<Cart>())).Returns((Cart c) => new CartDTO() { Total = c.Total, CartItems = c.CartItems.Select(z => new CartItemDTO() { Quantity = z.Quantity }).ToList() });           
            mapperMock.Setup(m => m.Map<CartItem>(It.IsAny<CartItemDTO>())).Returns((CartItemDTO c) => new CartItem() { Quantity = 1, Product = new ProductItem() {Code = c.Product.Code } });

            var rulesMock = new Mock<IRulesEngine>();
            rulesMock.Setup(m => m.CalculateCartPrices(It.IsAny<List<CartItem>>(), It.IsAny<List<ProductItem>>())).Returns((List<CartItem> c, List<ProductItem> products) => new Cart() { CartItems = c, Total = 555 });

            var mockDb = new Mock<IProductRepository>();
            List<ProductItem> result = CreateProductItemsList();
            mockDb.Setup(x => x.GetProducts()).Returns(result.AsQueryable());

            IProductService service = new ProductsService(mockDb.Object, rulesMock.Object, mapperMock.Object);

            List<CartItemDTO> cartItems = new List<CartItemDTO>();
            cartItems.Add(new CartItemDTO() { Product = new ProductItemDTO() { Code = "CRM" }, Quantity = 1});

            //Act
            CartDTO cart = service.AddProductToCart("CRM", cartItems).Result;

            // Assert           
            Assert.AreEqual(2, cart.CartItems[0].Quantity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]       
        public async Task CodeNotProvided_Fail()
        {            
            IProductService service = new ProductsService(null, null, null);

            List<CartItemDTO> cartItems = new List<CartItemDTO>();
            cartItems.Add(new CartItemDTO() { Product = new ProductItemDTO() { Code = "CRM" }, Quantity = 1 });

            //Act
            CartDTO cart = await service.AddProductToCart("", cartItems);           
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CartItemsNotProvided_Fail()
        {
            IProductService service = new ProductsService(null, null, null);

            List<CartItemDTO> cartItems = new List<CartItemDTO>();
            cartItems.Add(null);

            //Act
            CartDTO cart = await service.AddProductToCart("CRM", cartItems);
        }



        private List<ProductItem> CreateProductItemsList()
        {
            List<ProductItem> result = new List<ProductItem>();
            result.Add(new ProductItem() { Code = "SMG", Description = "Samsung Galaxy Phone", Price = 695, AmountInStock = 5 });
            result.Add(new ProductItem() { Code = "CRM", Description = "Chromecast 2", Price = 55, AmountInStock = 10 });

            return result;
        }

    }
}
