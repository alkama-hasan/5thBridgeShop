using _5thBridgeShop.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace _5thBridgeShop.Models
{
    public class ApplicationDbContext: IdentityDbContext<Application>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
        public DbSet<Product> Products { get; set; } 
        public DbSet<Cart> Carts { get; set; }
      //  public DbSet<Login> Login { get; set; }
        //public DbSet<Register> Register { get; set; }  

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(100);

                entity.Property(e => e.Rating)
                .IsRequired()
                .HasMaxLength(1);

                entity.Property(e => e.description)
                .IsRequired()
                .HasMaxLength(100);
            });

            base.OnModelCreating(builder);
        }






    }
}
