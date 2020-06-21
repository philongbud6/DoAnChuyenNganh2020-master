using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication13.Helper;
using WebApplication13.Models;

namespace WebApplication13.Controllers
{
    public class SanPhamsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        static string ImgPath = "/Content/Client/Img/";

        #region "SanPham"
        public ActionResult Index()
        {
            var sanPhams = (from s in db.SanPhams where s.Xoa == false select s).Include(a => a.LoaiSP);

            return View(sanPhams.ToList());
        }

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

        #region "Create"
        public ActionResult Create()
        {
            DD_List objTestList = new DD_List();
            objTestList.LoaiSP_Model = new List<DD_LoaiSP>();
            objTestList.LoaiSP_Model = GetAllLoaiSP();
            return View(objTestList);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SanPhamId,MaSanPham,TenSP,SoLuong,MoTa,DonGia,NhaCungCapId,LoaiSPId,NgayTao,KhoHangId")] SanPham sanPham, FormCollection f, HttpPostedFileBase Image1, HttpPostedFileBase Image2)
        {
            var GetTenSP = db.SanPhams.FirstOrDefault(a => a.TenSP == sanPham.TenSP);

            if (Image1 != null && Image1.ContentLength > 0)
            {
                sanPham.Image1 = new byte[Image1.ContentLength];
                Image1.InputStream.Read(sanPham.Image1, 0, Image1.ContentLength);
                string filename1 = System.IO.Path.GetFileName(Image1.FileName);
                string urlImage1 = Server.MapPath(ImgPath + filename1);
                if (!System.IO.File.Exists(urlImage1))
                {
                    Image1.SaveAs(urlImage1);
                    sanPham.Url_img1 = ImgPath + filename1;
                }

                else
                {
                    DD_List objTestList = new DD_List();
                    objTestList.LoaiSP_Model = new List<DD_LoaiSP>();
                    objTestList.LoaiSP_Model = GetAllLoaiSP();
                    ViewBag.FileExists = "**File exists " + filename1;
                    return View(objTestList);
                }
            }
            if (Image2 != null && Image2.ContentLength > 0)
            {
                sanPham.Image2 = new byte[Image2.ContentLength];
                Image2.InputStream.Read(sanPham.Image2, 0, Image2.ContentLength);
                string filename2 = System.IO.Path.GetFileName(Image2.FileName);
                string urlImage2 = Server.MapPath(ImgPath + filename2);
                if (!System.IO.File.Exists(urlImage2))
                {
                    Image1.SaveAs(urlImage2);
                    sanPham.Url_img2 = ImgPath + filename2;
                }

                else
                {
                    DD_List objTestList = new DD_List();
                    objTestList.LoaiSP_Model = new List<DD_LoaiSP>();
                    objTestList.LoaiSP_Model = GetAllLoaiSP();
                    ViewBag.FileExists = "**File exists " + filename2;
                    return View(objTestList);
                }
            }

            if (GetTenSP != null)
            {
                DD_List objTestList = new DD_List();
                objTestList.LoaiSP_Model = new List<DD_LoaiSP>();
                objTestList.LoaiSP_Model = GetAllLoaiSP();

                ViewBag.TenSPExists = "** TenSP tồn Tại " + GetTenSP.TenSP;
                return View(objTestList);
            }


            sanPham.LoaiSPId = int.Parse(f["LoaiSP_Model"]);
            sanPham.NhaCungCapId = int.Parse(f["ddlcity"]);

            if (ModelState.IsValid)
            {
                sanPham.KhoHangId = 0;
                sanPham.NgayTao = DateTime.Now;
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LoaiSPId = new SelectList(db.LoaiSPs, "LoaiSPId", "TenLoai", sanPham.LoaiSPId);

            ViewBag.NhaCungCapId = new SelectList(db.NhaCungCaps, "NhaCungCapId", "TenNhaCungCap", sanPham.NhaCungCapId);
            return View(sanPham);
        }
        #endregion

        #region "Edit"
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



            //ViewBag.LoaiSPId = new SelectList(db.LoaiSPs, "LoaiSPId", "TenLoai", sanPham.LoaiSPId);
            //ViewBag.NhaCungCapId = new SelectList(db.NhaCungCaps, "NhaCungCapId", "TenNhaCungCap", sanPham.NhaCungCapId);
            return View(sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SanPhamId,MaSanPham,TenSP,SoLuong,MoTa,DonGia,NhaCungCapId,LoaiSPId,NgayTao,KhoHangId")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LoaiSPId = new SelectList(db.LoaiSPs, "LoaiSPId", "TenLoai", sanPham.LoaiSPId);
            ViewBag.NhaCungCapId = new SelectList(db.NhaCungCaps, "NhaCungCapId", "TenNhaCungCap", sanPham.NhaCungCapId);
            return View(sanPham);
        }
        #endregion

        #region "Delete"
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            sanPham.Xoa = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion


        #region "Get Dropdown list"
        [HttpPost]
        public ActionResult GetNCCByLoaiSPId(int DD_LoaiSPId)
        {
            List<DD_NhaCungCap> objcity = new List<DD_NhaCungCap>();
            objcity = GetAllNhaCungcap().Where(m => m.DD_LoaiSPId == DD_LoaiSPId).ToList();
            SelectList obgNCC = new SelectList(objcity, "DD_NhaCungCapId", "DD_TenNhaCungCap", 0);
            return Json(obgNCC);
        }


        public List<DD_LoaiSP> GetAllLoaiSP()
        {
            var LoaiSPList = db.LoaiSPs.ToList();
            List<DD_LoaiSP> objDD_LoaiSP = new List<DD_LoaiSP>();
            objDD_LoaiSP.Add(new DD_LoaiSP { DD_LoaiSPId = 0, DD_TenLoai = "Chọn Loại Sản Phẩm" });
            foreach (var item in LoaiSPList)
            {
                objDD_LoaiSP.Add(new DD_LoaiSP { DD_LoaiSPId = item.LoaiSPId, DD_TenLoai = item.TenLoai });
            }
            return objDD_LoaiSP;
        }

        public List<DD_NhaCungCap> GetAllNhaCungcap()
        {

            var GetNCC = db.NhaCungCaps.ToList();
            List<DD_NhaCungCap> objNCC = new List<DD_NhaCungCap>();
            foreach (var item in GetNCC)
            {
                objNCC.Add(new DD_NhaCungCap { DD_NhaCungCapId = item.NhaCungCapId, DD_LoaiSPId = item.LoaiSPId, DD_TenNhaCungCap = item.TenNhaCungCap });
            }
            return objNCC;
        }


        #endregion


        public ActionResult KiemTraTonKho()
        {
            var MaCH = int.Parse(User.TenCuaHang());
            var sanpham = db.SanPhams.Where(n => n.KhoHangId == MaCH).Include(s => s.LoaiSP);
            return View(sanpham.ToList());
        }


    }
}

