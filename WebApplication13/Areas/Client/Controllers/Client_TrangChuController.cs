using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication13.Models;
using WebApplication13.Areas.Client.Models;
using System.Data.Entity;

namespace WebApplication13.Areas.Client.Controllers
{
    public class Client_TrangChuController : Controller
    {
        // GET: Client/Client_TrangChu
        public ActionResult Index()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var Get = from s in db.SanPhams.Include(a => a.LoaiSP).OrderBy(n => n.Ghim)
                          group s by s.LoaiSP.TenLoai into g
                          select new GroupByLoaiSP<string, IEnumerable<SanPham>> { Key = g.Key, Values = g.Take(7).ToList() };
                return View(Get.ToList());
            }
        }
    }
}
