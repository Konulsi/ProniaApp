namespace Pronia.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public bool SoftDelete { get; set; } = false;
        public DateTime CreateDate { get; set; }= DateTime.Now;
        public DateTime UpdateDate { get; set; }=DateTime.Now;

    }
}
