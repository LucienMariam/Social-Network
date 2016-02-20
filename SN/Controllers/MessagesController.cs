using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SN.Models;


using System.Linq.Expressions;
using System.Transactions;

using System.IO;

using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using SN.Filters;

using System.Net.Mail;
using System.Net;
using System.Web.Mvc.Html;
using System.Reflection;
using System.ComponentModel;
using SN.Logics;

namespace SN.Controllers
{
    public class MessagesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [AuthorizeAttribute]
        public ActionResult Messages(SN.Models.Messages model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                SN.Logics.Messages allMessages = new SN.Logics.Messages();
                allMessages.SearchAllMessages(WebSecurity.GetUserId(User.Identity.Name));
                ViewBag.Photo = allMessages.Photo;
                ViewBag.With = allMessages.With;
                ViewBag.Count = allMessages.With.Count;
                ViewBag.Text = allMessages.Text;
                ViewBag.Date = allMessages.Date;
            }
            return View();
        }

        [AuthorizeAttribute]
        public ActionResult MessagesHistory(SN.Models.Messages model, string username)
        {
            
           if (ModelState.IsValid)
            {
                SN.Logics.Messages messagesHistory = new SN.Logics.Messages();
                messagesHistory.SearchMessagesHistory(WebSecurity.GetUserId(username), WebSecurity.GetUserId(User.Identity.Name));
                using (UsersContext db = new UsersContext())
                {
                    ViewBag.With = username;
                    ViewBag.Photo = messagesHistory.Photo;
                    ViewBag.username = username;
                    ViewBag.From = messagesHistory.With;

                    ViewBag.Text = messagesHistory.Text;
                    ViewBag.Date = messagesHistory.Date;
                    ViewBag.IsRead = messagesHistory.IsRead;
                    ViewBag.Count = messagesHistory.With.Count();
                }
            }
            return View();
        }

        [AuthorizeAttribute]
        public ActionResult SendMessage(SendMessageModel model, string username)
        {
            if (username == User.Identity.Name || !ModelState.IsValid)
            {
                return View(model);
            }
                using (UsersContext db = new UsersContext())
                {
                      List<string> temp = (from UserProfile in db.UserProfiles where UserProfile.UserName == username select UserProfile.UserName).ToList();
                        if (!temp.Any())
                       {
                            ModelState.AddModelError("", "Error, user does not exist");
                            return View(model);
                        }
                        
                    db.Messages.Add(new SN.Models.Messages { From = WebSecurity.GetUserId(User.Identity.Name), Date = DateTime.Now, IsRead = false, Text = model.Text, To = WebSecurity.GetUserId(username) });
                    db.SaveChanges();                      
                }
                return RedirectToAction("Messages", "Messages");
        }
    }
}
