using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TreeUtility;

namespace DetailWorkflow.Models
{
    public class Category : ITreeNode<Category>
    {
        // For production use data annotations in view model
        public int Id { get; set; }
        [Display(Name = "Parent Category")]
        public int? ParentCategoryId { get; set; }
        public virtual Category Parent { get; set; }
        public IList<Category> Children { get; set; }
        [Required(ErrorMessage = "You must enter a category name")]
        [StringLength(20, ErrorMessage = "Category name must be 20 characters or less")]
        [Display(Name = "Category")]
        public string CategoryName { get; set; }
        public virtual List<InventoryItem> InventoryItems { get; set; }
    }
}