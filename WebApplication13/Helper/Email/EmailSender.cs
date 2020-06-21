
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;
using System.Drawing;
using MimeKit.Utils;
using System.Configuration;
using WebApplication13.Helper;
using WebApplication13.Models;

namespace WebApplication13.Email
{
    public interface IEmailSender
    {
        Task<(bool success, string errorMsg)> SendEmailAsync(MailboxAddress sender, MailboxAddress[] recepients, MailboxAddress[] ccEmail, string subject, string body, SmtpConfig config = null, bool isHtml = true);
        Task<(bool success, string errorMsg)> SendEmailAsync(string recepientName, string recepientEmail, string ccEmail, string subject, string body, SmtpConfig config = null, bool isHtml = true);
        Task<(bool success, string errorMsg)> SendEmailAsync(string senderName, string senderEmail, string recepientName, string recepientEmail, string ccEmail, string subject, string body, SmtpConfig config = null, bool isHtml = true);

        Task<bool> PCKAsync(string recepientName, string recepientEmail, string ccEmail = null);
        Task<bool> ConfirmDatHangAsync(string recepientName, string recepientEmail, string TenKhachHang, string SoLuong,
            string TongTien, string NgayMua, string Email, string DiaChi, string NgayGiaoDuKien, string CallBackUrl, string ccEmail = null);
        Task<bool> ThongBaoDonHangMoiAsync(string recepientName, string recepientEmail, string TenKhachHang, string SoLuong,
            string TongTien, string NgayMua, string Email, string DiaChi, string ccEmail = null);
    }
    public class EmailSender : SmtpConfig, IEmailSender
    {
        Boolean hasAttachment = false;
        //string pathQRCodeLocation = String.Format("{0}", ConfigurationManager.AppSettings["QRCodeLocation"]);
        string path = String.Format("{0}", ConfigurationManager.AppSettings["TemplatesLocation"]);
        string Imgpath = String.Format("{0}", ConfigurationManager.AppSettings["ImageLocation"]);

        //static string QRCodeLocation;

        public async Task<(bool success, string errorMsg)> SendEmailAsync(
            string recepientName,
            string recepientEmail,
            string ccEmail,
            string subject,
            string body,
            SmtpConfig config = null,
            bool isHtml = true)
        {
            var from = new MailboxAddress(Name, EmailAddress);
            var to = new MailboxAddress(recepientName, recepientEmail);
            if (ccEmail != null)
            {
                string[] ccList = ccEmail.Split(',');
                List<MailboxAddress> cc = new List<MailboxAddress>();
                foreach (string temp in ccList)
                {
                    cc.Add(new MailboxAddress(temp));
                }
                return await SendEmailAsync(from, new MailboxAddress[] { to }, cc.ToArray(), subject, body, config, isHtml);
            }
            else
            {
                return await SendEmailAsync(from, new MailboxAddress[] { to }, null, subject, body, config, isHtml);
            }
        }
        public async Task<(bool success, string errorMsg)> SendEmailAsync(
            string senderName,
            string senderEmail,
            string recepientName,
            string recepientEmail,
            string ccEmail,
            string subject,
            string body,
            SmtpConfig config = null,
            bool isHtml = true)
        {
            var from = new MailboxAddress(senderName, senderEmail);
            var to = new MailboxAddress(recepientName, recepientEmail);
            var cc = new MailboxAddress(ccEmail);
            return await SendEmailAsync(from, new MailboxAddress[] { to }, new MailboxAddress[] { cc }, subject, body, config, isHtml);
        }
        public async Task<(bool success, string errorMsg)> SendEmailAsync(
            MailboxAddress sender,
            MailboxAddress[] recepients,
            MailboxAddress[] ccEmail,

            string subject,
            string body,
            SmtpConfig config = null,
            bool isHtml = true)
        {

            MimeMessage message = new MimeMessage();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //var AdminNtidList = db.UserRoleMappings.Where(s => s.Role == "admin1").Select(s => s.NTID).ToArray();
                //var AdminEmailList = db.vms_Users.Where(x => AdminNtidList.Contains(x.user_ntid)).Select(x => x.email_address).ToArray();
                //var AdminEmail = string.Join(",", AdminEmailList);

                message.From.Add(sender);
                message.To.AddRange(recepients);
                message.Subject = subject;
                //message.Cc.AddRange(InternetAddressList.Parse(AdminEmail));
                if (ccEmail != null)
                {
                    message.Cc.AddRange(ccEmail);
                }
                var builder = new BodyBuilder();
                builder.HtmlBody = body;
                if (hasAttachment == true)
                {
                    builder.Attachments.Add(path + "\\testing.xlsx");
                }

                var HeaderImage = builder.LinkedResources.Add(Imgpath + "\\header.png");
                HeaderImage.ContentId = "header";
                //var CompanylogoImage = builder.LinkedResources.Add(path + "\\bosch_logo.png");
                //CompanylogoImage.ContentId = "companylogo";
                var FooterImage = builder.LinkedResources.Add(Imgpath + "\\footer.png");
                FooterImage.ContentId = "footer";
                message.Body = builder.ToMessageBody();
            }
            try
            {
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(SendHost, SendPort, false);
                    client.Authenticate(Account, Password);
                    client.Send(message);
                    client.Disconnect(true);
                }
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<bool> PCKAsync(string recepientName, string recepientEmail, string ccEmail = null)
        {
            this.hasAttachment = true;
            string message = EmailTemplates.GetPCKEmail(recepientName);
            (bool success, string errorMsg) = await this.SendEmailAsync(recepientName, recepientEmail, ccEmail, "Visitor Management System – Approval completed", message);
            if (success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ConfirmDatHangAsync(string recepientName, string recepientEmail, string TenKhachHang, string SoLuong,
            string TongTien, string NgayMua, string Email, string DiaChi, string NgayGiaoDuKien, string CallBackUrl, string ccEmail = null)
        {
            this.hasAttachment = false;
            string message = EmailTemplates.GetConfirmDatHangEmail(recepientName, TenKhachHang, SoLuong, TongTien, NgayMua, Email, DiaChi, NgayGiaoDuKien, CallBackUrl);
            (bool success, string errorMsg) = await this.SendEmailAsync(recepientName, recepientEmail, ccEmail, "Xác Nhận Đơn Đặt Hàng", message);
            if (success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //public async Task<bool> ThongBaoDonHangMoiAsync(string recepientName, string recepientEmail, string TenKhachHang, string SoLuong,
        //   string TongTien, string NgayMua, string Email, string DiaChi, string ccEmail = null)
        //{
        //    this.hasAttachment = false;
        //    string message = EmailTemplates.ThongBaoDonHangMoi(recepientName, TenKhachHang, SoLuong, TongTien, NgayMua, Email, DiaChi);
        //    (bool success, string errorMsg) = await this.SendEmailAsync(recepientName, recepientEmail, ccEmail, "Thông Báo Đơn Hàng Mới", message);
        //    if (success)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        public async Task<bool> ThongBaoDonHangMoiAsync(string recepientName, string recepientEmail, string TenKhachHang, string SoLuong, string TongTien, string NgayMua, string Email, string DiaChi, string ccEmail = null)
        {
            this.hasAttachment = false;
            string message = EmailTemplates.ThongBaoDonHangMoi(recepientName, TenKhachHang, SoLuong, TongTien, NgayMua, Email, DiaChi);
            (bool success, string errorMsg) = await this.SendEmailAsync(recepientName, recepientEmail, ccEmail, "Thông Báo Đơn Hàng Mới", message);
            if (success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

