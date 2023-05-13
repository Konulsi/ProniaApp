using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Data;
using Pronia.Helpers;
using Pronia.Models;
using Pronia.Services;
using Pronia.Services.Interfaces;
using System.Drawing;
using System.Xml.Linq;
using Color = Pronia.Models.Color;
using Size = Pronia.Models.Size;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IColorService _colorService;
        private readonly ISizeService _sizeService;
        private readonly ITagService _tagService;



        public ProductController(AppDbContext context, 
                                IWebHostEnvironment env, 
                                IProductService productService, 
                                ICategoryService categoryService,
                                ISizeService sizeService,
                                ITagService tagService,
                                IColorService colorService)
        {
            _context = context;
            _env = env;
            _productService = productService;
            _categoryService = categoryService;
            _sizeService = sizeService;
            _tagService = tagService;
            _colorService = colorService;

        }
        public async Task<IActionResult> Index(int page = 1, int take = 5 , int? cateId = null, int? tagId = null)
        {
            List<Product> products = await _productService.GetPaginatedDatas(page, take , cateId, tagId);

            List<ProductListVM> mappedDatas = GetMappedDatas(products);
            int pageCount = await GetPageCountAsync(take);

            Paginate<ProductListVM> paginatedDatas = new(mappedDatas, page, pageCount);

            ViewBag.take = take;

            return View(paginatedDatas);
        }



        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            try
            {
                if (id == null) return BadRequest();

                Product dbProduct = await _productService.GetFullDataById((int)id);

                if (dbProduct == null) return NotFound();


                ProductDetailVM model = new()
                {
                    Name = dbProduct.Name,
                    SaleCount = dbProduct.SaleCount,
                    StockCount = dbProduct.StockCount,
                    Description= dbProduct.Description,
                    Price = dbProduct.Price,
                    SKU= dbProduct.SKU,
                    Rate= dbProduct.Rate,
                    ProductCategories = dbProduct.ProductCategories,
                    ProductImages = dbProduct.Images,
                    ProductSizes= dbProduct.ProductSizes,
                    ProductTags = dbProduct.ProductTags,
                    ColorName = dbProduct.Color.Name
                };

                return View(model);
            }
            catch (Exception)
            {
                throw;
            }

        }


        private async Task<int> GetPageCountAsync(int take)
        {
            var productCount = await _productService.GetCountAsync();
            return (int)Math.Ceiling((decimal)productCount / take);
        }

        private List<ProductListVM> GetMappedDatas(List<Product> products)
        {
            List<ProductListVM> mappedDatas = new();

            foreach (var product in products)
            {
                ProductListVM productVM = new()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    MainImage = product.Images.FirstOrDefault(m => m.IsMain).Image,
                    SKU= product.SKU,
                };

                mappedDatas.Add(productVM);
            }

            return mappedDatas;
        }





        private async Task<SelectList> GetCategoryAsync()
        {
            List<Category> categories = await _categoryService.GetCategories();
            return new SelectList(categories, "Id", "Name");
        }

        private async Task<SelectList> GetColorAsync()
        {
            List<Color> colors = await _colorService.GetAllColors();
            return new SelectList(colors, "Id", "Name");
        }

        private async Task<SelectList> GetSizeAsync()
        {
            List<Size> sizes = await _sizeService.GetAllSize();
            return new SelectList(sizes, "Id", "Name");
        }

        private async Task<SelectList> GetTagAsync()
        {
            List<Tag> tags = await _tagService.GetAllAsync();
            return new SelectList(tags, "Id", "Name");
        }







        [HttpGet]
        public async Task<IActionResult> Create()
        {

            ViewBag.categories = await GetCategoryAsync();
            ViewBag.tags = await GetTagAsync();
            ViewBag.sizes = await GetSizeAsync();
            ViewBag.colors = await GetColorAsync();


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM model)
        {
            try
            {
                ViewBag.categories = await GetCategoryAsync();
                ViewBag.tags = await GetTagAsync();
                ViewBag.sizes = await GetSizeAsync();
                ViewBag.colors = await GetColorAsync();

                if (!ModelState.IsValid)
                {
                    return View(model);
                }


                Product newProduct = new();
                List<ProductImage> productImages = new();
                List<ProductCategory> productCategories= new();
                List<ProductTag> productTags= new();
                List<ProductSize> productSizes= new();




                //all images
                foreach (var photo in model.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View();
                    }

                    if (!photo.CheckFileSize(500))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View();
                    }
                }


                foreach (var photo in model.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);
                    await FileHelper.SaveFileAsync(path, photo);

                    ProductImage productImage = new()
                    {
                        Image = fileName
                    };

                    productImages.Add(productImage);
                }
                newProduct.Images= productImages;
                newProduct.Images.FirstOrDefault().IsMain = true;
                newProduct.Images.Skip(1).FirstOrDefault().IsHover = true;

                if (model.CategoryIds.Count > 0)
                {
                    foreach (var cateId in model.CategoryIds)
                    {
                        ProductCategory productCategory = new()
                        {
                            CategoryId = cateId
                        };
                        productCategories.Add(productCategory);
                    }
                    newProduct.ProductCategories = productCategories;
                }
                else
                {
                    ModelState.AddModelError("CategoryIds", "Don't be empty");
                    return View();
                }

                if (model.TagIds.Count > 0)
                {
                    foreach (var tagId in model.TagIds)
                    {
                        ProductTag productTag = new()
                        {
                            TagId = tagId
                        };
                        productTags.Add(productTag);
                    }
                    newProduct.ProductTags = productTags;
                }
                else
                {
                    ModelState.AddModelError("TagIds", "Don't be empty");
                    return View();
                }

                if (model.SizeIds.Count > 0)
                {
                    foreach (var sizeId in model.SizeIds)
                    {
                        ProductSize productSize = new()
                        {
                            SizeId = sizeId
                        };
                        productSizes.Add(productSize);
                    }
                    newProduct.ProductSizes = productSizes;
                }
                else
                {
                    ModelState.AddModelError("TagIds", "Don't be empty");
                    return View();
                }

                var convertPrice = decimal.Parse(model.Price);
                Random random = new();

                newProduct.Name= model.Name;
                newProduct.Description= model.Description;
                newProduct.Price= convertPrice;
                newProduct.StockCount= model.StockCount;
                newProduct.SaleCount= model.SaleCount;
                newProduct.Rate= model.Rate;
                newProduct.ColorId= model.ColorId;
                newProduct.SKU= model.Name.Substring(0,3) + "_" + random.Next();

                await _context.ProductImages.AddRangeAsync(productImages);
                await _context.Products.AddAsync(newProduct);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) return BadRequest();
                Product dbProduct = await _productService.GetFullDataById((int)id);
                if (dbProduct == null) return NotFound();

                foreach (var photo in dbProduct.Images)
                {
                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", photo.Image);
                    FileHelper.DeleteFile(path);
                }

                _context.Products.Remove(dbProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }




        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                ViewBag.categories = await GetCategoryAsync();
                ViewBag.tags = await GetTagAsync();
                ViewBag.sizes = await GetSizeAsync();
                ViewBag.colors = await GetColorAsync(); 

                Product dbProduct = await _productService.GetFullDataById((int)id);
                if (dbProduct == null) return NotFound();

                ProductUpdateVM model = new()
                {
                    Id = dbProduct.Id,
                    Name = dbProduct.Name,
                    Description = dbProduct.Description,
                    Price = dbProduct.Price.ToString("0.#####"),
                    SKU = dbProduct.SKU,
                    Rate = dbProduct.Rate,
                    Images = dbProduct.Images.ToList(),
                    TagIds = dbProduct.ProductTags.Select(m => m.Tag.Id).ToList(),
                    SizeIds = dbProduct.ProductSizes.Select(m => m.Size.Id).ToList(),
                    CategoryIds = dbProduct.ProductCategories.Select(m => m.Category.Id).ToList(),
                    ColorId = dbProduct.ColorId,
                    StockCount = dbProduct.StockCount,
                    SaleCount = dbProduct.SaleCount,
                };
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ProductUpdateVM updatedProduct)
        {
            try
            {
                ViewBag.categories = await GetCategoryAsync();
                ViewBag.tags = await GetTagAsync();
                ViewBag.sizes = await GetSizeAsync();
                ViewBag.colors = await GetColorAsync();

                if (id == null) return BadRequest();
                Product dbProduct = await _productService.GetFullDataById((int)id);
                if (dbProduct == null) return NotFound();

                ProductUpdateVM model = new()
                {
                    Id = dbProduct.Id,
                    Name = dbProduct.Name,
                    Description = dbProduct.Description,
                    Price = dbProduct.Price.ToString("0.#####"),
                    CategoryIds = dbProduct.ProductCategories.Select(m => m.Category.Id).ToList(),
                    Images = dbProduct.Images.ToList(),
                    TagIds = dbProduct.ProductTags.Select(m => m.Tag.Id).ToList(),
                    SizeIds = dbProduct.ProductSizes.Select(m => m.Size.Id).ToList(),
                    ColorId = dbProduct.ColorId,
                    SKU = dbProduct.SKU,
                    Rate = dbProduct.Rate,
                    StockCount = dbProduct.StockCount,
                    SaleCount = dbProduct.SaleCount,
                };

                if (!ModelState.IsValid) return View(model);

                if (updatedProduct.Photos is not null)
                {
                    foreach (var photo in updatedProduct.Photos)
                    {
                        if (!photo.CheckFileType("image/"))
                        {
                            ModelState.AddModelError("Photo", "File type must be image");
                            return View(model);
                        }

                        if (!photo.CheckFileSize(200))
                        {
                            ModelState.AddModelError("Photo", "Image size must be max 200kb");
                            return View(model);
                        }
                    }

                    foreach (var item in dbProduct.Images)
                    {
                        string dbPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", item.Image);
                        FileHelper.DeleteFile(dbPath);
                    }

                    List<ProductImage> productImages = new();
                    foreach (var photo in updatedProduct.Photos)
                    {
                        string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                        string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);
                        await FileHelper.SaveFileAsync(path, photo);

                        ProductImage productImage = new()
                        {
                            Image = fileName
                        };
                        productImages.Add(productImage);
                    }
                    dbProduct.Images = productImages;
                    dbProduct.Images.FirstOrDefault().IsMain = true;
                    dbProduct.Images.Skip(1).FirstOrDefault().IsHover = true;

                    await _context.ProductImages.AddRangeAsync(productImages);
                }
                else
                {
                    Product newProduct = new()
                    {
                        Images = dbProduct.Images
                    };
                }


                List<ProductCategory> productCategories = new();
                if (updatedProduct.CategoryIds.Count > 0)
                {
                    foreach (var cateId in updatedProduct.CategoryIds)
                    {
                        ProductCategory productCategory = new()
                        {
                            CategoryId = cateId
                        };
                        productCategories.Add(productCategory);
                    }
                    dbProduct.ProductCategories = productCategories;
                }
                else
                {
                    ModelState.AddModelError("CategoryIds", "Don't be empty");
                    return View();
                }


                List<ProductTag> productTags = new();
                if (updatedProduct.TagIds.Count > 0)
                {
                    foreach (var tagId in updatedProduct.TagIds)
                    {
                        ProductTag productTag = new()
                        {
                            TagId = tagId
                        };
                        productTags.Add(productTag);
                    }
                    dbProduct.ProductTags = productTags;
                }
                else
                {
                    ModelState.AddModelError("TagIds", "Don't be empty");
                    return View();
                }


                List<ProductSize> productSizes = new();
                if (updatedProduct.SizeIds.Count > 0)
                {
                    foreach (var sizeId in updatedProduct.SizeIds)
                    {
                        ProductSize productSize = new()
                        {
                            SizeId = sizeId
                        };
                        productSizes.Add(productSize);
                    }
                    dbProduct.ProductSizes = productSizes;
                }
                else
                {
                    ModelState.AddModelError("TagIds", "Don't be empty");
                    return View();
                }

                var convertPrice = decimal.Parse(updatedProduct.Price);

                dbProduct.Name = updatedProduct.Name;
                dbProduct.Description = updatedProduct.Description;
                dbProduct.Price = convertPrice;
                dbProduct.StockCount = updatedProduct.StockCount;
                dbProduct.SaleCount = updatedProduct.SaleCount;
                dbProduct.ColorId = updatedProduct.ColorId;
                dbProduct.Rate= updatedProduct.Rate;
                dbProduct.SKU= updatedProduct.SKU;


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }
    }
}
