using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IS7012.ProjectAssignment.Models;

namespace IS7012.ProjectAssignment.Controllers
{
    public class ManagerController : Controller
    {
        private StoreContext db = new StoreContext();
        /*
         This method returns the view which contains various links i.e. sales data, manage employee and 
         * manage stores.
         */

        [Authorize(Roles = "Manager")]
        public ActionResult Dashboard()
        {
            return View();
        }
       
        /*
         * this method groups the order data based on store and then this data is displayed on front end.
         */
        [Authorize(Roles = "Manager")]
        public ActionResult Index()
        {

            List<Order> or = db.Orders.Where(ord=>ord.placeOrder==true).ToList();
            //this creates the group based on stores
            IEnumerable<IGrouping<Store, Order>> ie = or.GroupBy(ord => ord.store, ord => ord);
            //these dictionaries contains data for various parameters for every stores.
            Dictionary<Store, int> totalSales = new Dictionary<Store, int>();
            Dictionary<Store, int> totalSalesDollars = new Dictionary<Store, int>();
            Dictionary<Store, long> employeeBonus = new Dictionary<Store, long>();
            Dictionary<Store, int> totalSalesWeek = new Dictionary<Store, int>();
            Dictionary<Store, int> totalSalesDollarWeek = new Dictionary<Store, int>();
            Dictionary<Store, long> employeeBonusWeek = new Dictionary<Store, long>();
            int totalProfit = 0;
            int totalProfitWeek = 0;
            //iterate over the loop and calculate and update the above lists.
            foreach (IGrouping<Store, Order> group in ie)
            {
                int totalsales = 0;
                int totalSalesDol = 0;
                long empBonus = 0;
                int totSalesWeek = 0;
                int totSalesDollarWeek = 0;
                long empBonusWeek = 0;
                            
                foreach(Order orders in group)
                {
                    if (orders.orderDate.Equals(DateTime.Today))
                    {
                        totalsales += 1;
                        totalSalesDol += orders.amountPaid;
                        empBonus += orders.bonusPayable;
                        totalProfit += orders.amountPaid;
                        foreach(OrderItem OI in (orders.itemsOrdered))
                        {
                            totalProfit -= OI.bicOrPur.cost;
                        }

                    }
                    if (orders.orderDate > DateTime.Today.AddDays(-7))
                    {
                        totSalesWeek += 1;
                        totSalesDollarWeek += orders.amountPaid;
                        empBonusWeek += orders.bonusPayable;
                        totalProfitWeek += orders.amountPaid;
                        foreach (OrderItem OI in (orders.itemsOrdered))
                        {
                            totalProfitWeek -= OI.bicOrPur.cost;
                        }

                    }
                    
                }
                totalSales.Add(group.Key, totalsales);
                totalSalesDollars.Add(group.Key, totalSalesDol);
                employeeBonus.Add(group.Key, empBonus);
                totalSalesWeek.Add(group.Key, totSalesWeek);
                totalSalesDollarWeek.Add(group.Key, totSalesDollarWeek);
                employeeBonusWeek.Add(group.Key, empBonusWeek);
            }
            //setup the viewbag to display data on front end.
            ViewBag.salesData = totalSales;
            ViewBag.salesDataDollar = totalSalesDollars;
            ViewBag.emBonus = employeeBonus;
            ViewBag.tosaWeek = totalSalesWeek;
            ViewBag.todoWeek = totalSalesDollarWeek;
            ViewBag.emBonusWeek = employeeBonusWeek;
            ViewBag.totalProfitWeek = totalProfitWeek;
            ViewBag.totalProfit = totalProfit;

            
            return View();
        }
        
    }
}