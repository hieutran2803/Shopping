using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace Shopping.Reponsitory.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Allow null (image upload is optional)
            var file = value as IFormFile;
            if (file == null)
            {
                return ValidationResult.Success;
            }

            var extension = Path.GetExtension(file.FileName).ToLower();
            string[] allowedExtensions = { ".jpg", ".png" };
            bool isValid = allowedExtensions.Any(x => extension == x);

            if (!isValid)
            {
                return new ValidationResult("Only .jpg and .png files are allowed.");
            }

            return ValidationResult.Success;
        }
    }
}
