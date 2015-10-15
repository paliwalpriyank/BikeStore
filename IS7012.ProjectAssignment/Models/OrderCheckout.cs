using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IS7012.ProjectAssignment.Models
{
    public class OrderCheckout
    {
        [Required(ErrorMessage = "Amount Paid is required")]
        [Display(Name = "Amount Paid")]
        public int amountPaid { get; set; }
        public int orderId { get; set; }
        public OrderCheckout()
        {

        }

    }
}