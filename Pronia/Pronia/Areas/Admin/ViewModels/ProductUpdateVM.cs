using Pronia.Models;
using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Admin.ViewModels
{
    public class ProductUpdateVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public string Price { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public int SaleCount { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public int StockCount { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public int ColorId { get; set; }
        public string SKU { get; set; }
        public int Rate { get; set; }

        public ICollection<ProductImage> Images { get; set; }

        public List<IFormFile> Photos { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public List<int> SizeIds { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public List<int> TagIds { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public List<int> CategoryIds { get; set; }
    }
}
