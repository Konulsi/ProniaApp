using Pronia.Models;

namespace Pronia.ViewModels
{
    public class AboutVM
    {
        public Dictionary<string, string> HeaderBackgrounds { get; set; }
        public List<Advertising> Advertisings { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Blog>  Blogs { get; set; }

    }
}
