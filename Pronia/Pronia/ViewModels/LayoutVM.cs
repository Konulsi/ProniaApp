using Pronia.Models;

namespace Pronia.ViewModels
{
    public class LayoutVM
    {
        public IEnumerable<Social> Socials { get; set; }

        public Dictionary<string, string> Settings { get; set; }
    }
}
