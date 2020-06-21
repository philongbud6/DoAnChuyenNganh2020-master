using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication13.Models;
using System.Data.Entity;

namespace WebApplication13.Areas.Client.Controllers
{
    public class Client_PartialController : Controller
    {
        public ActionResult Nav_Category()
        {
            return View();
        }

        public ActionResult Nav_NhaCungCap(string TenLoai)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var List = db.NhaCungCaps.Include(s => s.LoaiSP).Where(a => a.LoaiSP.TenLoai == TenLoai);
                return PartialView(List.ToList());
            }
        }
        public ActionResult Partial_Footer_CuaHangAddress()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var GetAddress = db.cuaHangs.Where(n => n.Show == true).ToList();
                return PartialView(GetAddress);
            }
        }
        public ActionResult TopOffer_Partial()
        {

            ApplicationDbContext db = new ApplicationDbContext();
            {
                var Get = db.SanPhams.Where(n => n.TopOffer == true).OrderBy(n => n.NgayTao);
                return PartialView(Get);
            }
        }
        public ActionResult Check_Con_Hang_Partial(string MaSP)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            {
                var Get = db.SanPhams.Where(n => n.MaSanPham == MaSP).Include(n => n.KhoHang).ToList();

                ViewBag.SoCuaHang = Get.Count() + 1;
                return PartialView(Get);
            }
        }
    }
}
