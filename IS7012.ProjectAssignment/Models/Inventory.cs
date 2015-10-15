using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace IS7012.ProjectAssignment.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        [Display(Name = "Frame Size")]
        [Required(ErrorMessage = "Frame Size is require")]
        public string frameSize{get;set;}
        [Display(Name = "Bike Type")]
        [Required(ErrorMessage = "Bike Type is require")]
        public string bikeType { get; set; }
        [Display(Name = "Bike Brand")]
        [Required(ErrorMessage = "Brand is require")]
        public string brand {get; set;}
        [Display(Name = "Bike Model")]
        public string model {get; set;}
        [Display(Name = "Description For Bike")]
        public string description {get; set;}
        [Display(Name = "Cost Of Bike")]
        [Required(ErrorMessage = "Cost is require")]
        public int cost {get; set;}
        [Display(Name = "Reselling Price")]
        public int recSellingPrice { get; set; }
        public virtual Store store {get; set;}
        [Display(Name = "Bike Status")]
        public string IsBooked { get; set; }
        public Inventory()
        {

        }
        public void SetFrameSize(string framSize)
        {
            if(framSize.Equals("Sm")||framSize.Equals("Md")||framSize.Equals("Lg")||framSize.Equals("XtraLg"))
            {
                this.frameSize = framSize;
            }
            else
            {
                throw new ArgumentException(" value should be in Sm, Md, Lg, XtraLg");
            }
        }
        public string GetFrameSize()
        {
            return this.frameSize;
        }

        public void SetBikeType(string bikeTyp)
        {
            if (bikeTyp.Equals("BMX") || bikeTyp.Equals("Mountain Bike") || bikeTyp.Equals("Hybrid") || bikeTyp.Equals("Road") || bikeTyp.Equals("Children’s"))
            {
                this.bikeType = bikeTyp;
            }
            else
            {
                throw new ArgumentException(" value should be in BMX, Mountain Bike, Hybrid, Road, Children’s");
            }
        }
        public string GetBikeType()
        {
            return this.bikeType;
        }
        
    }
}