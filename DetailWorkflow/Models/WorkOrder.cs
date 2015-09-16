using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DetailWorkflow.Models
{
    [Authorize]
    public class WorkOrder
    {
        public int WorkOrderId { get; set; }

        [Display(Name = "Customer")]
        [Required(ErrorMessage = "You must choose a customer.")]
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        [Display(Name = "Order Date")]
        public DateTime OrderDateTime { get; set; }

        [Display(Name = "Target Date")]
        public DateTime? TargetDateTime { get; set; }

        [Display(Name = "Drop Dead Date")]
        public DateTime? DropDeadDateTime { get; set; }

        [StringLength(256, ErrorMessage = "The description must be 256 characters or shorter.")]
        public string Description { get; set; }

        [Display(Name = "Status")]
        public WorkOrderStatus WorkOrderStatus { get; set; }

        public decimal Total { get; set; }

        [Display(Name = "Certification Requirements")]
        [StringLength(120, ErrorMessage = "The certification requirements must be 120 characters or shorter.")]
        public string CertificationRequirements { get; set; }

        public virtual ApplicationUser CurrentWorker { get; set; }

        public string CurrentWorkerId { get; set; }
    }

    public enum WorkOrderStatus
    {
        Created = 0,
        InProgress = 10,
        Rework = 15,
        Submitted = 20,
        Approved = 30,
        Cancelled = -10,
        Rejected = -20
    }
}
