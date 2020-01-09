using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.Infrastructure;
using Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using Models.Models;

namespace IdentitySystem.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork uow;
        private readonly IHostingEnvironment he;
        private Cart cart;
        public CartController(IUnitOfWork uow, IHostingEnvironment he, Cart cartService)
        {
            this.uow = uow;
            this.he = he;
            cart = cartService;
        }
        public RedirectToActionResult AddToCart(int Id, string returnUrl)
        {
            Products product = uow.productRepository.GetAll().FirstOrDefault(p => p.Id == Id);
            if (product != null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public RedirectToActionResult RemoveFromCart(int Id, string returnUrl)
        {
            Products product = uow.productRepository.GetAll().FirstOrDefault(p => p.Id == Id);
            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }
        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }
        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel {
                Cart = GetCart(),
                ReturnUrl = returnUrl });
        }
    }
}