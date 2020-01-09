
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.Extensions;
//using Microsoft.Extensions.DependencyInjection;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Text;
//namespace Models.Models
//{
//    public class SessionCart : Cart
//    {
//        public static Cart GetCart(IServiceProvider services)
//        {
//            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
//            SessionCart cart = session?.GetJson<SessionCart>("Cart") ?? new SessionCart();
//            cart.Session = session;
//            return cart;
//        }
//        [JsonIgnore]
//        public ISession Session { get; set; }

//        public override void AddItem(Products product, int quantity)
//        {
//            base.AddItem(product, quantity);
//            Session.SetJson("Cart", this);
//        }

//        public override void RemoveLine(Products product)
//        {
//            base.RemoveLine(product);
//            Session.SetJson("Cart", this);
//        }
//        public override void Clear()
//        {
//            base.Clear();
//            Session.Remove("Cart");
//        }
//    }

//}
