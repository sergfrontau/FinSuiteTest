using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RuleEngine;
using RuleEngine.Compiler;
using System.Xml.Linq;
using DomainModel;

namespace ServiceLayer
{
    public class PricesRuleEngine: IRulesEngine
    {
        const string XML_FILE_NAME = "XMLRules.xml";
        const string CART_TAG = "Cart";
        const string PRICE_TAG = "Price";
        const string AMOUNT_TAG = "Amount";
        const string TOTALS_TAG = "Totals";
        const string SUM_TAG = "Sum";
        const string TOTAL_PRICE_TAG = "TotalPrice";

        /// <summary>
        /// Calculate discounts and return Cart with updated prices and totals
        /// </summary>      
        public Cart CalculateCartPrices(List<CartItem> cartItems, List<ProductItem> products)
        {
            UpdateCartItemsPrices(cartItems, products);           
            decimal discountedTotal = CalculateTotalDiscount(cartItems, products);

            Cart cart = new Cart() { CartItems = cartItems, Total = discountedTotal };

            return cart;
        }


        private void UpdateCartItemsPrices(List<CartItem> cartItems, List<ProductItem> products)
        {
            XElement cartXML = CreateCartXMLDocument(cartItems, products);
            XmlDocument model = LoadAndEvaluateRules(cartXML.ToString());

            // Calculate all cart items discounts
            foreach (CartItem item in cartItems)
            {
                item.DiscountedPrice = Convert.ToDecimal(model[CART_TAG][item.Product.Code][item.Product.Code + PRICE_TAG].InnerText);
            }            
        }


        private decimal CalculateTotalDiscount(List<CartItem> cartItems, List<ProductItem> products)
        {
            //Calculate Total for all items
            decimal total = cartItems.Select(x => x.DiscountedPrice).Sum();

            XElement cartXML = CreateCartXMLDocument(cartItems, products);

            cartXML.Element(TOTALS_TAG).Element(SUM_TAG).Value = total.ToString();
            cartXML.Element(TOTALS_TAG).Element(TOTAL_PRICE_TAG).Value = total.ToString();

            XmlDocument model = LoadAndEvaluateRules(cartXML.ToString());

            return Convert.ToDecimal(model[CART_TAG][TOTALS_TAG][TOTAL_PRICE_TAG].InnerText);
        }


        private XElement CreateCartXMLDocument(List<CartItem> cartItems, List<ProductItem> products)
        {
            XElement root = new XElement(CART_TAG);

            // Populate cart items nodes
            foreach (CartItem cartItem in cartItems)
            {
                XElement codeTag = new XElement(cartItem.Product.Code);
                XElement amountTag = new XElement(cartItem.Product.Code + AMOUNT_TAG, cartItem.Quantity);
                XElement priceTag = new XElement(cartItem.Product.Code + PRICE_TAG, cartItem.Product.Price);

                codeTag.Add(amountTag);
                codeTag.Add(priceTag);

                root.Add(codeTag);
            }

            // Populate products nodes
            foreach (ProductItem productItem in products.Where(p => !cartItems.Any(c => c.Product.Code == p.Code)).ToList())
            {
                XElement codeTag = new XElement(productItem.Code);
                XElement amountTag = new XElement(productItem.Code + AMOUNT_TAG, 0);
                XElement priceTag = new XElement(productItem.Code + PRICE_TAG, productItem.Price);

                codeTag.Add(amountTag);
                codeTag.Add(priceTag);

                root.Add(codeTag);
            }

            // Populate Totals node
            XElement totalsTag = new XElement(TOTALS_TAG);
            XElement sumTag = new XElement(SUM_TAG, 0);
            XElement totalPriceTag = new XElement(TOTAL_PRICE_TAG, 0);
            totalsTag.Add(sumTag);
            totalsTag.Add(totalPriceTag);
            root.Add(totalsTag);

            return root;
        }


        private XmlDocument LoadAndEvaluateRules(string xml)
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory + @"\" + XML_FILE_NAME;

            // Load rules
            XmlDocument rules = new XmlDocument();          
            rules.Load(directory);

            // Load cart model
            XmlDocument model = new XmlDocument();
            model.LoadXml(xml);

            // Evaluate rules
            ROM rom = Compiler.Compile(rules);
            rom.AddModel(CART_TAG, model);
            rom.Evaluate();

            return model;
        }
        
    }
}
