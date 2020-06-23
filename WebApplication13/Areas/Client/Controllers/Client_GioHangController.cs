using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication13.Areas.Client.Models;
using WebApplication13.Email;
using WebApplication13.Helper;
using WebApplication13.Models;
namespace WebApplication13.Areas.Client.Controllers
{
    public class Client_GioHangController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        #region "Gio hang"
        // GET: Client/Client_GioHang
        public ActionResult Index()
        {
            return View();
        }
        public List<Client_GioHang> LayClient_GioHang()
        {
            List<Client_GioHang> listClient_GioHang = Session["Client_GioHang"] as List<Client_GioHang>;
            if (listClient_GioHang == null)
            {
                listClient_GioHang = new List<Client_GioHang>();
                Session["Client_GioHang"] = listClient_GioHang;
            }
            return listClient_GioHang;
        }
        public ActionResult ThemClient_GioHang(int cSanPhamId, string strURL)
        {
            List<Client_GioHang> listClient_GioHang = LayClient_GioHang();
            Client_GioHang sanpham = listClient_GioHang.Find(n => n.cSanPhamId == cSanPhamId);
            if (sanpham == null)
            {
                sanpham = new Client_GioHang(cSanPhamId);
                listClient_GioHang.Add(sanpham);
                ViewBag.message = "Them Thanh Cong";
                return Redirect(strURL);
            }
            else
            {
                sanpham.cSoLuong++;
                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            int gTongSoLuong = 0;

            List<Client_GioHang> listClient_GioHang = Session["Client_GioHang"] as List<Client_GioHang>;
            if (listClient_GioHang != null)
            {
                gTongSoLuong = listClient_GioHang.Sum(n => n.cSoLuong);
            }
            return gTongSoLuong;
        }
        private float ChietKhau()
        {
            float gChietKhau = 0;

            List<Client_GioHang> listClient_GioHang = Session["Client_GioHang"] as List<Client_GioHang>;
            if (listClient_GioHang != null)
            {
                gChietKhau = listClient_GioHang.Sum(n => n.cGiamGia);
            }
            return gChietKhau;
        }
        private int TongTien()
        {
            int gTongTien = 0;
            List<Client_GioHang> listClient_GioHang = Session["Client_GioHang"] as List<Client_GioHang>;
            if (listClient_GioHang != null)
            {
                gTongTien = listClient_GioHang.Sum(n => n.cThanhTien);
            }
            return gTongTien;
        }
        public ActionResult XoaClient_GioHang(int csanphamid)
        {
            //Lấy giỏ hànng từ session
            List<Client_GioHang> listGioHang = LayClient_GioHang();
            Client_GioHang sanpham = listGioHang.SingleOrDefault(n => n.cSanPhamId == csanphamid);
            if (sanpham != null)
            {
                listGioHang.RemoveAll(n => n.cSanPhamId == csanphamid);
                return RedirectToAction("Client_GioHang");
            }
            if (listGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Client_TrangChu");
            }
            return RedirectToAction("Client_GioHang");
        }
        public ActionResult CapNhatClient_GioHang(int csanphamid, FormCollection f)
        {
            List<Client_GioHang> listClient_GioHang = LayClient_GioHang();
            Client_GioHang sanpham = listClient_GioHang.SingleOrDefault(n => n.cSanPhamId == csanphamid);
            if (sanpham != null)
            {
                sanpham.cSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("Client_GioHang");


        }
        [HttpGet]
        public ActionResult Client_GioHang()
        {
            List<Client_GioHang> listClient_GioHang = LayClient_GioHang();
            if (listClient_GioHang.Count == 0)
            {
                ViewBag.message = "";
                return RedirectToAction("Index", "Client_TrangChu");
            }
            if (listClient_GioHang.Count == 1)
            {
                foreach (var item in listClient_GioHang)
                {
                    ViewBag.CheckMaSP = item.cMaSanPham;
                    ViewBag.MaKhoHang = new SelectList(db.SanPhams.Where(n => n.MaSanPham == item.cMaSanPham).Include(n => n.KhoHang).Select(n => n.KhoHang.TenKho), "TenKho");
                }
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(listClient_GioHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Client_GioHang(FormCollection collection, string RadSDT)
        {
            string tenKho = collection["MaKhoHang"];
            var KhoHangId = db.KhoHangs.SingleOrDefault(n => n.TenKho == tenKho).KhoHangId;
            DonHang DH = new DonHang();
            List<Client_GioHang> gh = LayClient_GioHang();
            DH.KhachHangId = 1;
            DH.CuaHangId = KhoHangId;
            DH.NgayMua = DateTime.Now;
            DH.SoLuongBan = TongSoLuong();
            int tongtien = TongTien();
            DH.TongTien = TongTien();
            DH.TokenKey = RandomNumber(6);
            DH.TrangThaiDH = EnumExtensions.GetDescription(Enumstatus.NotConfirm);
            string tenform = collection["TenForm"];

            switch (tenform)
            {
                case "form1":
                    DH.HinhThucTT = EnumExtensions.GetDescription(Enumstatus.SHIP_COD);
                    break;
                case "form2":
                    DH.HinhThucTT = EnumExtensions.GetDescription(Enumstatus.TT_Online);
                    break;
                case "form3":
                    DH.HinhThucTT = EnumExtensions.GetDescription(Enumstatus.Pick_Up);
                    break;
            }

            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            db.DonHangs.Add(DH);
            db.SaveChanges();
            foreach (var item in gh)
            {
                CTDonHang CTDH = new CTDonHang();
                CTDH.DonhangId = DH.DonHangId;
                CTDH.SanPhamId = item.cSanPhamId;
                CTDH.SoLuong = item.cSoLuong;
                CTDH.DonGia = item.cDonGia;
                CTDH.GiamGia = item.cGiamGia;
                db.CTDonHangs.Add(CTDH);
            }
            db.SaveChanges();
            Session["Client_GioHang"] = null;
            if (collection["rad"] == "RadSDT")
            {
                string ToNumber = "+84" + collection["Sdt"].Substring(1);
                ServiceSMS sms = new ServiceSMS();
                sms.SendSMS(ToNumber, DH.TokenKey);
                return RedirectToAction("VerifySMS");
            }
            if (collection["rad"] == "RadMail")
            {
                SendMail();
                return RedirectToAction("VerifyMail");

            }
            return View();
            void SendMail()
            {
                var tenKH = collection["TenKH"].Trim(',');
                var Sdt = collection["Sdt"].Trim(',');
                var Diachi = collection["Diachi"].Trim(',');
                var Email = collection["Email"].Trim(',');
                DateTime NgayGiaoDuKien = DH.NgayMua.AddDays(3);
                var callbackUrl = "http://Localhost:54655" + Url.Action("ConfirmDH", "Client_GioHang", new { @TokenKey = DH.TokenKey });
                EmailSender email = new EmailSender();
                email.ConfirmDatHangAsync(tenKH, Email, tenKH, DH.SoLuongBan.ToString(), string.Format("{0:0,0}", DH.TongTien), DH.NgayMua.ToShortDateString(), Email, Diachi, NgayGiaoDuKien.ToShortDateString(), callbackUrl);
            }
        }

        public ActionResult VerifySMS()
        {
            return View();
        }
        public ActionResult VerifyMail()
        {
            return View();
        }
        public ActionResult ConfirmSMS(FormCollection collection)
        {
            string token = collection["Token"];

            return RedirectToAction("ConfirmDH", new { @TokenKey = token });
        }
        public ActionResult result()
        {
            return View();
        }
        public ActionResult ConfirmDH(string TokenKey)
        {
            if (db.DonHangs.Where(n => n.TokenKey == TokenKey).Count() == 1)
            {               
                EmailSender email = new EmailSender();
                var DonHang = db.DonHangs.SingleOrDefault(n => n.TokenKey == TokenKey);                
                DonHang.TokenKey = "";
                DonHang.TrangThaiDH = EnumExtensions.GetDescription(Enumstatus.Confirmed);
                email.ThongBaoDonHangMoiAsync("Cửa Hàng","philong110597@gmail.com","Truong",DonHang.SoLuongBan.ToString(),DonHang.TongTien.ToString(),DonHang.NgayMua.ToString(),"Truongta9701@gmail.com","BinhTan");

                db.SaveChanges();
                return RedirectToAction("Result");
            }
            else
            {
                return View();
            }

        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            return PartialView();
        }

        public static string RandomNumber(int numberRD)
        {
            string randomStr = "";
            try
            {

                int[] myIntArray = new int[numberRD];
                int x;
                //that is to create the random # and add it to uour string
                Random autoRand = new Random();
                for (x = 0; x < numberRD; x++)
                {
                    myIntArray[x] = System.Convert.ToInt32(autoRand.Next(0, 9));
                    randomStr += (myIntArray[x].ToString());
                }
            }
            catch (Exception ex)
            {
                randomStr = "error";
            }
            return randomStr;
        }
        #endregion

        #region "Thanh Toán"
        [HttpGet]
        //public ActionResult DatHang()
        //{
        //    if(Session["Client_GioHang"] == null)
        //    {
        //        return RedirectToAction("Index", "Client_TrangChu");
        //    }
        //    List<Client_GioHang> lstGiohang = LayClient_GioHang();
        //    ViewBag.TongSoLuong = TongSoLuong();
        //    ViewBag.TongTien = TongTien();

        //    return View(lstGiohang);
        //}

        public ActionResult DatHang(FormCollection collection)
        {
            string s = collection["inputtext"];
            var MaKH = db.KhachHangs.SingleOrDefault(n => n.SoDT == s).KhachHangId;
            DonHang DH = new DonHang();
            List<Client_GioHang> gh = LayClient_GioHang();
            DH.KhachHangId = MaKH;
            DH.CuaHangId = 4;
            DH.NgayMua = DateTime.Now;
            DH.SoLuongBan = TongSoLuong();
            DH.TongTien = TongTien();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            db.DonHangs.Add(DH);
            db.SaveChanges();
            foreach (var item in gh)
            {
                CTDonHang CTDH = new CTDonHang();
                CTDH.DonhangId = DH.DonHangId;
                CTDH.SanPhamId = item.cSanPhamId;
                CTDH.SoLuong = item.cSoLuong;
                CTDH.DonGia = item.cDonGia;
                CTDH.GiamGia = item.cGiamGia;
                db.CTDonHangs.Add(CTDH);
            }
            db.SaveChanges();
            Session["Client_GioHang"] = null;
            return RedirectToAction("Index", "client_TrangChu");



        }


        #endregion












    }
}

