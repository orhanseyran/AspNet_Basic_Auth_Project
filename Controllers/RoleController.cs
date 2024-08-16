using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var role =  _roleManager.Roles.ToList();
            
            
            return View(role);
        }
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        
        {
          var  roleName = user.UserRole;
            if(roleName == null)
            {
                return View(user);
            }

            if(ModelState.IsValid){
                var result = await _roleManager.CreateAsync(new IdentityRole(
                    roleName
                ));
                if(result.Succeeded){
                    return RedirectToAction("Index");
                }
                else{
                    foreach(var error in result.Errors){
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(user);
        }

        public async Task<IActionResult> Delete(string id){
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View(role);
        }

        
    }
}