using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IS7012.ProjectAssignment.Models;

namespace IS7012.ProjectAssignment.Controllers
{
    public class HomeController : Controller
    {
        private StoreContext storeContext = new StoreContext();
        /*
          this method return the view for the home page of the application
         */
        public ActionResult Index()
        {
           
            return View();
        }
        /*
         this method return the contact information for every store
        */
        public ActionResult Contact()
        {
            List<Store> store = storeContext.Stores.ToList();
            List<Inventory> inventory = storeContext.Inventories.ToList();
            ViewBag.Message = "Store Information";
            ViewBag.storeList = store;

            return View();
        }

        /*
           this method renders testimonial of various users.
          */
        public ActionResult Testimonials()
        {
            ViewBag.Message = "Your Testimonials page.";

            return View();
        }
        /*
          this method renders the about page of the chain of stores.
         */
        public ActionResult About()
        {
            ViewBag.Message = "The bike store is actually a chain of three physical stores in three different cities. Each store keeps its own inventory of bicycles and has a separate staff (although sometimes staff substitute or transfer to one of the other stores). The owner hopes to expand his stores and would like the ability to add additional stores if needed.";

            return View();
        }
        
    }
}