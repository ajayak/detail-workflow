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
    public class LaborsController : Controller
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();


        public async Task<ActionResult> Index(int workOrderId)
        {
            ViewBag.WorkOrderId = workOrderId;
            var labors = _applicationDbContext.Labors
                .Where(l => l.WorkOrderId == workOrderId)
                .OrderBy(l => l.ServiceItemCode);
            return PartialView("_Index", await labors.ToListAsync());
        }


        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Labor labor = await _applicationDbContext.Labors.FindAsync(id);
            if (labor == null)
            {
                return HttpNotFound();
            }
            return View(labor);
        }


        public ActionResult Create(int workOrderId)
        {
            Labor labor = new Labor();
            labor.WorkOrderId = workOrderId;
            return PartialView("_Create", labor);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LaborId,WorkOrderId,ServiceItemCode,ServiceItemName,LaborHours,Rate,Notes")] Labor labor)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Labors.Add(labor);
                await _applicationDbContext.SaveChangesAsync();
                return Json(new { success = true });
            }

            return PartialView("_Create", labor);
        }


        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Labor labor = await _applicationDbContext.Labors.FindAsync(id);
            if (labor == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Edit", labor);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "LaborId,WorkOrderId,ServiceItemCode,ServiceItemName,LaborHours,Rate,Notes")] Labor labor)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Entry(labor).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();
                return Json(new { success = true });
            }
            return PartialView("_Edit", labor);
        }


        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Labor labor = await _applicationDbContext.Labors.FindAsync(id);
            if (labor == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete", labor);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Labor labor = await _applicationDbContext.Labors.FindAsync(id);
            _applicationDbContext.Labors.Remove(labor);
            await _applicationDbContext.SaveChangesAsync();
            return Json(new { success = true });
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
