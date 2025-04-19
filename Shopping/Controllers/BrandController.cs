using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Reponsitory;

namespace Shopping.Controllers
{
    public class BrandController : Controller
    {
        private readonly DataContext _dataContext;
        public BrandController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index(string slug = "")
        {
            BrandModel brandModel = await _dataContext.Brands.FirstOrDefaultAsync(x => x.Slug == slug);
            if (brandModel == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var products = await _dataContext.Products.Where(x => x.BrandId == brandModel.Id).ToListAsync();
            return View(products.OrderByDescending(c => c.Id).ToList());
        }
    }
}
