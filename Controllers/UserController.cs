using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace auth.Controllers
{
       [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Retrieve all users
            var users = _userManager.Users.ToList();

            // Create a list to store user and role information
            var userRolesList = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                // Fetch roles for the user
                var roles = await _userManager.GetRolesAsync(user);

                // Add user and their roles to the list
                userRolesList.Add(new UserRoleViewModel
                {
                    User = user,
                    Roles = roles
                });
            }

            // Pass the user and role data to the view using ViewBag
            ViewBag.UserRoles = userRolesList;

            return View();
        }


        public IActionResult Create()
        {
            var role =  _roleManager.Roles.ToList();
            ViewBag.Role = role;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddUser addUser)
        {
            if (addUser.Password == null)
            {
                return BadRequest("Password is required");
            }

            var usercreate = new IdentityUser
            {
                Email = addUser.Email,
                UserName = addUser.Email,
                EmailConfirmed = true,
                LockoutEnabled = true
            };

            var result = await _userManager.CreateAsync(usercreate, addUser.Password);

            if (addUser.Role == null)
            {
                return View(addUser);
                
            }

            var role  =  await _userManager.AddToRoleAsync(usercreate, addUser.Role);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(addUser);
            }
        }
    }
}