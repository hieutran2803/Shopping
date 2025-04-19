using Microsoft.EntityFrameworkCore;
using Shopping.Models;

namespace Shopping.Reponsitory
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();

            // Seed Brands
            if (!_context.Brands.Any())
            {
                var brandApple = new BrandModel
                {
                    Name = "Apple",
                    Slug = "apple",
                    Status = 1,
                    Description = "Apple is a tech company that produces the iPhone."
                };

                var brandSamsung = new BrandModel
                {
                    Name = "Samsung",
                    Slug = "samsung",
                    Status = 1,
                    Description = "Samsung is a global tech brand known for its Galaxy phones."
                };

                var brandXiaomi = new BrandModel
                {
                    Name = "Xiaomi",
                    Slug = "xiaomi",
                    Status = 1,
                    Description = "Xiaomi offers affordable and high-quality smartphones."
                };

                _context.Brands.AddRange(brandApple, brandSamsung, brandXiaomi);
                _context.SaveChanges();

                // Seed Categories
                var categoryApple = new CategoryModel
                {
                    Name = "Apple",
                    Description = "Apple category",
                    Slug = "apple",
                    Status = 1
                };

                var categorySamsung = new CategoryModel
                {
                    Name = "Samsung",
                    Description = "Samsung category",
                    Slug = "samsung",
                    Status = 1
                };

                var categoryXiaomi = new CategoryModel
                {
                    Name = "Xiaomi",
                    Description = "Xiaomi category",
                    Slug = "xiaomi",
                    Status = 1
                };

                _context.Categories.AddRange(categoryApple, categorySamsung, categoryXiaomi);
                _context.SaveChanges();

                // Seed Products
                var products = new[]
                {
                    new ProductModel
                    {
                        Name = "iPhone 14",
                        Description = "iPhone 14 is a fruit",
                        Slug = "iphone-14",
                        Price = 1000,
                        Image = "iphone14.jpg",
                        BrandId = brandApple.Id,
                        CategoryId = categoryApple.Id
                    },
                    new ProductModel
                    {  
                        Name = "iPhone 15",
                        Description = "iPhone 15 is a fruit",
                        Slug = "iphone-15",
                        Price = 2000,
                        Image = "iphone15.jpg",
                        BrandId = brandApple.Id,
                        CategoryId = categoryApple.Id
                    },
                    new ProductModel
                    {
                        Name = "Samsung Galaxy S23",
                        Description = "Samsung Galaxy S23 is a fruit",
                        Slug = "samsung-galaxy-s23",
                        Price = 3000,
                        Image = "samsung_s23.jpg",
                        BrandId = brandSamsung.Id,
                        CategoryId = categorySamsung.Id
                    },
                    new ProductModel
                    {
                        Name = "Xiaomi Mi 13",
                        Description = "Xiaomi Mi 13 is a fruit",
                        Slug = "xiaomi-mi-13",
                        Price = 4000,
                        Image = "xiaomi_mi13.jpg",
                        BrandId = brandXiaomi.Id,
                        CategoryId = categoryXiaomi.Id
                    }
                };

                _context.Products.AddRange(products);
                _context.SaveChanges();
            }
        }
    }
}
