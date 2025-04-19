using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Reponsitory;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoryController(DataContext dataContext, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = dataContext;
            _webHostEnvironment = webHostEnvironment;
        }
        // GET: CategoryController
        public async Task<IActionResult> Index()
        {
            var categories = await _dataContext.Categories.OrderByDescending(s=>s.Id).ToListAsync();
            return View(categories);
        }
        // GET: CategoryController/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");
                var existingCategory = await _dataContext.Categories
                    .FirstOrDefaultAsync(c => c.Slug == category.Slug);
                if (existingCategory != null)
                {
                    ModelState.AddModelError("Slug", "Slug đã tồn tại");
                    return View(category);
                }
                _dataContext.Categories.Add(category);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        // GET: CategoryController/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _dataContext.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        // Fix for CS0103: The name 'existingCategory' does not exist in the current context
        // The issue is that 'existingCategory' is being used as a method, but it is not defined as such.
        // Replace the problematic line with a proper method call to check if the category exists.

        private bool CategoryExists(int id)
        {
            return _dataContext.Categories.Any(c => c.Id == id);
        }

        // Update the Edit method to use the new method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryModel category)
        {
            // Kiểm tra id có khớp với category.Id không
            if (id != category.Id)
            {
                return NotFound();
            }

            // Kiểm tra tính hợp lệ của model
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            try
            {
                // Tạo Slug từ tên danh mục
                category.Slug = category.Name.ToLower().Replace(" ", "-");

                // Kiểm tra trùng Slug (ngoại trừ chính nó)
                var duplicateSlug = await _dataContext.Categories
                    .FirstOrDefaultAsync(c => c.Slug == category.Slug && c.Id != category.Id);

                if (duplicateSlug != null)
                {
                    ModelState.AddModelError("Slug", "Slug đã tồn tại.");
                    return View(category);
                }

                // Cập nhật danh mục
                _dataContext.Update(category);
                await _dataContext.SaveChangesAsync();

                TempData["Success"] = "Cập nhật danh mục thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.Id))
                {
                    return NotFound();
                }

                TempData["Error"] = "Xảy ra lỗi đồng bộ khi cập nhật danh mục.";
                throw;
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _dataContext.Categories.FindAsync(id);
            if (category == null)
            {
                TempData["Error"] = "Không tìm thấy danh mục cần xóa.";
                return RedirectToAction(nameof(Index));
            }

            _dataContext.Categories.Remove(category);
            await _dataContext.SaveChangesAsync();

            TempData["Success"] = "Xóa danh mục thành công.";
            return RedirectToAction("Index");
        }

    }
}
