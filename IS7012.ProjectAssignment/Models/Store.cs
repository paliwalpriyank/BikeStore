using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace IS7012.ProjectAssignment.Models
{
    public class Store
    {
        public int Id { get; set; }
        [Display(Name = "Store Name")]
        [Required(ErrorMessage = "Store Name is require")]
        public string name { get; set; }
        [Display(Name = "City")]
        public string city { get; set; }
        [Display(Name = "Address")]
        public string address { get; set; }
        [Required(ErrorMessage = "Phone Number is require")]
        [Display(Name = "Phone Number")]
        public long telephone { get; set; }
        public List<Employee> employeesWorking { get; set; }
        public Store()
        {
            employeesWorking = new List<Employee>();
        }


    }
}