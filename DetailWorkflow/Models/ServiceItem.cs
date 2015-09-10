using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DetailWorkflow.Models
{
    public class ServiceItem
    {
        public int ServiceItemId { get; set; }
        public string ServiceItemCode { get; set; }
        public string ServiceItemName { get; set; }
        public decimal Rate { get; set; }
    }
}