using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DetailWorkflow.Models
{
    public class ServiceItem
    {
        public int ServiceItemId { get; set; }
        [Required(ErrorMessage = "You must enter an item code")]
        [StringLength(15, ErrorMessage = "The Item code must be 15 characters or less")]
        [Display(Name = "Item Code")]
        public string ServiceItemCode { get; set; }
        [Required(ErrorMessage = "You must enter a name")]
        [StringLength(80, ErrorMessage = "The name must be 80 characters or less")]
        [Display(Name = "Name")]
        public string ServiceItemName { get; set; }
        [Range(typeof(decimal), "0", "7921232131232131323121312")]
        public decimal Rate { get; set; }
    }
}