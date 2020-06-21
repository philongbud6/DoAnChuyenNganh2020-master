using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication13.Models
{
    public class SanPham
    {
        public int SanPhamId { get; set; }
        [Required]
        public string MaSanPham { get; set; }
        public string TenSP { get; set; }
        [Required]
        public int SoLuong { get; set; }
        public int tempSoLuong { get; set; }

        public string MoTa { get; set; }
        public int DonGia { get; set; }
        public int NhaCungCapId { get; set; }
        public LoaiSP LoaiSP { get; set; }
        [Required]
        public int LoaiSPId { get; set; }
        public Boolean TopOffer { get; set; }
        public int Ghim { get; set; }
        public byte[] Image2 { get; set; }
        public string Url_img2 { get; set; }
        public byte[] Image1 { get; set; }
        public string Url_img1 { get; set; }
        public DateTime NgayTao { get; set; }
        public Boolean Xoa { get; set; }
        public KhoHang KhoHang { get; set; }
        public int KhoHangId { get; set; }
        public Boolean Show { get; set; }

    }
}
