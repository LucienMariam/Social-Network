using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.IO;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using SN.Filters;
using SN.Models;
using System.Net.Mail;
using System.Net;
using System.Web.Mvc.Html;
using System.Reflection;
using System.ComponentModel;

namespace SN.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/ HAHAHAHH
        public ActionResult ViewProfile(string username)
        {
            using (UsersContext db = new UsersContext())
            {
                int UserId = WebSecurity.GetUserId(username);
                if (!db.UserProfiles.Where(m=>m.UserName == username).Any())
                {
                    ModelState.AddModelError("", "Error, user does not exist");
                    return View();
                }
                ViewBag.NewMessages = (from Messages in db.Messages where Messages.To == UserId && Messages.IsRead == false select Messages).Count();
                ViewBag.Email = (from UserProfile in db.UserProfiles where UserProfile.UserName == username select UserProfile.Email).ToList().ElementAt(0);
                ViewBag.Username = username;
                if (!(from Form in db.Form where Form.UserId == UserId select Form).ToList().Any())
                {
                    ViewBag.Flag = false;
                    return View();
                }
                string directory = Url.Content(@"/Uploads/PhotoGallery/Luce/*");
                string[] files = Directory.GetFiles(@"C:\Users\SHUSTRICK\Documents\Visual Studio 2013\Projects\SN\SN\Uploads\PhotoGallery\" + WebSecurity.GetUserId(username), "*.*");
                ViewBag.PhotoGallery = new List<string>();
                if (files.Count() >= 3)
                {
                    for (int i = 0; i < 3; i++)
                        ViewBag.PhotoGallery.Add(String.Format("/Uploads/PhotoGallery/{0}/{1}", WebSecurity.GetUserId(username), Path.GetFileName(files[i])));
                        ViewBag.CountPhotoGallery = 3;
                }
                else
                { 
                    for (int i = 0; i < files.Count(); i++)
                        ViewBag.PhotoGallery.Add(String.Format("/Uploads/PhotoGallery/{0}/{1}", WebSecurity.GetUserId(username), Path.GetFileName(files[i])));
                        ViewBag.CountPhotoGallery = files.Count();
                }
                    
                int userId = WebSecurity.GetUserId(username);
                ViewBag.LikesCount = db.Likes.Where(m => m.To == userId && m.LikeOrDislike == true).Count();
                ViewBag.DisLikescount = db.Likes.Where(m => m.To == userId && m.LikeOrDislike == false).Count();
                ViewBag.About = db.Form.Find(userId).About;
                ViewBag.Day = db.Form.Find(userId).BirthDate.Day;
                ViewBag.Month = db.Form.Find(userId).BirthDate.Month;
                ViewBag.Year = db.Form.Find(userId).BirthDate.Year;
                ViewBag.BirthDate = String.Format("{0}{1}.{2}{3}.{4}", ViewBag.Day / 10, ViewBag.Day - (ViewBag.Day / 10) * 10, ViewBag.Month / 10, ViewBag.Month - (ViewBag.Month / 10) * 10, ViewBag.Year);
                ViewBag.Interests = db.Form.Find(userId).Interests;
                ViewBag.IsSearching = db.Form.Find(userId).IsSearching;
                ViewBag.Photo = db.Form.Find(userId).Photo;
                ViewBag.Sex = db.Form.Find(userId).WhichSex;
                ViewBag.SexPreferences = db.Form.Find(userId).SexPreferences;

            }
            return View();
        }

        public ActionResult PhotoGallery(string username)
        {
            string directory = Url.Content(@"/Uploads/PhotoGallery/Luce/*");
            string[] files = Directory.GetFiles(@"C:\Users\SHUSTRICK\Documents\Visual Studio 2013\Projects\SN\SN\Uploads\PhotoGallery\" + WebSecurity.GetUserId(username), "*.*");
            ViewBag.PhotoGallery = new List<string>();
            for (int i = 0; i < files.Count(); i++)
                ViewBag.PhotoGallery.Add(String.Format("/Uploads/PhotoGallery/{0}/{1}", WebSecurity.GetUserId(username), Path.GetFileName(files[i])));
            ViewBag.Count = files.Count();
            ViewBag.username = username;
            return View();
        }

        [Authorize]
        public ActionResult AddPhoto()
        {
            string filePath = "";
            if (Request.Files.Count > 0)
            {
                foreach (string inputTagName in Request.Files)
                {

                    HttpPostedFileBase file = Request.Files[inputTagName];
                    if (file.ContentLength > 0 && file.ContentLength <= 2000000 && (Path.GetExtension(file.FileName) == ".jpg" || Path.GetExtension(file.FileName) == ".JPEG" || Path.GetExtension(file.FileName) == ".JPG" || Path.GetExtension(file.FileName) == ".jpeg"))
                    {
                        filePath = Path.Combine(HttpContext.Server.MapPath("../Uploads/PhotoGallery/" + WebSecurity.GetUserId(User.Identity.Name)), Path.GetFileName(file.FileName));
                        file.SaveAs(filePath);
                    }

                }
            }
            return View();
        }


        [AuthorizeAttribute]
        public ActionResult AddLike(string username)
        {
            if (username == User.Identity.Name)
                return RedirectToAction("ViewProfile", new { Username = username });
            using (UsersContext db = new UsersContext())
            {
                int userIdMe = WebSecurity.GetUserId(User.Identity.Name);
                int userIdTo = WebSecurity.GetUserId(username);
                Likes tempLike = db.Likes.Where(m => m.From == userIdMe && m.To == userIdTo).FirstOrDefault();
                if (tempLike != null)
                {
                    if (tempLike.LikeOrDislike == false)
                        tempLike.LikeOrDislike = true;
                    else
                        db.Likes.Remove(tempLike);
                    db.SaveChanges();
                }

                else
                {
                    db.Likes.Add(new Likes { From = userIdMe, To = userIdTo, LikeOrDislike = true });
                    db.SaveChanges();
                }
            }
            return RedirectToAction("ViewProfile", new { Username = username });
        }
        [AuthorizeAttribute]
        public ActionResult AddDisLike(string username)
        {
            if (username == User.Identity.Name)
                return RedirectToAction("ViewProfile", new { Username = username });
            using (UsersContext db = new UsersContext())
            {
                int userIdMe = WebSecurity.GetUserId(User.Identity.Name);
                int userIdTo = WebSecurity.GetUserId(username);
                Likes tempLike = db.Likes.Where(m => m.From == userIdMe && m.To == userIdTo).FirstOrDefault();
                if (tempLike != null)
                {
                    if (tempLike.LikeOrDislike == true)
                        tempLike.LikeOrDislike = false;
                    else
                        db.Likes.Remove(tempLike);
                    db.SaveChanges();
                }

                else
                {
                    db.Likes.Add(new Likes { From = userIdMe, To = userIdTo, LikeOrDislike = false });
                    db.SaveChanges();
                }
            }
            return RedirectToAction("ViewProfile", new { Username = username });
        }


        [AuthorizeAttribute]
        public ActionResult Form()
        {
            FormModel model = new FormModel();
            UsersContext db = new UsersContext();
            int userId = WebSecurity.GetUserId(User.Identity.Name);
            Form tempForm = db.Form.Find(userId);
            model.Photo = tempForm.Photo;
            model.Interests = tempForm.Interests;
            model.About = tempForm.About;
            model.BirthDay = tempForm.BirthDate.Day;
            model.BirthYear = tempForm.BirthDate.Year;
            model.BirthMonth = (Month)(tempForm.BirthDate.Month - 1);
            ViewBag.Days = new List<int>();
            for (int i = 1; i <= 31; i++ )
            {
                ViewBag.Days.Add(i);
            }
            ViewBag.Years = new List<int>();
            for (int i = 1900; i < DateTime.Now.Year; i++ )
            {
                ViewBag.Years.Add(i);
            }
                // model.BirthDate = String.Format("{0}{1}.{2}{3}.{4}", tempForm.BirthDate.Day / 10, tempForm.BirthDate.Day - (tempForm.BirthDate.Day / 10) * 10, tempForm.BirthDate.Month / 10, tempForm.BirthDate.Month - (tempForm.BirthDate.Month / 10) * 10, tempForm.BirthDate.Year);
            if (tempForm.IsSearching == true)
                model.IsSearching = YesNo.Yes;
            else
                model.IsSearching = YesNo.No;
            return View(model);
        }



        [AuthorizeAttribute]
        [HttpPost]
        public ActionResult Form(FormModel model)
        {
            ViewBag.Days = new List<int>();
            for (int i = 1; i <= 31; i++)
            {
                ViewBag.Days.Add(i);
            }
            ViewBag.Years = new List<int>();
            for (int i = 1900; i < DateTime.Now.Year; i++)
            {
                ViewBag.Years.Add(i);
            }
            if (ModelState.IsValid)
            {
                UsersContext db = new UsersContext();
                int userId = WebSecurity.GetUserId(User.Identity.Name);
                Form tempForm = db.Form.Find(userId);
                tempForm.About = model.About;
                tempForm.Interests = model.Interests;
                tempForm.SexPreferences = model.SexPreferences;
                tempForm.BirthDate = new DateTime(model.BirthYear, (int)model.BirthMonth + 1, model.BirthDay);
                tempForm.IsSearching = (model.IsSearching == YesNo.Yes);
                tempForm.WhichSex = model.WhichSex;
                string filePath;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    if (file.ContentLength > 0 && file.ContentLength <= 2000000 && (Path.GetExtension(file.FileName) == ".jpg" || Path.GetExtension(file.FileName) == ".JPEG" || Path.GetExtension(file.FileName) == ".JPG" || Path.GetExtension(file.FileName) == ".jpeg"))
                    {
                        filePath = Path.Combine(HttpContext.Server.MapPath("../Uploads/"),  userId.ToString()+ Path.GetExtension(file.FileName));
                        file.SaveAs(filePath);
                        filePath = String.Format("{0}{1}", "~/Uploads/", Path.GetFileName(filePath));
                        tempForm.Photo = filePath;
                    }
                }

                db.SaveChanges();
                
            }
            return View(model);
        }
    }
}
