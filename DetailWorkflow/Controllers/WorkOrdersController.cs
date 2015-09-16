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
using Microsoft.AspNet.Identity;

namespace DetailWorkflow.Controllers
{
    public class WorkOrdersController : Controller
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();

        // GET: WorkOrders
        public async Task<ActionResult> Index()
        {
            var workOrders = _applicationDbContext.WorkOrders.Include(w => w.CurrentWorker).Include(w => w.Customer);
            return View(await workOrders.ToListAsync());
        }

        // GET: WorkOrders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkOrder workOrder = await _applicationDbContext.WorkOrders.FindAsync(id);
            if (workOrder == null)
            {
                return HttpNotFound();
            }
            return View(workOrder);
        }

        // GET: WorkOrders/Create
        public ActionResult Create()
        {
            //ViewBag.CurrentWorkerId = new SelectList(_applicationDbContext.ApplicationUsers, "Id", "FirstName");
            ViewBag.CustomerId = new SelectList(_applicationDbContext.Customers, "CustomerId", "AccountNumber");
            return View();
        }

        // POST: WorkOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "WorkOrderId,CustomerId,OrderDateTime,TargetDateTime,DropDeadDateTime,Description,WorkOrderStatus,Total,CertificationRequirements,CurrentWorkerId")] WorkOrder workOrder)
        {
            if (ModelState.IsValid)
            {
                workOrder.CurrentWorkerId = User.Identity.GetUserId();
                _applicationDbContext.WorkOrders.Add(workOrder);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Edit", new { controller = "WorkOrders", action = "Edit", Id = workOrder.WorkOrderId });
            }

            //ViewBag.CurrentWorkerId = new SelectList(_applicationDbContext.ApplicationUsers, "Id", "FirstName", workOrder.CurrentWorkerId);
            ViewBag.CustomerId = new SelectList(_applicationDbContext.Customers, "CustomerId", "AccountNumber", workOrder.CustomerId);
            return View(workOrder);
        }

        // GET: WorkOrders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkOrder workOrder = await _applicationDbContext.WorkOrders.FindAsync(id);
            if (workOrder == null)
            {
                return HttpNotFound();
            }
            return View(workOrder);
        }

        // POST: WorkOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "WorkOrderId,CustomerId,OrderDateTime,TargetDateTime,DropDeadDateTime,Description,WorkOrderStatus,Total,CertificationRequirements,CurrentWorkerId")] WorkOrder workOrder)
        {
            if (ModelState.IsValid)
            {
                workOrder.CurrentWorkerId = User.Identity.GetUserId();
                _applicationDbContext.Entry(workOrder).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //ViewBag.CurrentWorkerId = new SelectList(_applicationDbContext.ApplicationUsers, "Id", "FirstName", workOrder.CurrentWorkerId);
            ViewBag.CustomerId = new SelectList(_applicationDbContext.Customers, "CustomerId", "AccountNumber", workOrder.CustomerId);
            return View(workOrder);
        }

        // GET: WorkOrders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkOrder workOrder = await _applicationDbContext.WorkOrders.FindAsync(id);
            if (workOrder == null)
            {
                return HttpNotFound();
            }
            return View(workOrder);
        }

        // POST: WorkOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            WorkOrder workOrder = await _applicationDbContext.WorkOrders.FindAsync(id);
            _applicationDbContext.WorkOrders.Remove(workOrder);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _applicationDbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
