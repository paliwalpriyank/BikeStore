using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IS7012.ProjectAssignment.Models;
using Microsoft.AspNet.Identity;


namespace IS7012.ProjectAssignment.Controllers
{
    public class OrdersController : Controller
    {
        private StoreContext db = new StoreContext();
        private int ordid;

        // GET: Orders
        [Authorize(Roles="Employee, Manager")]
       //this method returns the list of orders for the employee who is logged in.
        public ActionResult Index()
        {
            string username = User.Identity.GetUserName();
            List<Order> or = db.Orders.ToList();
            Employee employee = db.Employees.FirstOrDefault(x => x.email == username);
                List<Store> ls = employee.assignedStore;
                int[] i = new int[20];
                int count = 0;
                
                foreach (Store s in ls)
                {
                    i[count] = s.Id;
                    count++;
                }
                var filterlist = (from e in or where i.Contains(e.itemsOrdered.First().bicOrPur.store.Id) select e);
                return View(filterlist);
            
            
        }
        //this will return the form to take amount paid 
        [Authorize(Roles = "Employee, Manager")]   
        public ActionResult PlaceOrder(int id)
        {
            TempData["ordId"] = id;
            ViewBag.ordId = id;
            TempData.Keep("ordId");
            ordid = id;
            
            
            return View("PlaceOrderForm");
           
        }
        //this will accepts the form and then update the customer name and place the order.
        [Authorize(Roles = "Employee, Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder([Bind(Include = "amountPaid,orderId")]OrderCheckout of)        
        {
            Order or = db.Orders.Find(of.orderId);
            int amountAllowed = 0;
            foreach(OrderItem oi in or.itemsOrdered)
            {
                amountAllowed += oi.bicOrPur.cost;
            }
            if ( (User.IsInRole("Manager") && of.amountPaid < amountAllowed*0.75) || (User.IsInRole("Employee") && of.amountPaid < amountAllowed*0.90))
            {
                ModelState.AddModelError("amountPaid", "You are not allowed to give that much discount");
                return View("PlaceOrderForm");
            }
            var i = ordid;
           
            string username = User.Identity.GetUserName();
            Employee employee = db.Employees.FirstOrDefault(x => x.email == username);
          
            or.employee = employee;
            
            or.placeOrder = true;
            or.paymentMethod = "cash";
            or.amountPaid = of.amountPaid;
            or.bonusPayable = 5;
            or.store=or.itemsOrdered.First().bicOrPur.store;
            
            foreach(OrderItem OI in(or.itemsOrdered))
            {
                if(OI.flag)
                {
                    or.bonusPayable += 50;
                    break; 
                }
                OI.bicOrPur.IsBooked = "ordered";
            }
            db.SaveChanges();
            return View();
        }


        //this will return the view of order details including order items in order
        [Authorize(Roles = "Employee, Manager")]
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.orderItemList = order.itemsOrdered;
            return View(order);
        }

        
        //edit the selected order 
        // GET: Orders/Edit/5
        [Authorize(Roles = "Employee, Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.ordid = id;
            Session["ordId"] = id;
            return View(order);
        }

        //update the order
        [Authorize(Roles = "Employee, Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,custName,bonusPayable,amountPaid")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        //delete the order
        [Authorize(Roles = "Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            
            return View(order);
        }

        //order will  be deleted from the database.
        [Authorize(Roles = "Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            List<OrderItem> orderItemToDelete= new List<OrderItem>();
            foreach(OrderItem OI in (order.itemsOrdered))
            {
                orderItemToDelete.Add(OI);
            }
            foreach(OrderItem OI  in (orderItemToDelete))
            {
                OI.bicOrPur.IsBooked = "";

            }
            order.itemsOrdered = new List<OrderItem>();
            db.Orders.Remove(order);
            
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
