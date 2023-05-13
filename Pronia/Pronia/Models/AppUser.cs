using Microsoft.AspNetCore.Identity;

namespace Pronia.Models
{
    public class AppUser: IdentityUser
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public bool IsRememberMe { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<ProductComment> ProductComments { get; set; }


    }
}
