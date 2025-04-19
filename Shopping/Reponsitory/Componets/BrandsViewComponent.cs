using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Shopping.Reponsitory.Componets
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public BrandsViewComponent(DataContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var brands = await _context.Brands.ToListAsync();
            return View(brands);
        }
    }
}
