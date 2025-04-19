﻿using System.Drawing;

namespace Shopping.Models
{
    public class CartItemModel
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Image { get; }

        public decimal TotalPrice => Price * Quantity;
        public CartItemModel()
        {
        }
        public CartItemModel(ProductModel product)
        {
            ProductId = product.Id;
            ProductName = product.Name;
            Quantity = 1;
            Price = product.Price;
            Image = product.Image;
        }
    }
}
