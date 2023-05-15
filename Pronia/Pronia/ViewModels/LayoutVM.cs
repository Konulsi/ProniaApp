using Pronia.Models;
using Pronia.ViewModels.Basket;

namespace Pronia.ViewModels
{
    public class LayoutVM
    {
        public int BasketCount { get; set; }
        public Dictionary<string, string> Settings { get; set; }

        public List<BasketDetailVM> BasketDetailVMs { get; set; }
    }
}
