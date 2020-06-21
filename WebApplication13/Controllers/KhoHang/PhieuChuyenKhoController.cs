using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication13.Email;
using WebApplication13.Helper;
using WebApplication13.Models;

namespace WebApplication13.Controllers.Kho_Hang
{
    [Authorize]
    public class PhieuChuyenKhoController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public List<PhieuCK> LayPhieu()
        {
            List<PhieuCK> lstPhieuCK = Session["PhieuCK"] as List<PhieuCK>;
            if (lstPhieuCK == null)
            {
                lstPhieuCK = new List<PhieuCK>();
                Session["PhieuCK"] = lstPhieuCK;
            }
            return lstPhieuCK;
        }

        [HttpPost]
        public ActionResult ThemPhieuChuyenKho(int pSanPhamId, int pSoLuongChuyen)
        {
            List<PhieuCK> lstPhieuCK = LayPhieu();
            PhieuCK PCK = lstPhieuCK.Find(n => n.pSanPhamId == pSanPhamId);
            ViewBag.TongSoLuong = TongSoLuong();

            if (PCK == null)
            {
                var check = db.SanPhams.SingleOrDefault(n => n.SanPhamId == pSanPhamId);
                if (check.SoLuong >= pSoLuongChuyen)
                {
                    PCK = new PhieuCK(pSanPhamId, pSoLuongChuyen);
                    lstPhieuCK.Add(PCK);
                    string message = "Thêm Thành Công";
                    return Json(new { Message = message, JsonRequestBehavior.AllowGet });
                }
                else
                {
                    string message = "Không Đủ Số Lượng";
                    return Json(new { Message = message, JsonRequestBehavior.AllowGet });

                }
            }
            else
            {
                string message = "ERROR";
                return Json(new { Message = message, JsonRequestBehavior.AllowGet });
            }
        }
        private int TongSoLuong()
        {
            int pTongSoLuong = 0;
            List<PhieuCK> lstPhieuCK = Session["PhieuCK"] as List<PhieuCK>;
            if (lstPhieuCK != null)
            {
                pTongSoLuong = lstPhieuCK.Sum(n => n.pSoLuongChuyen);
            }
            return pTongSoLuong;
        }
        public ActionResult PhieuCK()
        {
            List<PhieuCK> lstPhieuCK = LayPhieu();
            ViewBag.TongSoLuong = TongSoLuong();

            return Json(lstPhieuCK, JsonRequestBehavior.AllowGet);
        }

        public ActionResult XoaPhieuCK(int pSanPhamId)
        {
            List<PhieuCK> lstPhieuCK = LayPhieu();
            PhieuCK PCK = lstPhieuCK.SingleOrDefault(n => n.pSanPhamId == pSanPhamId);
            if (PCK != null)
            {
                lstPhieuCK.RemoveAll(n => n.pSanPhamId == pSanPhamId);
                return RedirectToAction("PhieuCK");
            }
            if (lstPhieuCK.Count == 0)
            {
                return RedirectToAction("TrangChu", "Admin");
            }
            return RedirectToAction("PhieuCK");

        }
        public ActionResult CapNhatPhieuCK(int pSanPhamId, FormCollection form)
        {
            List<PhieuCK> lstPhieuCK = LayPhieu();
            PhieuCK PCK = lstPhieuCK.SingleOrDefault(n => n.pSanPhamId == pSanPhamId);
            if (PCK != null)
            {
                PCK.pSoLuongChuyen = int.Parse(form["SoLuongChuyen"].ToString());
            }
            return RedirectToAction("PhieuCK");
        }

        [HttpPost]
        public ActionResult LapPhieu(FormCollection collection)
        {
            PhieuChuyenKho PCK = new PhieuChuyenKho();
            List<PhieuCK> listPCK = LayPhieu();
            PCK.SoLuongChuyen = TongSoLuong();
            PCK.FromKho = int.Parse(User.TenCuaHang());
            PCK.NgayTao = DateTime.Now;
            PCK.ToKho = int.Parse(collection["KhoHangId"]);
            PCK.TrangThaiId = 0;
            db.PhieuChuyenKhos.Add(PCK);
            foreach (var item in listPCK)
            {
                CTPhieuChuyenKho CTPCK = new CTPhieuChuyenKho();
                CTPCK.PhieuChuyenKhoId = PCK.PhieuChuyenKhoId;
                CTPCK.SanPhamId = item.pSanPhamId;
                CTPCK.SoLuongChuyen = item.pSoLuongChuyen;
                db.CTPhieuChuyenKhos.Add(CTPCK);

                var get = db.SanPhams.SingleOrDefault(n => n.SanPhamId == item.pSanPhamId);
                get.tempSoLuong = (get.SoLuong - item.pSoLuongChuyen);
                db.Entry(get).State = EntityState.Modified;
            }
            db.SaveChanges();
            InformMailAsync();

            Session["PhieuCK"] = null;
            return RedirectToAction("PhieuChuyenHang");
        }
        [Authorize]
        public ActionResult Create()
        {
            var Get = db.SanPhams.Where(n => n.KhoHangId == 0).ToList();
            List<PhieuCK> lstPhieuCK = LayPhieu();
            ViewBag.KhoHangId = new SelectList(db.KhoHangs, "CuaHangId", "TenKho");
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.Check = lstPhieuCK == null ? "True" : "False";

            return View(Get);
        }


        [Authorize]
        public ActionResult PhieuChuyenHang()
        {
            ViewBag.TrangThaiId = new SelectList(db.TrangThais, "TrangThaiId", "TenTrangThai");
            var MaCH = int.Parse(User.TenCuaHang());
            var get = db.PhieuChuyenKhos.Where(n => n.FromKho == MaCH && n.TrangThaiId != 4).ToList();
            List<PhieuChuyenKhoVM> lstPhieuCK = Session["PCHVM"] as List<PhieuChuyenKhoVM>;
            lstPhieuCK = new List<PhieuChuyenKhoVM>();
            foreach (var item in get)
            {
                PhieuChuyenKhoVM PCK = new PhieuChuyenKhoVM();
                PCK.vPhieuChuyenKhoId = item.PhieuChuyenKhoId;
                PCK.vSoLuongChuyen = item.SoLuongChuyen;
                PCK.vTrangThai = EnumExtensions.GetDescription(item.TrangThaiId == 0 ? Enumstatus.Open : Enumstatus.Close);

                PCK.vFromKho = db.cuaHangs.SingleOrDefault(n => n.CuaHangId == item.FromKho).displayName;
                PCK.vToKho = db.cuaHangs.SingleOrDefault(n => n.CuaHangId == item.ToKho).displayName;
                PCK.vConfirmFrom = EnumExtensions.GetDescription(item.ConfirmFrom == false ? Enumstatus.Confirm_False : Enumstatus.Confirm_True);
                PCK.vConfirmTo = EnumExtensions.GetDescription(item.ConfirmTo == false ? Enumstatus.Confirm_False : Enumstatus.Confirm_True);
                PCK.vNgayTao = item.NgayTao;
                lstPhieuCK.Add(PCK);
            }
            return View(lstPhieuCK);
        }
        public ActionResult PhieuNhanHang()
        {
            ViewBag.TrangThaiId = new SelectList(db.TrangThais, "TrangThaiId", "TenTrangThai");
            var MaCH = int.Parse(User.TenCuaHang());
            var get = db.PhieuChuyenKhos.Where(n => n.ToKho == MaCH && n.TrangThaiId != 4).ToList();
            List<PhieuChuyenKhoVM> lstPhieuCK = Session["PNHVM"] as List<PhieuChuyenKhoVM>;
            lstPhieuCK = new List<PhieuChuyenKhoVM>();
            foreach (var item in get)
            {
                PhieuChuyenKhoVM PCK = new PhieuChuyenKhoVM();
                PCK.vPhieuChuyenKhoId = item.PhieuChuyenKhoId;
                PCK.vSoLuongChuyen = item.SoLuongChuyen;
                PCK.vTrangThai = EnumExtensions.GetDescription(item.TrangThaiId == 0 ? Enumstatus.Open : Enumstatus.Close);

                PCK.vFromKho = db.cuaHangs.SingleOrDefault(n => n.CuaHangId == item.FromKho).displayName;
                PCK.vToKho = db.cuaHangs.SingleOrDefault(n => n.CuaHangId == item.ToKho).displayName;
                PCK.vConfirmFrom = EnumExtensions.GetDescription(item.ConfirmFrom == false ? Enumstatus.Confirm_False : Enumstatus.Confirm_True);
                PCK.vConfirmTo = EnumExtensions.GetDescription(item.ConfirmTo == false ? Enumstatus.Confirm_False : Enumstatus.Confirm_True);
                PCK.vNgayTao = item.NgayTao;
                lstPhieuCK.Add(PCK);
            }
            return View(lstPhieuCK);
        }
        public ActionResult CTPhieuChuyenKho(int phieuchuyenkhoId)
        {
            ViewBag.MaCH = int.Parse(User.TenCuaHang());
            ViewBag.Id = phieuchuyenkhoId;
            ViewBag.Check = " ";
            var Get = db.CTPhieuChuyenKhos.Where(n => n.PhieuChuyenKhoId == phieuchuyenkhoId).Include(n => n.SanPham).ToList();
            var check = db.PhieuChuyenKhos.SingleOrDefault(n => n.PhieuChuyenKhoId == phieuchuyenkhoId);
            if (check.FromKho == ViewBag.MaCH && check.ConfirmFrom == true)
            {
                ViewBag.Check = "Disabled";
            }
            if (check.ToKho == ViewBag.MaCH && check.ConfirmTo == true)
            {
                ViewBag.Check = "Disabled";
            }
            return View(Get);
        }

        public ActionResult Confirm(int phieuchuyenkhoId, int KhoHangId)
        {
            var Check = db.PhieuChuyenKhos.SingleOrDefault(n => n.PhieuChuyenKhoId == phieuchuyenkhoId);
            if (Check != null)
            {
                if (Check.FromKho == KhoHangId)
                {
                    Check.ConfirmFrom = true;
                    Check.NgayConfirmFrom = DateTime.Now;
                }
                if (Check.ToKho == KhoHangId)
                {
                    Check.ConfirmTo = true;
                    Check.NgayConfirmTo = DateTime.Now;
                }
                db.Entry(Check).State = EntityState.Modified;
                db.SaveChanges();
                if (ClosePhieuChuyenKho(phieuchuyenkhoId) == true)
                {
                    string message = "Đóng Phiếu Chuyển Kho";
                    return Json(new { Message = message, JsonRequestBehavior.AllowGet });
                }
                else
                {
                    string message = " Xác Nhận Thành Công";
                    return Json(new { Message = message, JsonRequestBehavior.AllowGet });
                }
            }
            else
            {
                string error = "Thất bại";
                return Json(new { Message = error, JsonRequestBehavior.AllowGet });
            }
        }
        public Boolean ClosePhieuChuyenKho(int phieuChuyenKhoId)
        {
            var Check = db.PhieuChuyenKhos.SingleOrDefault(n => n.PhieuChuyenKhoId == phieuChuyenKhoId);
            if (Check.ConfirmFrom == true && Check.ConfirmTo == true)
            {
                var get = db.CTPhieuChuyenKhos.Where(n => n.PhieuChuyenKhoId == phieuChuyenKhoId).ToList();
                foreach (var item in get)
                {
                    SanPham SP = new SanPham();
                    SP.SanPhamId = item.SanPhamId;
                    var s = db.SanPhams.Where(n => n.SanPhamId == item.SanPhamId).ToList();
                    foreach (var ss in s)
                    {
                        SP.TenSP = ss.TenSP;
                        SP.SoLuong = item.SoLuongChuyen;
                        SP.DonGia = ss.DonGia;
                        SP.MoTa = ss.MoTa;
                        SP.MaSanPham = ss.MaSanPham;
                        SP.NhaCungCapId = ss.NhaCungCapId;
                        SP.Show = false;
                        SP.NhaCungCapId = ss.NhaCungCapId;
                        SP.LoaiSPId = ss.LoaiSPId;
                        SP.TopOffer = ss.TopOffer;
                        SP.Url_img1 = ss.Url_img1;
                        SP.Url_img2 = ss.Url_img2;
                        SP.Xoa = ss.Xoa;
                        SP.KhoHangId = db.PhieuChuyenKhos.SingleOrDefault(n => n.PhieuChuyenKhoId == phieuChuyenKhoId).ToKho;
                        SP.NgayTao = db.PhieuChuyenKhos.SingleOrDefault(n => n.PhieuChuyenKhoId == phieuChuyenKhoId).NgayTao;
                        ss.SoLuong = ss.tempSoLuong;
                        db.Entry(ss).State = EntityState.Modified;
                    }

                    var PCK = db.PhieuChuyenKhos.SingleOrDefault(n => n.PhieuChuyenKhoId == phieuChuyenKhoId);
                    PCK.TrangThaiId = 4;
                    db.Entry(PCK).State = EntityState.Modified;
                    db.SanPhams.Add(SP);
                }
                db.SaveChanges();
                return true;

            }
            return false;
        }

        public ActionResult PhieuChuyenKhoByCuaHang(int TrangThaiId)
        {
            var MaCH = int.Parse(User.TenCuaHang());
            var get = db.PhieuChuyenKhos.Where(n => n.FromKho == MaCH && n.TrangThaiId == TrangThaiId || n.ToKho == MaCH && n.TrangThaiId == TrangThaiId).ToList();

            List<PhieuChuyenKhoVM> lstPhieuCK = Session["PCKVM"] as List<PhieuChuyenKhoVM>;
            lstPhieuCK = new List<PhieuChuyenKhoVM>();
            foreach (var item in get)
            {
                PhieuChuyenKhoVM PCK = new PhieuChuyenKhoVM();
                PCK.vPhieuChuyenKhoId = item.PhieuChuyenKhoId;
                PCK.vSoLuongChuyen = item.SoLuongChuyen;
                PCK.vTrangThai = EnumExtensions.GetDescription(item.TrangThaiId == 0 ? Enumstatus.Open : Enumstatus.Close);

                PCK.vFromKho = db.cuaHangs.SingleOrDefault(n => n.CuaHangId == item.FromKho).displayName;
                PCK.vToKho = db.cuaHangs.SingleOrDefault(n => n.CuaHangId == item.ToKho).displayName;
                PCK.vConfirmFrom = EnumExtensions.GetDescription(item.ConfirmFrom == false ? Enumstatus.Confirm_False : Enumstatus.Confirm_True);
                PCK.vConfirmTo = EnumExtensions.GetDescription(item.ConfirmTo == false ? Enumstatus.Confirm_False : Enumstatus.Confirm_True);
                PCK.vNgayTao = item.NgayTao;
                lstPhieuCK.Add(PCK);
            }
            return Json(lstPhieuCK, JsonRequestBehavior.AllowGet);
        }




        #region Excel
        public void ExportToExcel()
        {

            List<PhieuCK> lstPhieuCK = LayPhieu();
            var gv = new GridView();
            gv.DataSource = lstPhieuCK;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
        }


        public void Excel()
        {
            string path = String.Format("{0}", ConfigurationManager.AppSettings["TemplatesLocation"]);
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Collections");
            List<PhieuCK> lstPhieuCK = LayPhieu();

            ws.Cell(1, 1).Value = "ID Sản Phẩm";
            ws.Cell(1, 2).Value = "Mã Sản Phẩm";
            ws.Cell(1, 3).Value = "Tên Sản Phẩm";
            ws.Cell(1, 4).Value = "Số Lượng Hiện Tại";
            ws.Cell(1, 5).Value = "Số Lượng Chuyển";
            ws.Cell(1, 6).Value = "Chuyển Từ";
            ws.Cell(1, 7).Value = "Chuyển đến";

            ws.Cell(2, 1).Value = lstPhieuCK;

            wb.SaveAs(path + "\\testing.xlsx");

        }

        #endregion

        #region SendMail
        public async System.Threading.Tasks.Task<bool> InformMailAsync()
        {
            Excel();
            EmailSender email = new EmailSender();
            if (await email.PCKAsync("aaa", "truong.taquang@vn.bosch.com").ConfigureAwait(false) == true)
                return true;
            else
                return false;
        }


        #endregion














        public JsonResult AutoComplete(string prefix)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var MaSanPham = (from s in db.SanPhams
                             where s.MaSanPham.StartsWith(prefix)
                             select new
                             {
                                 label = s.MaSanPham,
                                 val = s.TenSP,
                                 MSP = s.SanPhamId,
                                 SLHT = s.SoLuong,
                             }).ToList();

            return Json(MaSanPham);
        }
    }
}
