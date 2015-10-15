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
    public class InventoriesController : Controller
    {
        private StoreContext storeContext = new StoreContext();


        /*
          This method displays the inventory available. For employee and manager corresponding store
         * inventory is displayed.
         * 
         */
        public ActionResult Index()
        {

            List<Inventory> inventory = storeContext.Inventories.Where(inven => inven.IsBooked != "ordered").ToList();
            string loginUserName = User.Identity.GetUserName();
            if (loginUserName != null && !loginUserName.Equals("") )
            {
                Employee employee = storeContext.Employees.FirstOrDefault(x => x.email == loginUserName);
                List<Store> assignedStoreList = employee.assignedStore;
                int[] storeIdArray = new int[20];
                int count = 0;

                foreach (Store store in assignedStoreList)
                {
                    storeIdArray[count] = store.Id;
                    count++;
                }
                var refinedlist = (from inv in inventory where storeIdArray.Contains(inv.store.Id) select inv);
                return View(refinedlist);
            }
            return View(inventory);
        }
        
        
        /*
         * this method displays the details lf an inventory.
         */
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = storeContext.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        /*
         * this method returns the view to create a new inventory.
         */
        [Authorize(Roles = "Manager")]
        public ActionResult Create()
        {
            return View();
        }

        /*
         This method accepts the form submitted for a new inventory and create a new inventory.
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public ActionResult Create([Bind(Include = "Id,frameSize,bikeType,brand,model,description,cost,recSellingPrice")] Inventory inventory, [Bind (Include="storeAssigned")]IEnumerable<SelectListItem> collection)
        {
            if (ModelState.IsValid)
            {
                List<string> allowedFrames= new List<string>();
                allowedFrames.Add("Sm");
                allowedFrames.Add("Md");
                allowedFrames.Add("Lg");
                allowedFrames.Add("XtraLg");
                List<string> allowedBikeTypes = new List<string>();
                allowedBikeTypes.Add("BMX");
                allowedBikeTypes.Add("Mountain Bike");
                allowedBikeTypes.Add("Hybrid");
                allowedBikeTypes.Add("Road");
                allowedBikeTypes.Add("Children’s");
                if (!allowedFrames.Contains(inventory.frameSize))
                {
                    ModelState.AddModelError("frameSize", "Allowed values are: Sm, Md, Lg, XtraLg");
                    return View("Create");
                }
                if (!allowedBikeTypes.Contains(inventory.bikeType))
                {
                    ModelState.AddModelError("bikeType", "Allowed values are: BMX, Mountain Bike, Hybrid, Road, Children’s");
                    return View("Create");
                }

                storeContext.Inventories.Add(inventory);
                storeContext.SaveChanges();
                Session["invId"] = inventory.Id.ToString();              
                return RedirectToAction("AssignStore");
            }

            return View(inventory);
        }

        /*
         * this method allows the manager to assign a store to the inventory this step comes after user has created
         * an inventory.
         */
         [Authorize(Roles = "Manager")]
        public ActionResult AssignStore()
        {
            List<Store> listStores = storeContext.Stores.ToList();

            IEnumerable<SelectListItem> storeList = listStores.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.name });
            ViewBag.selectList = new SelectList(storeList, "Value", "Text");
            ViewBag.invId = Session["invId"] as string;
            return View();
        }

        /*
         * this method takes the form submitted from the front end and assign the manager into db.
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public ActionResult AssignStore([Bind(Include = "storeId,inventoryId")] AssignStore assignStore)
        {
            int inventoryId = Int32.Parse(assignStore.inventoryId);
            Inventory inventory = storeContext.Inventories.Find(inventoryId);
            Store store= storeContext.Stores.Find(assignStore.storeId);
            inventory.store = store;
            storeContext.SaveChanges();
            return RedirectToAction("Index");
        }
        /*
         * this method allows the manager to edit the inventory.
         * 
         */
        [Authorize(Roles = "Manager")]
        // GET: Inventories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = storeContext.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        /*
         * this method takes form submitted from the front end and change inventory accordingly.
         * 
         */
        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,frameSize,bikeType,brand,model,description,cost,recSellingPrice")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                storeContext.Entry(inventory).State = EntityState.Modified;
                storeContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inventory);
        }
        /*
         * this method allows manager to delete an inventory.
         */
        [Authorize(Roles = "Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = storeContext.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        /*
         * after confirmation from the manager this method deletes an inventory.
         */
        [Authorize(Roles = "Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inventory inventory = storeContext.Inventories.Find(id);
            storeContext.Inventories.Remove(inventory);
            storeContext.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                storeContext.Dispose();
            }
            base.Dispose(disposing);
        }
        /*
          This method allows employees and manager to update the existing order.
         * 
         */
        [Authorize(Roles = "Employee, Manager")]
        public ActionResult UpdateOrder(string id)
        {
            ViewBag.ordid = id.ToString();
            List<Inventory> inventory = storeContext.Inventories.ToList();
            string username = User.Identity.GetUserName();
            if (username != null && !username.Equals(""))
            {
                Employee employee = storeContext.Employees.FirstOrDefault(x => x.email == username);
                List<Store> assignedStoreList = employee.assignedStore;
                int[] storeIdArray = new int[20];
                int count = 0;
                foreach (Store store in assignedStoreList)
                {
                    storeIdArray[count] = store.Id;
                    count++;
                }
                var refinedlist = (from inv in inventory where storeIdArray.Contains(inv.store.Id) select inv);
                return View(refinedlist);
            }

            return View(inventory);
        }
        /*
         * this method allows user to book an inventory. 
         */
        public ActionResult Reserve(string id)
        {

            Session["InvId"] = id;
            return View();

        }

        /*
         * this method takes customer name provided by the user and update the order.
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reserve([Bind(Include = "CustomerName")]OrderForm of)
        {
            var id = Session["orderId"] as string;
            
            Order order;
            if (id == null)
            {
                order = new Order();
            }
            else
            {
                order = storeContext.Orders.First(x => x.Id == Convert.ToInt32(id));
            }
            OrderItem OI = new OrderItem();
            string s=Session["InvId"] as string;
            var ident=User.Identity;
            int invID=Int32.Parse(s);
            Inventory inventory = storeContext.Inventories.FirstOrDefault(x => x.Id == invID);
            OI.bicOrPur = inventory;
            inventory.IsBooked="booked";
            order.custName = of.CustomerName;
            order.itemsOrdered.Add(OI);
            storeContext.OrderItems.Add(OI);
            storeContext.Orders.Add(order);
            storeContext.SaveChanges();
            Session["orderId"] = order.Id;
            return View("Reserved");
        }
        /*
          this method update the existing order with the inventory selected at the front end.
         * 
         */
        [Authorize(Roles = "Employee, Manager")]
        public ActionResult UpdateExistingOrder(string id, string ordid)
        {
            if (ordid != null)
            {
                int orderId = Int32.Parse(ordid);
                int inventoryId = Int32.Parse(id);
                Order order = storeContext.Orders.Find(orderId);
                Inventory inventory = storeContext.Inventories.Find(inventoryId);
                OrderItem orderedItem = new OrderItem();
                orderedItem.bicOrPur = inventory;
                orderedItem.flag = true;
                orderedItem.bicOrPur.IsBooked = "booked";
                order.itemsOrdered.Add(orderedItem);
                storeContext.OrderItems.Add(orderedItem);
                storeContext.SaveChanges();
            }
            return View();
        }
    }
}
