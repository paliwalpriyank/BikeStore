using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IS7012.ProjectAssignment.Models
{
    public class OrderForm
    {
        [Required(ErrorMessage = "Customer Name is required")]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        
      
        public void Reset()
        {
            CustomerName = "";
        }
    }
}