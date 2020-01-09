using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.IdentityModels;

namespace IdentitySystem.Areas.Users.Controllers
{
    [Area("Users")]
   
    public class AccountController : Controller
    {
        //Ctor For The Users Account To Inject UserManger
        private UserManager<AppUser> userManager;
        private IUserValidator<AppUser> userValidator;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordHasher;
        private SignInManager<AppUser> signInManager;
        private readonly IHostingEnvironment he;
        private RoleManager<IdentityRole> roleManager;

        public AccountController(IHostingEnvironment he,RoleManager<IdentityRole> roleMgr, UserManager<AppUser> usrMgr, IUserValidator<AppUser> userValid, IPasswordValidator<AppUser> passValid, IPasswordHasher<AppUser> passwordHash, SignInManager<AppUser> signinMgr)
        {
            userManager = usrMgr;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passwordHash;
            signInManager = signinMgr;
            this.he = he;
            roleManager = roleMgr;
        }

        [Authorize(Roles = "Users,Admin,Modrators")]
        public IActionResult Index()
        {
            //Quary For Getting All Of The Users Form User Manger Instance
            var Users = userManager.Users.ToHashSet();
            //Here We Will Return The Users To The View
            return View(Users);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterUser()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task <IActionResult> RegisterUser(RegisterUser registerUser,IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser();
                //Mapping The Data Comming From The View To The App User Class
                appUser.UserName = registerUser.UserName;
                appUser.Email = registerUser.Email;
                appUser.Image = FileHelper.FileUpload(Image, he, "Files/UserProfilePics");
                //Check If The User Exit 
                IdentityResult Result = await userManager.CreateAsync(appUser, registerUser.Password);
                if (Result.Succeeded)
                {
                   IdentityResult result = await userManager.AddToRoleAsync(appUser, "Users");
                    await signInManager.SignOutAsync();
                  var check= await signInManager.PasswordSignInAsync(appUser, registerUser.Password, false, false);
                    if (check.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                   
                }
                else
                {
                    foreach (IdentityError error in Result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
             
            }
            return View(registerUser);
          
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            //Find The User Of The Comming Id 
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                //Delete The Fetched Users 
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                // View The Errors If There Any 
                ModelState.AddModelError("", "User Not Found");
            }
            return View("Index", userManager.Users);
        }
        [Authorize(Roles = "Users,Admin,Modrators")]
        public async Task<IActionResult> Edit(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, string email, string password)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null) {
                user.Email = email;
                // Validate The User Props Before Submit It 
                IdentityResult validEmail = await userValidator.ValidateAsync(userManager, user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                { 
                    //Validate The Password Then Hash It  
                    validPass = await passwordValidator.ValidateAsync(userManager, user, password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }
                if ((validEmail.Succeeded && validPass == null) || (validEmail.Succeeded && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(login.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, login.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(ReturnUrl ?? "/");
                    }
                }
                ModelState.AddModelError("", "Invalid user or password");
            }
            return View(login);
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            //Show Access Denied Page If The User Is Not Allowed To Access The Action Method 
            return View();
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

    }
}