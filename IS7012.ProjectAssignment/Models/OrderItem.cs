using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace IS7012.ProjectAssignment.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public virtual Inventory bicOrPur { get; set; }
        [Display(Name = "Price Sold")]
        public int priceSold {get; set;}
        [Display(Name = "Online/InStore")]
        public bool flag { get; set; }
        public OrderItem()
        {

        }
        
    }
}