using EcommerceWebsite.DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Razor.Text;

namespace EcommerceWebsite.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register

        dbMyOnlineShoppingEntities db = new dbMyOnlineShoppingEntities();

        public ActionResult Index()
        {
            return View(new Tbl_Members());
        }

        [HttpPost]
        public ActionResult Index(Tbl_Members model)
        {
            // Xử lý dữ liệu, có thể lưu vào database hoặc truyền sang trang Admin
            TempData["MemberData"] = model; // Lưu vào TempData để sử dụng ở trang khác
            return RedirectToAction("Members", "Admin");
        }

        public ActionResult LoginAdmin()
        {
            return View();
        }

        public JsonResult SaveData(Tbl_Members model)
        {
            model.IsActive = false;
            db.Tbl_Members.Add(model);
            db.SaveChanges();
            BuildEmailTemplate(model.MemberId);
            return Json("Registration Successful" , JsonRequestBehavior.AllowGet);
        }

        public ActionResult Confirm(int regId)
        {
            ViewBag.regID = regId;
            return View();
        }

        public JsonResult RegisterConfirm(int regId)
        {
            //Tbl_Members data = db.Tbl_Members.Where(x => x.MemberId == regId).FirstOrDefault();
            //data.IsActive = true;
            //db.SaveChanges();
            //var msg = "Your email is verified";
            //return Json(msg, JsonRequestBehavior.AllowGet);
            Tbl_Members data = db.Tbl_Members.FirstOrDefault(x => x.MemberId == regId);
            if (data != null)
            {
                data.IsActive = true;
                db.SaveChanges();

                // Gửi email xác nhận
                BuildEmailTemplate(regId);

                var msg = "Your email is verified";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var errorMsg = "Invalid registration ID";
                return Json(errorMsg, JsonRequestBehavior.AllowGet);
            }
        }

        private void BuildEmailTemplate(int regID)
        {
            //string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + "cshtml");
            //var regInfo = db.Tbl_Members.Where(x => x.MemberId == regID).FirstOrDefault();
            //var url = "http://localhost:57171/" + "Register/Confirm?regId=?" + regID;
            //body = body.Replace("@ViewBag.ConfirmationLink" , url);
            //body = body.ToString();
            //BuildEmailTemplate("Your Account Is Successfully Created" , body, regInfo.EmailId);
            string body = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplate/Text.cshtml"));
            var regInfo = db.Tbl_Members.FirstOrDefault(x => x.MemberId == regID);
            if (regInfo != null)
            {
                var url = "http://localhost:57171/Register/Confirm?regId=" + regID;
                body = body.Replace("@ViewBag.ConfirmationLink", url);
                BuildEmail("Your Account Is Successfully Created", body, regInfo.EmailId);
            }
        }

        public static void BuildEmail(string subjectText, string bodyText, string sendTo)
        {
            //string from, to, bcc, cc, subject, body;
            //from = "anhhoangtitan2k4@gmail.com";
            //to = sendTo.Trim();
            //bcc = "";
            //cc = "";
            //subject = subjectText;
            //StringBuilder sb = new StringBuilder();
            //sb.Append(bodyText);
            //body = sb.ToString();
            //MailMessage mail = new MailMessage(); // nếu bị lỗi thì ấn Ctrl  + Enter
            //mail.From = new MailAddress(from);
            //mail.To.Add(new MailAddress(to));
            //if (!string.IsNullOrEmpty(bcc))
            //{
            //    mail.Bcc.Add(new MailAddress(bcc));
            //}
            //if(!string.IsNullOrEmpty(cc))
            //{           
            //    mail.CC.Add(new MailAddress(cc));
            //}
            //mail.Subject = subject;
            //mail.Body = body;
            //mail.IsBodyHtml = true;
            //SendEmail(mail);
            string from, to, bcc, cc, subject, body;
            from = "mk"; // Thay thế bằng địa chỉ email của bạn
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            body = bodyText;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));

            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!string.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SendEmail(mail);
        }

        private static void SendEmail(MailMessage mail)
        {
            //SmtpClient client = new SmtpClient();
            //client.Host = "smtp.gmail.com";
            //client.Port = 587;
            //client.EnableSsl = true;
            //client.UseDefaultCredentials = false;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.Credentials = new System.Net.NetworkCredential("anhhoangtitan2k4@gmail.com", "mk"); // Sửa thành email và pasword của mình
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("anhhoangtitan2k4@gmail.com", "mk");
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public JsonResult CheckValidUser(Tbl_Members model)
        {
            string result = "Fail";
            var DataItem = db.Tbl_Members.Where(x => x.EmailId == model.EmailId && x.Password == model.Password).SingleOrDefault();
            if (DataItem != null)
            {
                Session["UserID"] = DataItem.MemberId.ToString();
                Session["UserName"] = DataItem.FirstName.ToString();
                result = "Success";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AfterLogin()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index");
        }
    }
}
