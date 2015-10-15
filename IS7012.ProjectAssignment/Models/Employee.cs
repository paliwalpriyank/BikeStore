using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace IS7012.ProjectAssignment.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Display(Name = "Employee Name")]
        [Required(ErrorMessage="Name is require")]
        public string name { get; set; }
        [Display(Name = "Date Of Birth")]
        
        public virtual DateTime dateOfBirth { get; set; }
        public string ssn;
        [Display(Name = "Total Inventories Sold")]
        public int inventorySold { get; set; }
        
        public  virtual List<Store> assignedStore { get; set; }
        [Display(Name = "Employee Type")]
        [Required(ErrorMessage = "Employee type require")]
        public string employeeType { get; set; }
        [Required(ErrorMessage = "Email is require")]
       [Display(Name = "Employee's Email")]
        public string email { get; set; }
        public Employee()
        {
            assignedStore = new List<Store>();
            dateOfBirth = DateTime.Today;
        }
        
    }
}