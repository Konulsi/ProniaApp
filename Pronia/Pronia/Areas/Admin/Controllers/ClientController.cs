using Microsoft.AspNetCore.Mvc;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Data;
using Pronia.Helpers;
using Pronia.Models;
using Pronia.Services.Interfaces;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ClientController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IClientService _clientService;
        public ClientController(AppDbContext context,
                                IWebHostEnvironment env,
                                IClientService clientService)
        {
            _context = context;
            _env = env;
            _clientService = clientService;
        }

        public async Task<IActionResult> Index()
        {
            List<Client> clients = await _clientService.GetClients();
            return View(clients);
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Client client = await _clientService.GetById(id);
            if (client is null) return NotFound();
            return View(client);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientCreateVM client)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(client);
                }



                if (!client.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View(client);
                }




                if (!client.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View(client);
                }



                string fileName = Guid.NewGuid().ToString() + "_" + client.Photo.FileName;


                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);

                await FileHelper.SaveFileAsync(path, client.Photo);

                Client newClient = new()
                {
                    Image = fileName,
                    Name = client.Name,
                    Description = client.Description
                };


                await _context.Clients.AddAsync(newClient);



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

                Client dbClient = await _clientService.GetById(id);

                if (dbClient is null) return NotFound();

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbClient.Image);

                FileHelper.DeleteFile(path);

                _context.Clients.Remove(dbClient);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            Client dbClient = await _clientService.GetById(id);
            if (dbClient is null) return NotFound();

            ClientUpdateVM model = new()
            {
                Image = dbClient.Image,
                Name = dbClient.Name,
                Description = dbClient.Description,
            };

            return View(model);

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ClientUpdateVM clienUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                Client dbClient = await _clientService.GetById(id);

                if (dbClient is null) return NotFound();

                ClientUpdateVM model = new()
                {
                    Image = dbClient.Image,
                    Name = dbClient.Name,
                    Description = dbClient.Description,
                };


                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (clienUpdate.Photo != null)
                {
                    if (!clienUpdate.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View(model);
                    }

                    if (!clienUpdate.Photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View(model);
                    }


                    string dbPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbClient.Image);

                    FileHelper.DeleteFile(dbPath);


                    string fileName = Guid.NewGuid().ToString() + "_" + clienUpdate.Photo.FileName;

                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);

                    await FileHelper.SaveFileAsync(newPath, clienUpdate.Photo);

                    dbClient.Image = fileName;
                }
                else
                {
                    Client client = new()
                    {
                        Image = dbClient.Image
                    };
                }


                dbClient.Name = clienUpdate.Name;
                dbClient.Description = clienUpdate.Description;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                @ViewBag.error = ex.Message;
                return View();
            }
        }
    }
}
