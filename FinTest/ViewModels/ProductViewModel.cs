using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinTest.ViewModels
{
    public class ProductViewModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int AmountInCart { get; set; }      
        public bool Available { get; set; }
    }
}