using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication13.Models
{
    public class PhieuCK
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public int pSanPhamId { get; set; }
        public string pMaSanPham { get; set; }
        public string pTenSP { get; set; }
        public int pSoLuongCur { get; set; }
        public int pSoLuongChuyen { get; set; }
        public int pFromKho { get; set; }

        public int pToKho { get; set; }




        public PhieuCK(int SanPhamId, int SoLuongChuyen)
        {
            pSanPhamId = SanPhamId;
            SanPham SP = db.SanPhams.SingleOrDefault(n => n.SanPhamId == pSanPhamId);
            pMaSanPham = SP.MaSanPham;
            pTenSP = SP.TenSP;
            pSoLuongCur = SP.SoLuong;
            ApplicationUser user = new ApplicationUser();
            pFromKho = user.CuaHangId;
            pSoLuongChuyen = SoLuongChuyen;
        }
    }
}
