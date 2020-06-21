using WebApplication13.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace WebApplication13.Email
{

    public static class EmailTemplates
    {

        static string EmailTemplate;
        // static string CurrentLocation = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
        static string path = String.Format("{0}", ConfigurationManager.AppSettings["TemplatesLocation"]);
        //access folder get templates        
        public static string GetPCKEmail(string recepientName)
        {
            //Get Template
            EmailTemplate = ReadPhysicalFile(path + "/PCK.html");
            //Fill Content
            string emailMessage = EmailTemplate
            .Replace("{recepientName}", recepientName.ToString());


            return emailMessage;
        }
        public static string GetConfirmDatHangEmail(string recepientName, string TenKhachHang, string SoLuong,
            string TongTien, string NgayMua, string Email, string DiaChi, string NgayGiaoDuKien, string CallBackUrl)
        {
            //Get Template
            EmailTemplate = ReadPhysicalFile(path + "/ConfirmDatHang.html");
            //Fill Content
            string emailMessage = EmailTemplate
            .Replace("{recepientName}", recepientName.ToString())
            .Replace("{TenKhachHang}", TenKhachHang.ToString())
            .Replace("{SoLuong}", SoLuong.ToString())
            .Replace("{TongTien}", TongTien.ToString())
            .Replace("{NgayMua}", NgayMua.ToString())
            .Replace("{Email}", Email.ToString())
            .Replace("{DiaChi}", DiaChi.ToString())
            .Replace("{NgayGiaoDuKien}", NgayGiaoDuKien.ToString())
            .Replace("{CallBackUrl}", CallBackUrl.ToString());


            return emailMessage;
        }
        public static string ThongBaoDonHangMoi(string recepientName, string TenKhachHang, string SoLuong,
          string TongTien, string NgayMua, string Email, string DiaChi)
        {
            //Get Template
            EmailTemplate = ReadPhysicalFile(path + "/ThongBaoDonHangMoi.html");
            //Fill Content
            string emailMessage = EmailTemplate
            .Replace("{recepientName}", recepientName.ToString())
            .Replace("{TenKhachHang}", TenKhachHang.ToString())
            .Replace("{SoLuong}", SoLuong.ToString())
            .Replace("{TongTien}", TongTien.ToString())
            .Replace("{NgayMua}", NgayMua.ToString())
            .Replace("{Email}", Email.ToString())
            .Replace("{DiaChi}", DiaChi.ToString());

            return emailMessage;
        }
        private static string ReadPhysicalFile(string path)
        {
            try
            {
                using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.ReadWrite))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return e.Message;
            }
        }


    }
}

