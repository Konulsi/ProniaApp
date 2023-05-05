namespace Pronia.Models
{
    public class Product: BaseEntity
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
        public int ColorId { get; set; }
        public Color Color { get; set; }
        public ICollection<ProductImage> Images { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
