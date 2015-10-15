using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IS7012.ProjectAssignment.Models;

namespace IS7012.ProjectAssignment.Controllers
{
    public class EmployeesController : Controller
    {
        private StoreContext storeContext = new StoreContext();

        /*
         This method returns all the list of employees and only manager can access this page.
         */
        [Authorize(Roles = "Manager")]
        public ActionResult Index()
        {
            return View(storeContext.Employees.ToList());
        }

        /*
         This method returns the details of the particular store user selected.
        GET: Employees/Details/5 */
        [Authorize(Roles = "Manager")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = storeContext.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            List<Store> storesAssigned = employee.assignedStore;
            ViewBag.assignedStores = storesAssigned;
            return View(employee);
        }
        /*
        This method return a view to create the employee.
        */
        [Authorize(Roles = "Manager")]
        public ActionResult Create()
        {
            return View();
        }

        /*
         This method accepts the form submitted fromt the front end and create a employee based on data.
         */
        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name,dateOfBirth,inventorySold,employeeType,username,password,email")] Employee employee)
        {
            List<string> employeeType = new List<string>();
            employeeType.Add("Regular");
            employeeType.Add("Manager");
            if (!employeeType.Contains(employee.employeeType))
            {
                ModelState.AddModelError("employeeType", "Allowed values are:Regular and Manger");
                return View();
            }
            if (ModelState.IsValid)
            {
                storeContext.Employees.Add(employee);
                storeContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        /*
         This method allows the manager to edit a employees detail.
         */
        [Authorize(Roles = "Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = storeContext.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        /*
         This method accepts the form submitted from the front end and edit the employees details accordingly.
         */
        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,name,dateOfBirth,inventorySold,employeeType,username,password,email")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                storeContext.Entry(employee).State = EntityState.Modified;
                storeContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        /*
         This method allows users to delete a employee from the database.
         */
        [Authorize(Roles = "Manager")]
        public ActionResult Delete(int? id)
        {
            //Fetch the employee details
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = storeContext.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        /*
         After manager confirmation employee is deleted.
         */
        [Authorize(Roles = "Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        { //when user confirms deletion delete employee from store
            Employee employee = storeContext.Employees.Find(id);
            storeContext.Employees.Remove(employee);
            storeContext.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Manager")]
        public ActionResult AssignStore(int id)
        {
            List<Store> listStores = storeContext.Stores.ToList();

            IEnumerable<SelectListItem> storeList = listStores.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.name });
            ViewBag.selectList = new SelectList(storeList, "Value", "Text");
            ViewBag.empId = id.ToString();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public ActionResult AssignStore([Bind(Include = "storeId,employeeId")] AssignStore assignStore)
        {
            int employeeId = Int32.Parse(assignStore.employeeId);
            Employee employee = storeContext.Employees.Find(employeeId);
            Store store = storeContext.Stores.Find(assignStore.storeId);
            employee.assignedStore.Add(store);
            store.employeesWorking.Add(employee);
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
    }
}
