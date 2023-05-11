using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Admin.ViewModels
{
    public class ColorCreateVM
    {
        [Required]
        public string Name { get; set; }

    }
}
