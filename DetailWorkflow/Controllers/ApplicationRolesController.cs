using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DetailWorkflow.DataLayer;
using DetailWorkflow.Models;
using DetailWorkflow.ViewModels;
using Microsoft.AspNet.Identity.Owin;

namespace DetailWorkflow.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class ApplicationRolesController : Controller
    {
        public ApplicationRolesController()
        {
        }

        public ApplicationRolesController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            set { _userManager = value; }
        }

        private ApplicationRoleManager _roleManager;

        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>(); }
            set { _roleManager = value; }
        }

        // GET: ApplicationRoles
        public async Task<ActionResult> Index()
        {
            return View(await RoleManager.Roles.ToListAsync());
        }

        // GET: ApplicationRoles/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationRole applicationRole = await RoleManager.FindByIdAsync(id);
            if (applicationRole == null)
            {
                return HttpNotFound();
            }
            return View(applicationRole);
        }

        //// GET: ApplicationRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationRoles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name")] ApplicationRoleViewModel applicationRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var applicationRole = new ApplicationRole(applicationRoleViewModel.Name);
                var roleResult = await RoleManager.CreateAsync(applicationRole);
                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError("", roleResult.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }

            return View(applicationRoleViewModel);
        }

        //// GET: ApplicationRoles/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationRole applicationRole = await RoleManager.FindByIdAsync(id);
            if (applicationRole == null)
            {
                return HttpNotFound();
            }

            var applicationRoleViewModel = new ApplicationRoleViewModel { Id = applicationRole.Id, Name = applicationRole.Name };

            return View(applicationRoleViewModel);
        }

        //// POST: ApplicationRoles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] ApplicationRoleViewModel applicationRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var applicationRole = await RoleManager.FindByIdAsync(applicationRoleViewModel.Id);
                var originalName = applicationRole.Name;

                if (originalName == "Admin" && applicationRoleViewModel.Name.ToLower() != "admin")
                {
                    ModelState.AddModelError("", "You cannot change the name of Admin Role");
                    return View(applicationRoleViewModel);
                }

                if (originalName != "Admin" && applicationRoleViewModel.Name.ToLower() == "admin")
                {
                    ModelState.AddModelError("", "You cannot change the name of a role to Admin");
                    return View(applicationRoleViewModel);
                }

                applicationRole.Name = applicationRoleViewModel.Name;
                await RoleManager.UpdateAsync(applicationRole);

                return RedirectToAction("Index");
            }
            return View(applicationRoleViewModel);
        }

        //// GET: ApplicationRoles/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationRole applicationRole = await RoleManager.FindByIdAsync(id);
            if (applicationRole == null)
            {
                return HttpNotFound();
            }
            return View(applicationRole);
        }

        //// POST: ApplicationRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var applicationRole = await RoleManager.FindByIdAsync(id);

            if (applicationRole.Name == "Admin")
            {
                ModelState.AddModelError("", "You cannot delete Admin Role");
                return View(applicationRole);
            }

            await RoleManager.DeleteAsync(applicationRole);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                RoleManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
