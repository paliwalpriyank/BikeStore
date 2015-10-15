using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace IS7012.ProjectAssignment.Models
{
    public class AssignStore
    {
        [Required(ErrorMessage = "Customer Name is required")]
        [Display(Name="Store Name")]
        public int storeId { get;set;}
        public string inventoryId { get; set; }
        public string employeeId { get; set; }
        public AssignStore()
        {

        }
    }
}