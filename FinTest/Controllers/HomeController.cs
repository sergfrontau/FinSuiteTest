using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using RuleEngine;
using RuleEngine.Compiler;
using ServiceLayer;
using ServiceLayer.DTO;

namespace FinTest.Controllers
{
    public class HomeController : Controller
    {       

        public ActionResult Index()
        {
            return View();
        }     
        
    }
}