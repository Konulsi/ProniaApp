using Pronia.Models;
using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Admin.ViewModels
{
    public class ProductCreateVM
    {
        [Required(ErrorMessage = "Don`t be empty")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Don`t be empty")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public string Price { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public int Rate { get; set; } = 5;

        [Required(ErrorMessage = "Don`t be empty")]
        public int SaleCount { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public int StockCount { get; set; }
        public string SKU { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public int ColorId { get; set; }
        [Required(ErrorMessage = "Don`t be empty")]
        public List<IFormFile> Photos { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public List<int> SizeIds { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public List<int> TagIds { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public List<int> CategoryIds { get; set; }


    }
}
