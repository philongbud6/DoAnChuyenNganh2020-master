using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication13.Models;

namespace WebApplication13.Areas.Client.Controllers
{
    public class Client_SanPhamController : Controller
    {
        // GET: Client/Client_SanPham
        public ActionResult Index(string TenLoai)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var Get = db.SanPhams.Include(s => s.LoaiSP).
                          Where(a => a.LoaiSP.TenLoai == TenLoai && a.KhoHangId == 0).OrderBy(n => n.Ghim);
                var ListNCC = db.NhaCungCaps.Include(s => s.LoaiSP).Where(a => a.LoaiSP.TenLoai == TenLoai);

                ViewBag.TenLoai = TenLoai;
                return View(Get.ToList());
            }
        }

        public ActionResult CTSanPham(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var Get = db.SanPhams.
                          Where(a => a.SanPhamId == id);
                return View(Get.ToList());
            }
        }
        public ActionResult SPByNhaCungCap(int NhaCungCapId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var Get = db.SanPhams.
                          Where(a => a.NhaCungCapId == NhaCungCapId);

                var check = db.NhaCungCaps.SingleOrDefault(n => n.NhaCungCapId == NhaCungCapId).LoaiSPId;
                ViewBag.TenLoai = db.LoaiSPs.SingleOrDefault(n => n.LoaiSPId == check).TenLoai;

                return View(Get.ToList());
            }
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}
