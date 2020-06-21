using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication13.Models;
using WebApplication13.Models.ViewModel;

namespace WebApplication13.Controllers.Thống_Kê
{
    public class ThongKeAdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ThongKeAdmin



        public ActionResult TKCuaHang(int id)
        {

            var Get = from s in db.DonHangs
                      where s.CuaHangId == id
                      group s by DbFunctions.TruncateTime(s.NgayMua) into g
                      select new Test<DateTime, int, int, float> { Key = g.Key.Value, TongDH = g.Count(), TongSL = g.Sum(s => s.SoLuongBan), TongTien = g.Sum(s => s.TongTien) };

            return View(Get.ToList());
        }

        public ActionResult TongDoanhSo()
        {
            var Tk = db.DonHangs.Include(a => a.CuaHang).Include(a => a.KhachHang);


            return View(Tk.ToList());
        }
        public ActionResult TongDS()
        {
            var TK = from s in db.DonHangs
                     group s by s.NgayMua.Month into g
                     select new ThongKeVM<int, float> { Key = g.Key, DoanhSo = g.Sum(s => s.TongTien) }.DoanhSo;
            var t = string.Join(",", TK);

            ViewData["DS"] = t;
            return View();
        }

    }

}

