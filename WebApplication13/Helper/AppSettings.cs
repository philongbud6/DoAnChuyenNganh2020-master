using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WebApplication13
{
    public class AppSettings
    {
        public SmtpConfig SmtpConfig { get; set; }

    }



    public class SmtpConfig
    {
        public static string SendHost = "smtp.gmail.com";
        public static int SendPort = 587;
        public static string Name = "WebBanHang";
        public static string Username = "WebBanHang";
        public static string EmailAddress = "truongta9701@gmail.com";
        public static string Account = "truongta9701@gmail.com";
        public static string Password = "lvyudrjznoxpaomy";
    }
}

