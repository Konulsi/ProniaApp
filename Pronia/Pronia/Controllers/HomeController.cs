using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services;
using Pronia.Services.Interfaces;
using Pronia.ViewModels;
using Pronia.ViewModels.Basket;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ISliderService _sliderService;
        private readonly IAdvertisingService _advertisingService;
        private readonly IProductService _productService;
        private readonly IClientService _clientService;
        private readonly IBrandService _brandService;
        private readonly IBlogService _blogService;
        private readonly IBasketService _basketService;
        



        public HomeController(ISliderService sliderService, 
                              IAdvertisingService advertisingService, 
                              AppDbContext context,
                              IProductService productService,
                              IClientService clientService,
                              IBrandService brandService,
                              IBlogService blogService,
                              IBasketService basketService)
        {
            _sliderService = sliderService;
            _advertisingService = advertisingService;
            _context= context;
            _productService= productService;
            _clientService = clientService;
            _brandService= brandService;
            _blogService= blogService;
            _basketService= basketService;

        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _sliderService.GetAll();
            List<Advertising> advertisings = await _advertisingService.GetAll();
            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Product> featuredProduct = await _productService.GetFeaturedProducts();
            List<Product> bestsellerProduct = await _productService.GetBestsellerProducts();
            List<Product> latestProduct = await _productService.GetLatestProducts();
            List<Product> newProduct = await _productService.GetNewProducts();
            List<Banner> banners = await _context.Banners.ToListAsync();
            List<Client> clients = await _clientService.GetClients();
            List<Brand> brands = await _brandService.GetBrands();
            List<Blog> blogs = await _blogService.GetBlogs();
            List<Product> products = await _productService.GetAll();



            HomeVM model = new()
            {
                Sliders = sliders,
                Advertisings = advertisings,
                HeaderBackgrounds= headerBackgrounds,
                FeaturedProduct = featuredProduct,
                BestSellerProduct = bestsellerProduct,
                LatestProduct = latestProduct,
                Banners = banners,
                NewProducts= newProduct,
                Clients= clients,
                Brands= brands,
                Blogs= blogs,
                Products= products,
                
            };

            return View(model);
        }



        //basket
        [HttpPost]
        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id == null) return BadRequest();

            Product dbproduct = await _productService.GetById((int)id);     

            if (dbproduct == null) return NotFound();

            List<BasketVM> basket = _basketService.GetBasketDatas(); 
            BasketVM existProduct = basket?.FirstOrDefault(m => m.Id == dbproduct.Id);

            _basketService.AddProductToBasket(existProduct, dbproduct, basket);  


            int basketCount = basket.Sum(m => m.Count);  


            return Ok(basketCount);
        }
    }
}
