using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.WebSockets;
using DetailWorkflow.DataLayer;
using DetailWorkflow.Models;
using DetailWorkflow.ViewModels;
using TreeUtility;

namespace DetailWorkflow.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();

        private void ValidateParentsAreParentLess(Category category)
        {
            //There is no parent
            if (category.ParentCategoryId == null)
            {
                return;
            }

            //The parent has a parent
            Category parentCategory = _applicationDbContext.Categories.Find(category.ParentCategoryId);
            if (parentCategory.ParentCategoryId != null)
            {
                throw new InvalidOperationException("You cannot nest this Category more than two levels deep.");
            }

            //The parent does not have a parent, but the category being nested has childeren
            int numberOfChildren = _applicationDbContext.Categories.Count(c => c.ParentCategoryId == category.Id);
            if (numberOfChildren > 0)
            {
                throw new InvalidOperationException("You cannot nest this Category's children more than two levels deep.");
            }
        }

        private List<Category> GetListOfNodes()
        {
            List<Category> sourceCategories = _applicationDbContext.Categories.ToList();
            List<Category> categories = new List<Category>();
            foreach (var sourceCategory in sourceCategories)
            {
                Category c = new Category();
                c.Id = sourceCategory.Id;
                c.CategoryName = sourceCategory.CategoryName;
                if (sourceCategory.Parent != null)
                {
                    c.Parent = new Category();
                    c.Parent.Id = (int)sourceCategory.ParentCategoryId;
                }
                categories.Add(c);
            }
            return categories;
        }

        private string EnumerateNodes(Category parent)
        {
            string content = String.Empty;
            content += "<li class=\"treenodes\">";
            content += parent.CategoryName;
            content += String.Format("<a href=\"/Categories/Edit/{0}\" class=\"btn btn-primary btn-xs treenodeeditbutton\">Edit</a>", parent.Id);
            content += String.Format("<a href=\"/Categories/Delete/{0}\" class=\"btn btn-danger btn-xs treenodedeletebutton\">Delete</a>", parent.Id);

            //If no children, end the list
            if (parent.Children.Count == 0)
            {
                content += "</li>";
            }
            else
            {
                content += "<ul>";

                //Loop one past the number of children
                int numberOfChildren = parent.Children.Count;
                for (int i = 0; i <= numberOfChildren; i++)
                {
                    // If this iteration index points to a child
                    //call the function recursively
                    if (numberOfChildren > 0 && i < numberOfChildren)
                    {
                        Category child = parent.Children[i];
                        content += EnumerateNodes(child);
                    }

                    // If this iteration's index points the last child, end the </ul>
                    if (numberOfChildren > 0 && i == numberOfChildren)
                    {
                        content += "</ul>";
                    }
                }
            }
            return content;
        }

        private SelectList populateParentCategorySelectListItem(int? id)
        {
            SelectList selectList;

            if (id == null)
            {
                selectList = new SelectList(
                    _applicationDbContext
                        .Categories
                        .Where(c => c.ParentCategoryId == null), "Id", "CategoryName");
            }
            else if (_applicationDbContext.Categories.Count(c => c.ParentCategoryId == id) == 0)
            {
                selectList = new SelectList(
                    _applicationDbContext
                        .Categories
                        .Where(c => c.ParentCategoryId == null && c.Id != id), "Id", "CategoryName");
            }
            else
            {
                selectList = new SelectList(
                   _applicationDbContext
                       .Categories
                       .Where(c => false), "Id", "CategoryName");
            }
            return selectList;
        }

        public async Task<ActionResult> Index()
        {
            var fullString = "<ul>";
            IList<Category> listOfNode = GetListOfNodes();
            IList<Category> topLevelCategories = TreeHelper.ConvertToForest(listOfNode);
            foreach (var topLevelCategory in topLevelCategories)
            {
                fullString += EnumerateNodes(topLevelCategory);
            }
            fullString += "</ul>";

            return View((object)fullString);
        }

        // GET: Categories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await _applicationDbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            ViewBag.ParentCategoryIdSelectList = populateParentCategorySelectListItem(null);
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ParentCategoryId,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ValidateParentsAreParentLess(category);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    ViewBag.ParentCategoryIdSelectList = populateParentCategorySelectListItem(null);
                    return View(category);
                }
                _applicationDbContext.Categories.Add(category);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await _applicationDbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            CategoryViewModel categoryViewModel = new CategoryViewModel();
            categoryViewModel.Id = category.Id;
            categoryViewModel.ParentCategoryId = category.ParentCategoryId;
            categoryViewModel.CategoryName = category.CategoryName;

            ViewBag.ParentCategoryIdSelectList = populateParentCategorySelectListItem(null);
            return View(categoryViewModel);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ParentCategoryId,CategoryName")] CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                Category editedCategory = new Category();
                try
                {
                    editedCategory.Id = categoryViewModel.Id;
                    editedCategory.CategoryName = categoryViewModel.CategoryName;
                    editedCategory.ParentCategoryId = categoryViewModel.ParentCategoryId;
                    ValidateParentsAreParentLess(editedCategory);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    ViewBag.ParentCategoryIdSelectList = populateParentCategorySelectListItem(categoryViewModel.Id);
                    return View("Edit", categoryViewModel);
                }

                _applicationDbContext.Entry(editedCategory).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ParentCategoryIdSelectList = populateParentCategorySelectListItem(categoryViewModel.Id);
            return View(categoryViewModel);
        }

        // GET: Categories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await _applicationDbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category category = await _applicationDbContext.Categories.FindAsync(id);
            _applicationDbContext.Categories.Remove(category);
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
