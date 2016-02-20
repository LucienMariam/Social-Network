using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SN.Models;

namespace SN.Controllers
{
    public class SearchController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Ages = new List<int>();
            for (int i = 7; i <= 150; i++)
            {
                ViewBag.Ages.Add(i);
            }
            ViewBag.Count = 0;
            return View();
        }
        [HttpPost]
        public ActionResult Index(SearchModel model)
        {

            ViewBag.Ages = new List<int>();
            for (int i = 7; i <= 150; i++)
            {
                ViewBag.Ages.Add(i);
            }
            if (model.MaxAge == model.MinAge)
            {
                model.MaxAge = 140;
                model.MinAge = 7;
            }
            List<Form> tempForms = new List<Form>();
            List<Form> tf = new List<Form>();
            List<UserProfile> tempId;
            UsersContext db = new UsersContext();
            ViewBag.usernames = new List<string>();
            ViewBag.userphotos = new List<string>();
            
            if (model.Username != null)
            {
                tempId = db.UserProfiles.Where(m => m.UserName.Contains(model.Username)).ToList();
                if(tempId.Any())
                {
                    foreach(UserProfile up in tempId)
                    {
                        tempForms.Add(db.Form.Find(up.UserId));
                    }
                }
            }
            
            if(model.Text != null)
            {
                tf = db.Form.Where(m => m.Interests.Contains(model.Text) || m.About.Contains(model.Text)).ToList();
                foreach(Form f in tf)
                {
                    if(!tempForms.Where(m=>m.UserId == f.UserId).Any())
                    {
                        tempForms.Add(f);
                    }
                }
            }
            

            for(int i = 0; i < tempForms.Count(); i ++)
             {
                 if ((DateTime.Now.Year - tempForms[i].BirthDate.Year) < model.MinAge || (DateTime.Now.Year - tempForms[i].BirthDate.Year) > model.MaxAge)
                 {
                     tempForms.Remove(tempForms[i]);
                     i--;
                 }
             }

             if (model.FilterBySex == true)
             {
                 for (int i = 0; i < tempForms.Count(); i++)
                 {
                     if (model.WhichSex != tempForms[i].WhichSex)
                     {
                         tempForms.Remove(tempForms[i]);
                         i--;
                     }
                 }
             }
            
         foreach (Form form in tempForms)
         {
             ViewBag.usernames.Add(db.UserProfiles.Find(form.UserId).UserName);
             ViewBag.userphotos.Add(form.Photo);
         }

            ViewBag.count = tempForms.Count();
            return View(model);
        }

    }
}
