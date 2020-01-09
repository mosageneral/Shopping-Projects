using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Models.Models;

namespace IdentitySystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork uow;
        private readonly IHostingEnvironment he;
        public CategoriesController(IUnitOfWork uow, IHostingEnvironment he)
        {
            this.uow = uow;
            this.he = he;
        }
        [Authorize(Roles = "Users,Admin,Modrators")]
        public IActionResult Index()
        {

            return View(uow.CategoryRepository.GetAll().ToHashSet());
        }
        [HttpGet]
        [Authorize(Roles = "Users,Admin,Modrators")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Category category)
        {
            uow.CategoryRepository.Add(category);
            uow.Save();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int Id)
        {
            try
            {
                uow.CategoryRepository.Delete(Id);
                uow.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
               
                
            }
            return RedirectToAction(nameof(Index));
        }
    }
}