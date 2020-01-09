using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.Infrastructure;
using Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace IdentitySystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {

        private readonly IUnitOfWork uow;
        private readonly IHostingEnvironment he;
        public ProductsController(IUnitOfWork uow,IHostingEnvironment he)
        {
            this.uow = uow;
            this.he = he;
        }
        [Authorize(Roles = "Users,Admin,Modrators")]
        public IActionResult Index()
        {
            //Returns All Of TheProducts 
            return View(uow.productRepository.GetAll().Include(a=>a.Category));
        }
        [Authorize(Roles = "Users,Admin,Modrators")]
        [HttpGet]
        public IActionResult CreateProduct()
        {
            ViewBag.Categories = new SelectList(uow.CategoryRepository.GetAll().ToList(), "Id", "Name");
            ViewBag.List = uow.CategoryRepository.GetAll().ToHashSet();
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Users,Admin,Modrators")]
        public IActionResult Details(int Id)
        {
            var P = uow.productRepository.GetAll().Include(a => a.Category).Where(x => x.Id == Id).FirstOrDefault() ;

           ViewBag.Images = P.Images.Split(",");
            
            return View(P);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateProduct(Products product,IFormFile[] Images, IFormFile MainImage)
        {
            if (ModelState.IsValid)
            {
                var ImagesNames = "";
                //Adding The Comming List Of Images In One String  And Spilt It With "," And Save It The Variable  (ImagesNames)
                foreach (var image in Images)
                {
                    if (ImagesNames == "")
                    {
                        ImagesNames = FileHelper.FileUpload(image, he, "Files/ProductsImages");
                    }
                    else
                    {
                        ImagesNames = ImagesNames + "," + FileHelper.FileUpload(image, he, "Files/ProductsImages");
                    }
                }
                //Adding The Main Image And Saves Its Name To The Db Prop
                product.Images = ImagesNames;
                product.MainImage = FileHelper.FileUpload(MainImage, he, "Files/ProductsImages");
                uow.productRepository.Add(product);
                uow.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int Id)
        {
            uow.productRepository.Delete(Id);
            uow.Save();
            return RedirectToAction(nameof(Index));
        }
       
    }
}