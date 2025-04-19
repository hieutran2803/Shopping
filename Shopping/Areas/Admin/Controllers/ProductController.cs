using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Reponsitory;

namespace Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller // Inherit from Controller base class
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(DataContext dataContext, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = dataContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Products
    .OrderByDescending(p => p.Id)
    .Include(p => p.Category).Include(p => p.Brand)
    .ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");
                var existingProduct = await _dataContext.Products
                    .FirstOrDefaultAsync(p => p.Slug == product.Slug);
                if (existingProduct != null)
                {
                    ModelState.AddModelError("Slug", "Slug đã tồn tại");
                    return View(product);
                }
                if (product.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                    string extension = Path.GetExtension(product.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    product.Image = fileName;
                    string path = Path.Combine(_webHostEnvironment.WebRootPath + "/images/products", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(fileStream);
                    }
                }
                _dataContext.Products.Add(product);
                await _dataContext.SaveChangesAsync();

                TempData["Success"] = "Thêm sản phẩm thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Có lỗi xảy ra trong quá trình thêm sản phẩm";
                List<string> errors = new List<string>();
                foreach (var error in ModelState.Values)
                {
                    foreach (var err in error.Errors)
                    {
                        errors.Add(err.ErrorMessage);
                    }
                }
                string errorMessage = string.Join(", ", errors);
            }
            return View(product);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _dataContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductModel product)
        {
            // Load lại danh sách categories và brands
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

            // Tìm sản phẩm hiện có
            var existingProduct = await _dataContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (existingProduct == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm.";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Có lỗi xảy ra trong quá trình cập nhật sản phẩm.";
                return View(product);
            }

            try
            {
                // Cập nhật slug từ tên sản phẩm
                product.Slug = product.Name.ToLower().Replace(" ", "-");

                // Xử lý ảnh nếu có ảnh mới
                if (product.ImageFile != null)
                {
                    // Tạo tên file mới
                    string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                    string extension = Path.GetExtension(product.ImageFile.FileName);
                    fileName += DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;
                    string savePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products", fileName);

                    // Lưu ảnh mới
                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(stream);
                    }

                    // Xóa ảnh cũ nếu tồn tại
                    if (!string.IsNullOrEmpty(existingProduct.Image))
                    {
                        string oldPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products", existingProduct.Image);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    existingProduct.Image = fileName;
                }

                // Cập nhật thông tin sản phẩm
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.BrandId = product.BrandId;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.Slug = product.Slug;

                _dataContext.Products.Update(existingProduct);
                await _dataContext.SaveChangesAsync();

                TempData["Success"] = "Cập nhật sản phẩm thành công.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Đã xảy ra lỗi: " + ex.Message;
                return View(product);
            }
        }

        public IActionResult Delete(int id)
        {
            var product = _dataContext.Products.Find(id);
            if(!string.IsNullOrEmpty(product.Image))
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath + "/images/products", product.Image);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            _dataContext.Products.Remove(product);
            _dataContext.SaveChanges();
            TempData["Success"] = "Xóa sản phẩm thành công";
            return RedirectToAction("Index");
        }
    }
}
