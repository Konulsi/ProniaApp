namespace Pronia.Models
{
    public class Basket: BaseEntity
    {
        //public int ProductCount { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<ProductBasket> ProductBaskets { get; set; }
    }
}
