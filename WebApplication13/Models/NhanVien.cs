using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication13.Models
{
    public class NhanVien
    {
        public int NhanVienId { get; set; }
        [Required]
        public string TenNhanVien { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        [Required]
        public CuaHang CuaHang { get; set; }
        [Required]
        public int CuaHangId { get; set; }


    }
}
