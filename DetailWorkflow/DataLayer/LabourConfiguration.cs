using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using DetailWorkflow.Models;

namespace DetailWorkflow.DataLayer
{
    public class LabourConfiguration : EntityTypeConfiguration<Labor>
    {
        public LabourConfiguration()
        {
            Property(p => p.ServiceItemCode)
                .HasMaxLength(15)
                .IsRequired()
                .HasColumnAnnotation("Index",
                new IndexAnnotation(new IndexAttribute("AK_Labour", 2) { IsUnique = true }));

            Property(p => p.WorkOrderId)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("AK_Labour", 1) { IsUnique = true }));

            Property(p => p.ServiceItemName)
                .HasMaxLength(80)
                .IsRequired();

            Property(p => p.LabourHours)
                .HasPrecision(18, 2);

            Property(p => p.Rate)
               .HasPrecision(18, 2);

            Property(p => p.ExtendedPrice)
                .HasPrecision(18, 2)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            Property(p => p.Notes)
                .HasMaxLength(140)
                .IsOptional();
        }
    }
}