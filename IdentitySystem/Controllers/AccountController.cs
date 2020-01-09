using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.IdentityModels;

namespace IdentitySystem.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;

        //class constructor
        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [AllowAnonymous]
        public IActionResult AccessDenied(string ReturnUrl)
        {
            ViewBag.c = ReturnUrl;
            return View();
        }
        public async Task<IActionResult> Profile(string userId)
        {
            var user = await _userManager.GetUserAsync(User);
            

            
            return View(user);
        }
    }
}