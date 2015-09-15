using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using RestSharp.Validation;

namespace DetailWorkflow.Models
{
    public class InventoryItem
    {
        public int InventoryItemId { get; set; }
        [Required(ErrorMessage = "You must enter an item code")]
        [StringLength(15, ErrorMessage = "The Item code must be 15 characters or less")]
        [Display(Name = "Item Code")]
        public string InventoryItemCode { get; set; }
        [Required(ErrorMessage = "You must enter a name")]
        [StringLength(80, ErrorMessage = "The name must be 80 characters or less")]
        [Display(Name = "Name")]
        public string InventoryItemName { get; set; }
        [Range(typeof(decimal),"0","7921232131232131323121312")]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }
        public virtual Category Category { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
    }
}