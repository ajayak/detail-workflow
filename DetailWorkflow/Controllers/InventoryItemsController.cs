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
    public class InventoryItemsController : Controller
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();

        // GET: InventoryItems
        public async Task<ActionResult> Index(string sort, string search)
        {
            ViewBag.CategorySort = String.IsNullOrEmpty(sort) ? "category_desc" : String.Empty;
            ViewBag.ItemCodeSort = sort == "itemcode" ? "itemcode_desc" : "itemcode";
            ViewBag.NameSort = sort == "name" ? "name_desc" : "name";
            ViewBag.UnitPriceSort = sort == "unitprice" ? "unitprice_desc" : "unitprice";

            IQueryable<InventoryItem> inventoryItems = _applicationDbContext.InventoryItems.Include(i => i.Category);

            if (!string.IsNullOrEmpty(search))
            {
                inventoryItems = inventoryItems
                    .Where(ii => ii.InventoryItemCode.StartsWith(search) ||
                                 ii.InventoryItemName.StartsWith(search));
            }

            switch (sort)
            {
                case "category_desc":
                    inventoryItems = inventoryItems
                        .OrderByDescending(ii => ii.Category.CategoryName)
                        .ThenBy(ii => ii.InventoryItemName);
                    break;

                case "itemcode":
                    inventoryItems = inventoryItems
                        .OrderBy(ii => ii.InventoryItemCode);
                    break;

                case "itemcode_desc":
                    inventoryItems = inventoryItems
                        .OrderByDescending(ii => ii.InventoryItemCode);
                    break;

                case "name":
                    inventoryItems = inventoryItems
                        .OrderBy(ii => ii.InventoryItemName);
                    break;

                case "name_desc":
                    inventoryItems = inventoryItems
                        .OrderByDescending(ii => ii.InventoryItemName);
                    break;

                case "unitprice":
                    inventoryItems = inventoryItems
                        .OrderBy(ii => ii.UnitPrice);
                    break;

                case "unitprice_desc":
                    inventoryItems = inventoryItems
                        .OrderByDescending(ii => ii.UnitPrice);
                    break;

                default:
                    inventoryItems = inventoryItems
                        .OrderBy(ii => ii.Category.CategoryName)
                        .ThenBy(ii => ii.InventoryItemName);
                    break;
            }

            return View(await inventoryItems.ToListAsync());
        }

        // GET: InventoryItems/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryItem inventoryItem = await _applicationDbContext.InventoryItems.FindAsync(id);
            if (inventoryItem == null)
            {
                return HttpNotFound();
            }
            return View(inventoryItem);
        }

        // GET: InventoryItems/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_applicationDbContext.Categories, "Id", "CategoryName");
            return View();
        }

        // POST: InventoryItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "InventoryItemId,InventoryItemCode,InventoryItemName,UnitPrice,CategoryId")] InventoryItem inventoryItem)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.InventoryItems.Add(inventoryItem);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(_applicationDbContext.Categories, "Id", "CategoryName", inventoryItem.CategoryId);
            return View(inventoryItem);
        }

        // GET: InventoryItems/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryItem inventoryItem = await _applicationDbContext.InventoryItems.FindAsync(id);
            if (inventoryItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(_applicationDbContext.Categories, "Id", "CategoryName", inventoryItem.CategoryId);
            return View(inventoryItem);
        }

        // POST: InventoryItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "InventoryItemId,InventoryItemCode,InventoryItemName,UnitPrice,CategoryId")] InventoryItem inventoryItem)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Entry(inventoryItem).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(_applicationDbContext.Categories, "Id", "CategoryName", inventoryItem.CategoryId);
            return View(inventoryItem);
        }

        // GET: InventoryItems/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryItem inventoryItem = await _applicationDbContext.InventoryItems.FindAsync(id);
            if (inventoryItem == null)
            {
                return HttpNotFound();
            }
            return View(inventoryItem);
        }

        // POST: InventoryItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            InventoryItem inventoryItem = await _applicationDbContext.InventoryItems.FindAsync(id);
            _applicationDbContext.InventoryItems.Remove(inventoryItem);
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
