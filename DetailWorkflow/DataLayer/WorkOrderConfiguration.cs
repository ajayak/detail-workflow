using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using DetailWorkflow.Models;

namespace DetailWorkflow.DataLayer
{
    public class WorkOrderConfiguration:EntityTypeConfiguration<WorkOrder>
    {
        public WorkOrderConfiguration()
        {
            Property(wo => wo.OrderDateTime).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            Property(wo => wo.Description).HasMaxLength(250).IsOptional();

            Property(wo => wo.Total).HasPrecision(18, 2).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            Property(wo => wo.CertificationRequirements).HasMaxLength(120).IsOptional();

            HasRequired(wo => wo.CurrentWorker);
        }
    }
}