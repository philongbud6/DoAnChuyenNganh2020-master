using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication13.Models;

namespace WebApplication13.Areas.Client.Models
{
    public class Client_GioHang
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public int cSanPhamId { get; set; }
        public string cTenSP { get; set; }
        public int cSoLuong { get; set; }
        public int cDonGia { get; set; }
        public int cGiamGia { get; set; }
        public int MaNV { get; set; }
        public string cMaSanPham { get; set; }
        public string cURL_Image1 { get; set; }
        public int cThanhTien { get; set; }
      
        public Client_GioHang(int SanPhamId)
        {
            MaNV = 0;
            cSanPhamId = SanPhamId;
            SanPham SP = db.SanPhams.Single(n => n.SanPhamId == cSanPhamId);
            cTenSP = SP.TenSP;
            cSoLuong = 1;
            cDonGia = SP.DonGia;
            cURL_Image1 = SP.Url_img1;
            cMaSanPham = SP.MaSanPham;
            cThanhTien = cSoLuong * (cDonGia - cGiamGia);
         
        }
    }
}
