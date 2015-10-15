using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace IS7012.ProjectAssignment.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Display(Name = "Customer Name")]
        public string custName {get; set;}
        [Display(Name = "Date Of Order")]
        public virtual DateTime orderDate {get; set;}
        [Display(Name = "Order Pickup Date")]
        public virtual DateTime pickDate {get; set;}
        
        public virtual List<OrderItem> itemsOrdered { get; set; }
        [Display(Name = "Bonus Payable")]
        public long bonusPayable {get; set;}
        
        public virtual Store store { get; set; }
        [Display(Name = "Amount Paid")]
        public int amountPaid {get; set;}
        [Display(Name = "Payment Method")]
        public string paymentMethod { get; set; }
        [Display(Name = "Is Order Placed")]
        public bool placeOrder { get; set; }
        public virtual Employee employee { get; set; }
        public Order()
        {
            itemsOrdered = new List<OrderItem>();
            
            orderDate = DateTime.Today;
            pickDate = DateTime.Today;
        }
        public void SetPayMethod(string payMethod)
        {
            if (payMethod.Equals("cash") || payMethod.Equals("check") || payMethod.Equals("credit card"))
            {
                this.paymentMethod = payMethod;
            }
            else
            {
                throw new ArgumentException(" value should be in cash, check, credit card");
            }
        }
        public string GetPaymentMethod()
        {
            return this.paymentMethod;
        }
                
    }
}