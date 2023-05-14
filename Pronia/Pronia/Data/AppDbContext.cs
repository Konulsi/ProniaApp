using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pronia.Models;
using System.Composition;

namespace Pronia.Data
{
    public class AppDbContext: IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Advertising> Advertisings { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<HeaderBackground> HeaderBackgrounds { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Slider> Sliders{ get; set; }
        public DbSet<Social> Socials { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        



        }

    }
}
