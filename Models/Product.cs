using System.ComponentModel.DataAnnotations;

namespace _5thBridgeShop.Models
{
    public class Product
    {
             [Key]
            public long Id { get; set; }
        [Required]
            public string ProductName { get; set; }
        [Required]
            public string Rating { get; set; }
        [Required]
            public string description { get; set; } 
    }
}
