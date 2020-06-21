using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication13.Models;

namespace WebApplication13.Controllers
{
    public class NhaCungCapsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public static int editID;

        static string ImgPath = "/Content/Client/Img/";
        // GET: NhaCungCaps
        public ActionResult Index()
        {
            var NCC = from s in db.NhaCungCaps where s.Xoa == false select s;
            return View(NCC.ToList());
        }

        // GET: NhaCungCaps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaCungCap nhaCungCap = db.NhaCungCaps.Find(id);
            if (nhaCungCap == null)
            {
                return HttpNotFound();
            }
            return View(nhaCungCap);
        }

        // GET: NhaCungCaps/Create
        public ActionResult Create()
        {
            ViewBag.LoaiSPId = new SelectList(db.LoaiSPs, "LoaiSPId", "TenLoai");

            return View();
        }

        // POST: NhaCungCaps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NhaCungCapId,TenNhaCungCap,DiaChi, NgayTao,LoaiSPId")] NhaCungCap nhaCungCap, HttpPostedFileBase Image)
        {
            if (Image != null && Image.ContentLength > 0)
            {
                nhaCungCap.Image = new byte[Image.ContentLength];
                Image.InputStream.Read(nhaCungCap.Image, 0, Image.ContentLength);
                string filename = System.IO.Path.GetFileName(Image.FileName);
                string urlImage = Server.MapPath(ImgPath + filename);
                Image.SaveAs(urlImage);
                nhaCungCap.Url_Image = ImgPath + filename;
            }

            if (ModelState.IsValid)
            {
                nhaCungCap.NgayTao = DateTime.Now;
                db.NhaCungCaps.Add(nhaCungCap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LoaiSPId = new SelectList(db.LoaiSPs, "LoaiSPId", "TenLoai", nhaCungCap.LoaiSPId);

            return View(nhaCungCap);
        }

        // GET: NhaCungCaps/Edit/5
        public ActionResult Edit(int? id, string strURL)
        {
            editID = id.Value;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaCungCap nhaCungCap = db.NhaCungCaps.Find(id);
            if (nhaCungCap == null)
            {
                return HttpNotFound();
            }
            return View(nhaCungCap);
        }

        // POST: NhaCungCaps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NhaCungCapId,TenNhaCungCap,DiaChi,Ngaytao")] NhaCungCap nhaCungCap, HttpPostedFileBase editImage)
        {
            NhaCungCap modifynhaCungCap = db.NhaCungCaps.Find(nhaCungCap.NhaCungCapId);
            if (modifynhaCungCap != null)
            {
                if (editImage != null && editImage.ContentLength > 0)
                {
                    modifynhaCungCap.Image = new byte[editImage.ContentLength];
                    editImage.InputStream.Read(modifynhaCungCap.Image, 0, editImage.ContentLength);
                    string filename = System.IO.Path.GetFileName(editImage.FileName);
                    string urlImage = Server.MapPath(ImgPath + filename);
                    editImage.SaveAs(urlImage);

                    modifynhaCungCap.Url_Image = ImgPath + filename;
                }
            }


            if (ModelState.IsValid)
            {
                db.Entry(modifynhaCungCap).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(Url.Action("Edit", "Nhacungcaps", new { id = editID }));
            }
            return View(nhaCungCap);
        }

        // GET: NhaCungCaps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaCungCap nhaCungCap = db.NhaCungCaps.Find(id);
            if (nhaCungCap == null)
            {
                return HttpNotFound();
            }
            return View(nhaCungCap);
        }

        // POST: NhaCungCaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NhaCungCap nhaCungCap = db.NhaCungCaps.Find(id);
            nhaCungCap.Xoa = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

