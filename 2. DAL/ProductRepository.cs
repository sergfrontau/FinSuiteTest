using System;
using System.Collections.Generic;
using DomainModel;
using System.Linq;

namespace DAL
{
    public class ProductRepository: IProductRepository
    {
        /// <summary>
        /// Database access method to get list of products
        /// </summary>        
        public IQueryable<ProductItem> GetProducts()
        {
            List<ProductItem> result = new List<ProductItem>();
            result.Add(new ProductItem() { Code = "SMG", Description = "Samsung Galaxy Phone", Price = 695, AmountInStock = 5 });
            result.Add(new ProductItem() { Code = "CRM", Description = "Chromecast 2", Price = 55, AmountInStock = 10 });
            result.Add(new ProductItem() { Code = "SPH", Description = "Sennheiser Earphones", Price = 80, AmountInStock = 10 });
            result.Add(new ProductItem() { Code = "FWC", Description = "FitBit Watch", AmountInStock = 0 });

            return result.AsQueryable();
        }
    }
}
