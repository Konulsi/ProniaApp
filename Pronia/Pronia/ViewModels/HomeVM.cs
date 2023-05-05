using Pronia.Models;

namespace Pronia.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Advertising> Advertisings { get; set; }
        public Dictionary<string, string> HeaderBackgrounds { get; set; }
        public List<Product> Products { get; set; }



    }
}
