namespace Pronia.Models
{
    public class BlogImage: BaseEntity
    {
        public string Image { get; set; }
        public bool IsMain { get; set; } = false;
        public bool IsHover { get; set; } = false;
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
