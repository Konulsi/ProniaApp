using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Admin.ViewModels;
using Pronia.Helpers.Enums;
using Pronia.Models;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AccountController : Controller
    {
 
            private readonly UserManager<AppUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;

            }


            [HttpGet]
            public async Task<IActionResult> AddRoleToUser()
            {
                ViewBag.users = await GetUsersAsync();
                ViewBag.roles = await GetRolesAsync();
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> AddRoleToUser(UserRoleVM model)
            {
                ViewBag.users = await GetUsersAsync();
                ViewBag.roles = await GetRolesAsync();

                AppUser user = await _userManager.FindByIdAsync(model.UserId);

                IdentityRole role = await _roleManager.FindByIdAsync(model.RoleId);

                await _userManager.AddToRoleAsync(user, role.Name);

                return View();
            }


            private async Task<SelectList> GetUsersAsync()
            {
                List<AppUser> users = await _userManager.Users.ToListAsync();
                return new SelectList(users, "Id", "FullName");
            }

            private async Task<SelectList> GetRolesAsync()
            {
                List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();
                return new SelectList(roles, "Id", "Name");
            }

            public async Task CreateRole()
            {
                foreach (var role in Enum.GetValues(typeof(Roles)))
                {
                    if (!await _roleManager.RoleExistsAsync(role.ToString()))
                    {
                        await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
                    }
                }
            }
    }
}
