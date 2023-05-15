namespace Pronia.Models
{
    public class ProductBasket : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int BasketId { get; set; }
        public Basket Basket { get; set; }

    }
}
