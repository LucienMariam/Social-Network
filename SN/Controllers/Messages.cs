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


namespace SN.Logics
{

    public class Messages : Controller
    {
        public List<string> With { get; set; }
        public List<string> Photo { get; set; }
        public List<string> Text { get; set; }
        public List<System.DateTime> Date { get; set; }

        public List<bool> IsRead { get; set; }

        public void SearchAllMessages(int userId)
        {
            UsersContext db = new UsersContext();

            List<SN.Models.Messages> messagesSqlRequest = (from Messages in db.Messages orderby Messages.Date where Messages.To == userId || Messages.From == userId select Messages).ToList();
            System.Collections.Generic.IEnumerable<System.Linq.IGrouping<int, SN.Models.Messages>> c=  messagesSqlRequest.GroupBy(m => m.To & m.From).ToList();
            With = new List<string>();
            Photo = new List<string>();
            Text = new List<string>();
            Date = new List<DateTime>();
            foreach(IGrouping<int, SN.Models.Messages> tempMessages in c)
            {

                if (tempMessages.Last().From == userId)
                {
                    With.Add(db.UserProfiles.Find(tempMessages.Last().To).UserName);
                    Photo.Add(db.Form.Find(tempMessages.Last().To).Photo);
                }
                else
                {
                    With.Add(db.UserProfiles.Find(tempMessages.Last().From).UserName);
                    Photo.Add(db.Form.Find(tempMessages.Last().From).Photo);
                }
                Text.Add(tempMessages.Last().Text);
                Date.Add(tempMessages.Last().Date);

            }
        }

        public void SearchMessagesHistory(int userId, int myId)
        {
            UsersContext db = new UsersContext();
            List<SN.Models.Messages> tempMessages = db.Messages.Where(m => (m.To == userId && m.From == myId) || (m.To == myId && m.From == userId)).OrderBy(m=>m.Date).ToList();

            With = new List<string>();
            Photo = new List<string>();
            Text = new List<string>();
            Date = new List<DateTime>();
            IsRead = new List<bool>();
            foreach (SN.Models.Messages message in tempMessages)
            {
                With.Add(db.UserProfiles.Find(message.From).UserName);
                Photo.Add(db.Form.Find(message.From).Photo);
                Text.Add(message.Text);
                Date.Add(message.Date);
                IsRead.Add(message.IsRead);
            }
            
        }

    }


}