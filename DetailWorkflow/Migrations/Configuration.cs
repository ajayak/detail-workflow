using DetailWorkflow.DataLayer;
using DetailWorkflow.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DetailWorkflow.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false
            };

            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new ApplicationDbContext()));

            var name = "a@a.com";
            var password = "ak1111";
            var firstName = "Ajay";
            var roleName = "Admin";

            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationRole(roleName);
                var roleResult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = name,
                    Email = name,
                    FirstName = firstName
                };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }

            string accountNumber = "ABC123";

            context.Customers.AddOrUpdate(
                c => c.AccountNumber,
                new Customer
                {
                    AccountNumber = accountNumber,
                    CompanyName = "ABC Company of America",
                    Address = "123 Main St.",
                    City = "Anytown",
                    State = "GA",
                    ZipCode = "30071"
                });

            context.SaveChanges();

            Customer customer = context.Customers.First(c => c.AccountNumber == accountNumber);

            string description = "Just another work order";

            context.WorkOrders.AddOrUpdate(
                wo => wo.Description,
                new WorkOrder { Description = description, CustomerId = customer.CustomerId, WorkOrderStatus = WorkOrderStatus.Created });

            context.SaveChanges();

            WorkOrder workOrder = context.WorkOrders.First(wo => wo.Description == description);

            context.Parts.AddOrUpdate(
                p => p.InventoryItemCode,
                new Part { InventoryItemCode = "THING1", InventoryItemName = "Thing Number One", Quantity = 1, UnitPrice = 1.23m, WorkOrderId = workOrder.WorkOrderId });

            context.Labors.AddOrUpdate(
                l => l.ServiceItemCode,
                new Labor { ServiceItemCode = "INSTALL", ServiceItemName = "Installation", LabourHours = 9.87m, Rate = 35.75m, WorkOrderId = workOrder.WorkOrderId });

            string categoryName = "Devices";

            context.Categories.AddOrUpdate(
                c => c.CategoryName,
                new Category { CategoryName = categoryName });

            context.SaveChanges();

            Category category = context.Categories.First(c => c.CategoryName == categoryName);

            context.InventoryItems.AddOrUpdate(
                ii => ii.InventoryItemCode,
                new InventoryItem { InventoryItemCode = "THING2", InventoryItemName = "A Second Kind of Thing", UnitPrice = 3.33m, CategoryId = category.CategoryId });

            context.ServiceItems.AddOrUpdate(
                si => si.ServiceItemCode,
                new ServiceItem { ServiceItemCode = "CLEAN", ServiceItemName = "General Cleaning", Rate = 23.50m });
        }
    }
}
