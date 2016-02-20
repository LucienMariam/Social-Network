using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
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

namespace SN.Logics
{
    public class UserRegistration
    {
        public static bool SendEmailMessage(string email, string username, string message)
        {
            string from = "nod1994@yandex.ru";
            string password = "viru$911";
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("nod1994@yandex.ru");
            mail.To.Add(new MailAddress(email));
            mail.Subject = "Registration";
            mail.Body = String.Format("Hello, {0}! You have registred in our social network. Thank you.\nPlease, confirm your account: http://localhost:23746/Account/Confirmation/{1}/{2}", username, username, message);
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.yandex.ru";
            client.Port = 587;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(from.Split('@')[0], password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(mail);
            mail.Dispose();
            return true;
        }
    }

}