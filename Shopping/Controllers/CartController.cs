using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Models.ViewModels;
using Shopping.Reponsitory;
using System.Text.Json;

namespace Shopping.Controllers
{
    public class CartController : Controller
    {
        private readonly DataContext _dataContext;
        public CartController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult Index()
        {
            // Fix for CS0019: Correctly deserialize the session data and handle null values
            var cartJson = HttpContext.Session.GetString("cart");
            List<CartItemModel> cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItemModel>()
                : JsonSerializer.Deserialize<List<CartItemModel>>(cartJson);

            // Fix for IDE0028: Simplify collection initialization
            CartItemViewModel cartItemViewModel = new()
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)
            };

            return View(cartItemViewModel);
        }
        public async Task<IActionResult> AddToCart(int Id)
        {
            var product = await _dataContext.Products.FirstOrDefaultAsync(x => x.Id == Id);
            if (product == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var cartJson = HttpContext.Session.GetString("cart");
            List<CartItemModel> cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItemModel>()
                : JsonSerializer.Deserialize<List<CartItemModel>>(cartJson);
            var existingCartItem = cart.FirstOrDefault(x => x.ProductId == Id);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItemModel
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1
                });
            }
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart));
            TempData["succes"] = "Product added to cart successfully!";
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> RemoveFromCart(int Id)
        {
            var cartJson = HttpContext.Session.GetString("cart");
            List<CartItemModel> cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItemModel>()
                : JsonSerializer.Deserialize<List<CartItemModel>>(cartJson);
            var existingCartItem = cart.FirstOrDefault(x => x.ProductId == Id);
            if (existingCartItem != null)
            {
                cart.Remove(existingCartItem);
            }
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart));
            TempData["succes"] = "Product removed from cart successfully!";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Decrease(int Id)
        {
            var cartJson = HttpContext.Session.GetString("cart");
            List<CartItemModel> cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItemModel>()
                : JsonSerializer.Deserialize<List<CartItemModel>>(cartJson);
            var existingCartItem = cart.FirstOrDefault(x => x.ProductId == Id);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity--;
                if (existingCartItem.Quantity <= 0)
                {
                    cart.Remove(existingCartItem);
                }
            }
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart));
            TempData["succes"] = "Product quantity Decrease successfully!";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Increase(int Id)
        {
            var cartJson = HttpContext.Session.GetString("cart");
            List<CartItemModel> cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItemModel>()
                : JsonSerializer.Deserialize<List<CartItemModel>>(cartJson);
            var existingCartItem = cart.FirstOrDefault(x => x.ProductId == Id);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity++;
            }
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart));
            TempData["succes"] = "Product quantity increased successfully!";
            return RedirectToAction("Index");
        }
    }
}
