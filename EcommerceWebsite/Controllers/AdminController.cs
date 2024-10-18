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
        dbMyOnlineShoppingEntities database = new dbMyOnlineShoppingEntities();

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

        public ActionResult Members()
        {
            if (TempData["MemberData"] != null)
            {
                var memberData = TempData["MemberData"] as Tbl_Members;
                return View(memberData);

            }
            var allMember = database.Tbl_Members.ToList();
            return View(allMember);
        }

        [HttpGet]
        public ActionResult MemberAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MemberAdd(Tbl_Members member)
        {
            try
            {
                database.Tbl_Members.Add(member);
                database.SaveChanges();
                return RedirectToAction("Members");
            }
            catch
            {
                return Content("Error");
            }
        }

        [HttpGet]
        public ActionResult MemberEdit(int? memberId)
        {
            if (memberId == null)
            {
                // Xử lý khi memberId là null, có thể redirect hoặc trả về lỗi
                return RedirectToAction("Error");
            }
            var member = database.Tbl_Members.Where(x => x.MemberId == memberId).FirstOrDefault();
            return View(member);
        }

        [HttpPost]
        public ActionResult MemberEdit(Tbl_Members member)
        {
            database.Entry(member).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Members");
        }

        [HttpGet]
        public ActionResult MemberDelete(int? memberId)
        {
            if(memberId == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var member = database.Tbl_Members.Where(x => x.MemberId == memberId).FirstOrDefault();
            if(member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        [HttpPost, ActionName("MemberDelete")]
        public ActionResult MemberDeleteConfirmed(int memberId)
        {
            try
            {
                var member = database.Tbl_Members.Where(x => x.MemberId == memberId).FirstOrDefault();
                database.Tbl_Members.Remove(member);
                database.SaveChanges();
                return RedirectToAction("Members");
            }
            catch
            {
                return Content("Error");
            }
        }
    }
}