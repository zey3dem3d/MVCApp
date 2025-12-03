using Route.MVCApp.DAL.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.MVCApp.BLL.Common.Service.EmailSettings
{
    public interface IEmailSettings
    {
        public void SendEmail(Email email);
    }
}
