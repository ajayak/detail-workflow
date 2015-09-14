using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TreeUtility;

namespace DetailWorkflow.Models
{
    public class Category : ITreeNode<Category>
    {
        public int Id { get; set; }
        public int? ParentCategoryId { get; set; }
        public Category Parent { get; set; }
        public IList<Category> Children { get; set; }
        public string CategoryName { get; set; }
        public virtual List<InventoryItem> InventoryItems { get; set; }
    }
}