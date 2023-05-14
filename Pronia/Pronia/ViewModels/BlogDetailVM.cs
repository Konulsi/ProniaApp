using Pronia.Models;

namespace Pronia.ViewModels
{
    public class BlogDetailVM
    {
        public Blog BlogDt { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Product> NewProducts { get; set; }
        public Dictionary<string, string> HeaderBackgrounds { get; set; }
        public List<Category> Categories { get; set; }
        public CommentVM CommentVM { get; set; }
        public List<BlogComment> BlogComments { get; set; }
    }
}
