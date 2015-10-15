using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace IS7012.ProjectAssignment.Models
{
    public class StoreDbConfiguration: DropCreateDatabaseIfModelChanges<StoreContext>
    {
        protected override void Seed(StoreContext dbContext)
        {
            Store s1=new Store { address = "Main Street, Cincinnati OH-45221", city = "Cincinnati", telephone = 1234567899, name = "Store1" };
            Store s2 = new Store { address = "Calhoun, Cincinnati OH-45222", city = "Cincinnati", telephone = 1234567898, name = "Store2" };
            Store s3 = new Store { address = "Martin Luther King Dr., Cincinnati OH-45223", city = "Cincinnati", telephone = 1234567897, name = "Store3" };
            
            Inventory inv1 = new Inventory { frameSize = "Sm", bikeType = "Mountain Bike", brand = "brand1", model = "model", description = "light weight", cost = 500, recSellingPrice = 200 };
            Inventory inv2 = new Inventory { frameSize = "Sm", bikeType = "Mountain Bike", brand = "brand2", model = "model", description = "light weight", cost = 500, recSellingPrice = 200 };
            Inventory inv3 = new Inventory { frameSize = "Sm", bikeType = "Mountain Bike", brand = "brand3", model = "model", description = "light weight", cost = 500, recSellingPrice = 200 };
            Inventory inv4 = new Inventory { frameSize = "Sm", bikeType = "Mountain Bike", brand = "brand1", model = "model1", description = "light weight", cost = 500, recSellingPrice = 200 };
            Inventory inv5 = new Inventory { frameSize = "Sm", bikeType = "Mountain Bike", brand = "brand2", model = "model1", description = "light weight", cost = 500, recSellingPrice = 200 };
            Inventory inv6 = new Inventory { frameSize = "Sm", bikeType = "Mountain Bike", brand = "brand3", model = "model1", description = "light weight", cost = 500, recSellingPrice = 200 };
            inv1.store = s1;
            inv2.store = s2;
            inv3.store = s3;
            inv4.store = s1;
            inv5.store = s2;
            inv6.store = s3;


            Employee Employee1 = new Employee { name = "Priyank", employeeType = "regular", email = "employee1@example.com" };
            Employee Employee2 = new Employee { name = "Rohit", employeeType = "regular", email = "employee2@example.com" };
            Employee Employee3 = new Employee { name = "Nirav", employeeType = "regular", email = "employee3@example.com" };
            Employee Employee4 = new Employee { name = "Manager", employeeType = "manager", email = "manager@example.com" };



            Employee1.assignedStore.Add(s1);
            Employee2.assignedStore.Add(s2);
            Employee3.assignedStore.Add(s3);
            Employee4.assignedStore.Add(s1);

            s1.employeesWorking.Add(Employee1);
            s2.employeesWorking.Add(Employee2);
            s3.employeesWorking.Add(Employee3);
            s1.employeesWorking.Add(Employee4);

            dbContext.Stores.Add(s1);
            dbContext.Stores.Add(s2);
            dbContext.Stores.Add(s3);

            dbContext.Inventories.Add(inv1);
            dbContext.Inventories.Add(inv2);
            dbContext.Inventories.Add(inv3);

            dbContext.Employees.Add(Employee1);
            dbContext.Employees.Add(Employee2);
            dbContext.Employees.Add(Employee3);
            dbContext.Employees.Add(Employee4);

            dbContext.SaveChanges();
        }
    }
}