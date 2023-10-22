using Newtonsoft.Json;
using EcommerceWebsite.DAL;
using EcommerceWebsite.Models;
using EcommerceWebsite.Repository;
using EcommerceWebsite.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceWebsite.Controllers
{
    public class HomeController : Controller
    {
        dbMyOnlineShoppingEntities context = new dbMyOnlineShoppingEntities();
        public ActionResult Index(string search, int? page)
        {
            HomeIndexViewModel model = new HomeIndexViewModel();
            HomeIndexViewModel viewModel = model.CreateModel(search, null , 4);
            return View(viewModel);
        }

        public ActionResult CheckOutDetails()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AddToCart(int productId)
        {
            List<Item> cart;

            if (Session["cart"] == null)
            {
                cart = new List<Item>();
                Session["cart"] = cart;
            }
            else
            {
                cart = (List<Item>)Session["cart"];
            }

            var product = context.Tbl_Product.Find(productId);
            var existingCartItem = cart.FirstOrDefault(item => item.product.ProductId == productId);

            if (existingCartItem != null)
            {
                existingCartItem.quantity++;
            }
            else
            {
                cart.Add(new Item { product = product, quantity = 1 });
            }

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int productId)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            //var product = context.Tbl_Product.Find(productId);
            foreach (var item in cart)
            {
                if (item.product.ProductId == productId)
                {
                    cart.Remove(item);
                    break;
                }
            }
            Session["cart"] = cart;
            return Redirect("Index");
        }
    }
}