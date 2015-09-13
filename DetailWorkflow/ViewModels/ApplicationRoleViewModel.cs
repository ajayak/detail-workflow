using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DetailWorkflow.ViewModels
{
    public class ApplicationRoleViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings=false, ErrorMessage = "You must enter a name for role")]
        [StringLength(256, ErrorMessage = "The role must be 256 characters or less")]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }
}