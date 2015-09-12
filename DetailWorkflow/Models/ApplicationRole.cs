﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DetailWorkflow.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {

        }

        public ApplicationRole(string name)
            : base(name)
        {

        }
    }
}