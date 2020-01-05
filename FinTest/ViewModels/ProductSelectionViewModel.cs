using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceLayer.DTO;

namespace FinTest.ViewModels
{
    public class ProductSelectionViewModel
    {
        public List<ProductViewModel> Products { get; set; }
        public decimal Total { get; set; }
        public List<DiscountViewModel> Discounts { get; set; }
        public CartDTO Cart { get; set; }
    }
}