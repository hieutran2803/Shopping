using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tên tài khoản")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Yêu cầu nhập gmail"), EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password),Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int Status { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
