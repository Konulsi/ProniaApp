using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;
using Pronia.ViewModels;
using Pronia.ViewModels.Basket;

namespace Pronia.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBasketService _basketService;
        private readonly IProductService _productService;


        public BasketController(AppDbContext context, IBasketService basketService, IProductService productService)
        {
            _context = context;
            _basketService = basketService;
            _productService = productService;
        }


        public async Task<IActionResult> Index()
        {
            List<BasketVM> basketProducts = _basketService.GetBasketDatas();

            List<BasketDetailVM> basketDetails = new();

            foreach (var product in basketProducts)
            {
                Product dbProduct = await _productService.GetFullDataById(product.Id);

                basketDetails.Add(new BasketDetailVM
                {
                    Id = dbProduct.Id,
                    Name = dbProduct.Name,
                    Price = dbProduct.Price,
                    Image = dbProduct.Images.Where(m => m.IsMain).FirstOrDefault().Image,
                    Count = product.Count,
                    Total = dbProduct.Price * product.Count
                });
            }

            return View(basketDetails);
        }



        public IActionResult DeleteProductFromBasket(int? id)
        {
            if (id == null) return BadRequest();

            _basketService.DeleteProductFromBasket((int)id);
            var datas = _basketService.GetBasketDatas();
            int count = datas.Sum(m => m.Count);
            return Ok(count);
        }



    }
}
