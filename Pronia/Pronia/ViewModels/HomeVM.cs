﻿using Pronia.Models;

namespace Pronia.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Advertising> Advertisings { get; set; }
        public Dictionary<string, string> HeaderBackgrounds { get; set; }
        public List<Product> BestSellerProduct { get; set; }
        public List<Product> FeaturedProduct { get; set; }
        public List<Product> LatestProduct { get; set; }
        public List<Product> NewProducts { get; set; }
        public List<Banner> Banners { get; set; }
        public List<Client> Clients { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Product> Products { get; set; }




    }
}
