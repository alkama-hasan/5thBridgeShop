using System.ComponentModel.DataAnnotations;

namespace _5thBridgeShop.Models
{
    public class Login
    {
        [Key]
        [Required(ErrorMessage ="Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Password is required")]
        public string Password { get; set; }
    }
}
