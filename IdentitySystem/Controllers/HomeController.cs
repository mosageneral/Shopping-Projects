using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentitySystem.Models;
using Microsoft.AspNetCore.Authorization;
using BL.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace IdentitySystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork uow;
        private readonly IHostingEnvironment he;
        public HomeController(IUnitOfWork uow, IHostingEnvironment he)
        {
            this.uow = uow;
            this.he = he;
        }
       
        public IActionResult Index()
        {
         
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult ProductDetails(int id)
        {
            var product = uow.productRepository.GetMany(a => a.Id == id).Include(c => c.Category).FirstOrDefault(); ;
            return View(product);
        }
        public IActionResult GetProducts(int Id)
        {
            var Products = uow.productRepository.GetMany(a => a.CategoryId == Id).Include(p=>p.Category).OrderByDescending(a=>a.Sold).ToHashSet();

            return PartialView("_GetProducts",Products);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
