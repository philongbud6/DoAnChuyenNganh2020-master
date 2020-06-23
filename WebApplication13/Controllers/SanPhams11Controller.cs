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
    public class SanPhams11Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SanPhams11
        public ActionResult Index()
        {
            var sanPhams = db.SanPhams.Include(s => s.KhoHang).Include(s => s.LoaiSP);
            return View(sanPhams.ToList());
        }

        // GET: SanPhams11/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: SanPhams11/Create
        public ActionResult Create()
        {
            ViewBag.KhoHangId = new SelectList(db.KhoHangs, "KhoHangId", "TenKho");
            ViewBag.LoaiSPId = new SelectList(db.LoaiSPs, "LoaiSPId", "TenLoai");
            return View();
        }

        // POST: SanPhams11/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SanPhamId,MaSanPham,TenSP,SoLuong,tempSoLuong,MoTa,DonGia,NhaCungCapId,LoaiSPId,TopOffer,Ghim,Image2,Url_img2,Image1,Url_img1,NgayTao,Xoa,KhoHangId,Show")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KhoHangId = new SelectList(db.KhoHangs, "KhoHangId", "TenKho", sanPham.KhoHangId);
            ViewBag.LoaiSPId = new SelectList(db.LoaiSPs, "LoaiSPId", "TenLoai", sanPham.LoaiSPId);
            return View(sanPham);
        }

        // GET: SanPhams11/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.KhoHangId = new SelectList(db.KhoHangs, "KhoHangId", "TenKho", sanPham.KhoHangId);
            ViewBag.LoaiSPId = new SelectList(db.LoaiSPs, "LoaiSPId", "TenLoai", sanPham.LoaiSPId);
            return View(sanPham);
        }

        // POST: SanPhams11/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SanPhamId,MaSanPham,TenSP,SoLuong,tempSoLuong,MoTa,DonGia,NhaCungCapId,LoaiSPId,TopOffer,Ghim,Image2,Url_img2,Image1,Url_img1,NgayTao,Xoa,KhoHangId,Show")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KhoHangId = new SelectList(db.KhoHangs, "KhoHangId", "TenKho", sanPham.KhoHangId);
            ViewBag.LoaiSPId = new SelectList(db.LoaiSPs, "LoaiSPId", "TenLoai", sanPham.LoaiSPId);
            return View(sanPham);
        }

        // GET: SanPhams11/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams11/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
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
