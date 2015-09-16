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

namespace DetailWorkflow.Controllers
{
    public class PartsController : Controller
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();

        // GET: Parts
        public async Task<ActionResult> Index(int workOrderId)
        {
            ViewBag.WorkOrderId = workOrderId;
            var parts = _applicationDbContext.Parts
                .Where(p => p.WorkOrderId == workOrderId)
                .OrderBy(p => p.InventoryItemCode);

            return PartialView("_Index", await parts.ToListAsync());
        }

        // GET: Parts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Part part = await _applicationDbContext.Parts.FindAsync(id);
            if (part == null)
            {
                return HttpNotFound();
            }
            return View(part);
        }

        // GET: Parts/Create
        public ActionResult Create()
        {
            ViewBag.WorkOrderId = new SelectList(_applicationDbContext.WorkOrders, "WorkOrderId", "Description");
            return View();
        }

        // POST: Parts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PartId,WorkOrderId,InventoryItemCode,InventoryItemName,Quantity,UnitPrice,ExtendedPrice,Notes,IsInstalled")] Part part)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Parts.Add(part);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.WorkOrderId = new SelectList(_applicationDbContext.WorkOrders, "WorkOrderId", "Description", part.WorkOrderId);
            return View(part);
        }

        // GET: Parts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Part part = await _applicationDbContext.Parts.FindAsync(id);
            if (part == null)
            {
                return HttpNotFound();
            }
            ViewBag.WorkOrderId = new SelectList(_applicationDbContext.WorkOrders, "WorkOrderId", "Description", part.WorkOrderId);
            return View(part);
        }

        // POST: Parts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PartId,WorkOrderId,InventoryItemCode,InventoryItemName,Quantity,UnitPrice,ExtendedPrice,Notes,IsInstalled")] Part part)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Entry(part).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.WorkOrderId = new SelectList(_applicationDbContext.WorkOrders, "WorkOrderId", "Description", part.WorkOrderId);
            return View(part);
        }

        // GET: Parts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Part part = await _applicationDbContext.Parts.FindAsync(id);
            if (part == null)
            {
                return HttpNotFound();
            }
            return View(part);
        }

        // POST: Parts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Part part = await _applicationDbContext.Parts.FindAsync(id);
            _applicationDbContext.Parts.Remove(part);
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
