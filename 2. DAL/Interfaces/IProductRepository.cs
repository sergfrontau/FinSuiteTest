using System;
using System.Collections.Generic;
using System.Text;
using DomainModel;
using System.Linq;


namespace DAL
{
    public interface IProductRepository
    {
        IQueryable<ProductItem> GetProducts();
    }
}
