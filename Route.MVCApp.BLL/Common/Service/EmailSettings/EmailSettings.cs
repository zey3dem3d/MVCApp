using Route.MVCApp.DAL.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Route.MVCApp.BLL.Common.Service.EmailSettings
{
    public class EmailSettings : IEmailSettings
    {
        public void SendEmail(Email email)
        {
            // 1. Mail Server : [Gmail, Yahoo, Outlook]
            // SMTP : Simple Mail Transfer Protocol

            var Client = new SmtpClient("smtp.gmail.com", 587);
            Client.EnableSsl = true;

            // Sender & Receiver

            Client.Credentials = new NetworkCredential("zeyad.emad.dev@gmail.com", "xlmockjcbswtlkyg");

            Client.Send("zeyad.emad.dev@gmail.com", email.To, email.Subject, email.Body);
        }
    }
}
