using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace IS7012.ProjectAssignment.Models
{
    public class StoreContext: DbContext
    {
        public DbSet<Employee> Employees{get; set;}
        public DbSet<Inventory> Inventories{get; set;}
        public DbSet<Order> Orders{get; set;}
        public DbSet<OrderItem> OrderItems{get; set;}
        public DbSet<Store> Stores{get; set;}
    }
}