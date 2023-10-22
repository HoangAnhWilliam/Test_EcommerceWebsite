using EcommerceWebsite.DAL;
using EcommerceWebsite.Models;
using EcommerceWebsite.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Mvc;

namespace EcommerceWebsite.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        public List<SelectListItem> GetCategory()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var cat = _unitOfWork.GetRepositoryInstance<Tbl_Category>().GetAllRecords();
            foreach(var item in cat)
            {
                list.Add(new SelectListItem { Value = item.CategoryId.ToString(), Text=item.CategoryName });
            }
            return list;
        }

        public ActionResult dashBoard()
        {
            return View();
        }

        public ActionResult Categories()
        {
            List<Tbl_Category> allcategories = _unitOfWork.GetRepositoryInstance<Tbl_Category>().GetAllRecordsIQueryable().Where(i => i.IsDelete==false).ToList();
            return View(allcategories);
        }

        public ActionResult AddCategory()
        {
            return UpdateCategory(0);
        }

        public ActionResult UpdateCategory(int categoryId) 
        {
            CategoryDetail cd;
                if(categoryId != 0)
            {
                cd = JsonConvert.DeserializeObject<CategoryDetail>(JsonConvert.SerializeObject(_unitOfWork.GetRepositoryInstance<Tbl_Category>().GetFirstOrDefault(categoryId)));
            }
                else
            {
                cd = new CategoryDetail();
            }
            return View("UpdateCategory", cd);
        }

        public ActionResult CategoryEdit(int catId)
        {
            return View(_unitOfWork.GetRepositoryInstance<Tbl_Category>().GetFirstOrDefault(catId));
        }

        [HttpPost]
        public ActionResult CategoryEdit(Tbl_Category tbl)
        {
            _unitOfWork.GetRepositoryInstance<Tbl_Category>().Update(tbl);
            return RedirectToAction("Categories");
        }

        public ActionResult Product()
        {
            return View(_unitOfWork.GetRepositoryInstance<Tbl_Product>().GetProduct());
        }

        public ActionResult ProductEdit(int productId)
        {
            ViewBag.CategoryList = GetCategory();
            return View(_unitOfWork.GetRepositoryInstance<Tbl_Product>().GetFirstOrDefault(productId));
        }

        [HttpPost]
        public ActionResult ProductEdit(Tbl_Product tbl,HttpPostedFileBase file, string pic)
        {
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImages/"), pic);
                file.SaveAs(path);
            }
            tbl.ProductImage = file != null ? pic : tbl.ProductImage;
            tbl.ModifiedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<Tbl_Product>().Update(tbl);
            return RedirectToAction("Product");
        }

        public ActionResult ProductAdd(int? productId)
        {
            ViewBag.CategoryList = GetCategory();
            return View();
        }

        [HttpPost]
        public ActionResult ProductAdd(Tbl_Product tbl, HttpPostedFileBase file)
        {
            string pic = null;
            if(file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImages/"), pic);
                file.SaveAs(path);
            }
            tbl.ProductImage = file != null ? pic : tbl.ProductImage;
            tbl.CreatedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<Tbl_Product>().Add(tbl);
            return RedirectToAction("Product");
        }
    }
}