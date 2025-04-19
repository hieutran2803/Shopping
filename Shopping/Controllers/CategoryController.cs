using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Reponsitory;


namespace Shopping.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataContext _dataContext;
        public CategoryController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async  Task<IActionResult> Index(string slug ="")
        {
            CategoryModel categoryModel = await _dataContext.Categories.FirstOrDefaultAsync(x => x.Slug == slug);
            if (categoryModel == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var products = await _dataContext.Products.Where(x => x.CategoryId == categoryModel.Id).ToListAsync();
            return View(products.OrderByDescending(c => c.Id).ToList());
        }
    }
}
