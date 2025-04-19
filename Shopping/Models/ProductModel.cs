using Shopping.Reponsitory.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }
        [Required, MinLength(4, ErrorMessage = "Yêu cầu nhập tên sản phẩm")]
        public string Name { get; set; }
        [Required, MinLength(4, ErrorMessage = "Yêu cầu nhập mô tả sản phẩm")]
        public string Description { get; set; }
        public string Slug { get; set; }
        [Required, Range(0.01, double.MaxValue, ErrorMessage = "Yêu cầu nhập giá sản phẩm lớn hơn 0")]
        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }
        public string Image { get; set; } 
        [Required, Range(1, int.MaxValue, ErrorMessage = "Chọn một thương hiệu")]
        public int BrandId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Chọn một danh mục")]
        public int CategoryId { get; set; }
        public CategoryModel Category { get; set; }
        public BrandModel Brand { get; set; }
        [NotMapped]
        [FileExtension]
        public IFormFile ImageFile { get; set; }
    }
}
