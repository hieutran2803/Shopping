using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Reponsitory;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BrandController(DataContext dataContext, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = dataContext;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: BrandController
        public async Task<IActionResult> Index()
        {
            var brands = await _dataContext.Brands.OrderByDescending(b => b.Id).ToListAsync();
            return View(brands);
        }

        // GET: BrandController/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BrandController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModel brand)
        {
            if (ModelState.IsValid)
            {
                brand.Slug = brand.Name.ToLower().Replace(" ", "-");

                var existingBrand = await _dataContext.Brands
                    .FirstOrDefaultAsync(b => b.Slug == brand.Slug);
                if (existingBrand != null)
                {
                    ModelState.AddModelError("Slug", "Slug đã tồn tại");
                    return View(brand);
                }

                _dataContext.Brands.Add(brand);
                await _dataContext.SaveChangesAsync();

                TempData["Success"] = "Tạo thương hiệu thành công.";
                return RedirectToAction(nameof(Index));
            }

            return View(brand);
        }

        // GET: BrandController/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var brand = await _dataContext.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: BrandController/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BrandModel brand)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(brand);
            }

            try
            {
                brand.Slug = brand.Name.ToLower().Replace(" ", "-");

                var duplicateSlug = await _dataContext.Brands
                    .FirstOrDefaultAsync(b => b.Slug == brand.Slug && b.Id != brand.Id);

                if (duplicateSlug != null)
                {
                    ModelState.AddModelError("Slug", "Slug đã tồn tại.");
                    return View(brand);
                }

                _dataContext.Update(brand);
                await _dataContext.SaveChangesAsync();

                TempData["Success"] = "Cập nhật thương hiệu thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrandExists(brand.Id))
                {
                    return NotFound();
                }

                TempData["Error"] = "Xảy ra lỗi đồng bộ khi cập nhật thương hiệu.";
                throw;
            }
        }

        // POST: BrandController/Delete
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _dataContext.Brands.FindAsync(id);
            if (brand == null)
            {
                TempData["Error"] = "Không tìm thấy thương hiệu cần xóa.";
                return RedirectToAction(nameof(Index));
            }

            _dataContext.Brands.Remove(brand);
            await _dataContext.SaveChangesAsync();

            TempData["Success"] = "Xóa thương hiệu thành công.";
            return RedirectToAction(nameof(Index));
        }

        private bool BrandExists(int id)
        {
            return _dataContext.Brands.Any(b => b.Id == id);
        }
    }
}
