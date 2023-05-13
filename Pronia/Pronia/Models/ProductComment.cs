namespace Pronia.Models
{
    public class ProductComment:  BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Subject { get; set; }
        public string Message { get; set; }

    }
}
