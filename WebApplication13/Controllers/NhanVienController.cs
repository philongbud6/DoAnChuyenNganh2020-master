using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication13.Models;

namespace WebApplication13.Controllers
{
    public class NhanVienController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        // GET: NhanVien
        #region "ROLE"
        public ActionResult Roles()
        {
            var model = db.Roles.AsEnumerable();
            return View(model);
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IdentityRole role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Roles.Add(role);
                    db.SaveChanges();
                }
                return RedirectToAction("Roles");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(role);
        }
        public ActionResult EditR(string Id)
        {
            IdentityRole role = db.Roles.Find(Id);
            return View(role);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditR(IdentityRole role)
        {
            try
            {
                db.Entry(role).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Roles");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(role);
            }
        }
        public ActionResult Delete(string Id)
        {
            var model = db.Roles.Find(Id);

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string Id)
        {
            IdentityRole model = null;
            try
            {
                model = db.Roles.Find(Id);
                db.Roles.Remove(model);
                db.SaveChanges();
                return RedirectToAction("Roles");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);

        }
        #endregion


        #region "Nhân Viên"

        public ActionResult NhanVien()

        {
            IEnumerable<ApplicationUser> model = db.Users.Where(n => n.IsAccount == false).Include(b => b.CuaHang).AsEnumerable();
            return View(model);
        }
        public ActionResult TaiKhoan()
        {
            IEnumerable<ApplicationUser> model = db.Users.Where(n => n.IsAccount == true).Include(b => b.CuaHang).AsEnumerable();

            return View(model);
        }
        public ActionResult Edit(string Id)

        {

            ApplicationUser model = db.Users.Find(Id);
            ViewBag.CuaHangId = new SelectList(db.cuaHangs, "CuaHangId", "TenCuaHang");

            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationUser model)
        {
            try
            {
                ViewBag.CuaHangId = new SelectList(db.cuaHangs, "CuaHangId", "TenCuaHang");
                model.FullName = model.FirstName + " " + model.LastName;
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("nhanVien");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.CuaHangId = new SelectList(db.cuaHangs, "CuaHangId", "TenCuaHang");

                return View(model);
            }
        }
        public ActionResult EditRole(string Id)

        {

            ApplicationUser model = db.Users.Find(Id);

            ViewBag.RoleId = new SelectList(db.Roles.ToList().Where(item => model.Roles.FirstOrDefault(r => r.RoleId == item.Id) == null).ToList(), "Id", "Name");

            return View(model);

        }

        [HttpPost]

        [ValidateAntiForgeryToken]

        public ActionResult AddToRole(string UserId, string[] RoleId)

        {

            ApplicationUser model = db.Users.Find(UserId);

            if (RoleId != null && RoleId.Count() > 0)

            {

                foreach (string item in RoleId)

                {

                    IdentityRole role = db.Roles.Find(RoleId);

                    model.Roles.Add(new IdentityUserRole() { UserId = UserId, RoleId = item });

                }

                db.SaveChanges();

            }

            ViewBag.RoleId = new SelectList(db.Roles.ToList().Where(item => model.Roles.FirstOrDefault(r => r.RoleId == item.Id) == null).ToList(), "Id", "Name");

            return RedirectToAction("EditRole", new { Id = UserId });

        }

        [HttpPost]

        [ValidateAntiForgeryToken]

        public ActionResult DeleteRoleFromUser(string UserId, string RoleId)

        {

            ApplicationUser model = db.Users.Find(UserId);

            model.Roles.Remove(model.Roles.Single(m => m.RoleId == RoleId));

            db.SaveChanges();

            ViewBag.RoleId = new SelectList(db.Roles.ToList().Where(item => model.Roles.FirstOrDefault(r => r.RoleId == item.Id) == null).ToList(), "Id", "Name");

            return RedirectToAction("EditRole", new { Id = UserId });

        }

        public ActionResult DeleteNV(string Id)

        {

            var model = db.Users.Find(Id);

            return View(model);

        }

        [HttpPost]

        [ValidateAntiForgeryToken]

        [ActionName("Delete")]

        public ActionResult DeleteConfirmedNV(string Id)
        {
            ApplicationUser model = null;
            try
            {
                model = db.Users.Find(Id);
                db.Users.Remove(model);
                db.SaveChanges();
                return RedirectToAction("NhanVien");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("DeleteNV", model);
            }
        }



        #endregion


    }
}
