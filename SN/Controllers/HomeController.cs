using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using SN.Models;

namespace SN.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using(UsersContext db = new UsersContext())
            {
                db.Likes.Where(m => m.LikeOrDislike == true).Count();
                var likes = db.Likes.Where(m => m.LikeOrDislike == true).GroupBy(m => m.To).ToList().OrderBy(m => m.Count()).Reverse().Take(6);
                ViewBag.photo = new List<string>();
                ViewBag.users = new List<string>();
                ViewBag.likesCount = new List<int>();
                ViewBag.disLikesCount = new List<int>();
                foreach(var group in likes)
                {
                    foreach(Likes like in group)
                    {
                        ViewBag.photo.Add(db.Form.Find(like.To).Photo);
                        ViewBag.users.Add(db.UserProfiles.Find(like.To).UserName);
                        ViewBag.likesCount.Add(group.Count());
                        ViewBag.disLikesCount.Add(0);
                        break;
                    }
                }
            }
            return View();
        }

    }
}
