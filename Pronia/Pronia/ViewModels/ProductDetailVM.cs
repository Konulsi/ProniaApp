using Pronia.Models;

namespace Pronia.ViewModels
{
    public class ProductDetailVM
    {
        public Product ProductDt { get; set; }
        public Dictionary<string, string> HeaderBackgrounds { get; set; }
        public List<Advertising> Advertisings { get; set; } 
        public List<Product> RelatedProducts { get; set; }
        public CommentVM CommentVM { get; set; }
        public List<ProductComment> ProductComments { get; set; }


    }
}
