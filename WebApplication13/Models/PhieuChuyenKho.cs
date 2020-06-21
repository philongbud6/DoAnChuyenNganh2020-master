using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication13.Models
{
    public class PhieuChuyenKho
    {
        public int PhieuChuyenKhoId { get; set; }
        public string TenPhieuChuyenKho { get; set; }
        public int SoLuongChuyen { get; set; }

        public TrangThai TrangThai { get; set; }
        [Required]
        public int TrangThaiId { get; set; }
        public int FromKho { get; set; }
        public int ToKho { get; set; }
        public DateTime NgayTao { get; set; }
        public Boolean ConfirmFrom { get; set; }
        public DateTime? NgayConfirmFrom { get; set; }
        public Boolean ConfirmTo { get; set; }
        public DateTime? NgayConfirmTo { get; set; }


        public KhoHang KhoHang { get; set; }

    }
}
