using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace _5thBridgeShop.Models
{
    public class Cart
    {
        [Key]
        public long Id { get; set; }
        
        public long ProductId { get; set; }
            [ForeignKey("ProductId")]
            [NotMapped]
            public Product Product { get; set; }
      //  public long UserId { get; set; }

        

    }
}
