using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Reponsitory;

namespace Shopping.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        public ProductController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IActionResult Index()
        {
            return View();
        }       
        public async Task<IActionResult> Details(int Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var productById = await _dataContext.Products.FirstOrDefaultAsync(x => x.Id == Id);
           
            return View(productById);
        }
    }
}
