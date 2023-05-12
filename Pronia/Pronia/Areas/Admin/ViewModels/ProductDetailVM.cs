using Pronia.Models;

namespace Pronia.Areas.Admin.ViewModels
{
    public class ProductDetailVM
    {
        public string Name { get; set; }
        public string MainImage { get; set; }
        public string HoverImage { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Rate { get; set; } = 5;
        public int SaleCount { get; set; }
        public int StockCount { get; set; }
        public string SKU { get; set; }
        public string ColorName { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }
        public ICollection<ProductSize> ProductSizes { get; set; }
        public ICollection<ProductTag> ProductTags { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }



    }
}
