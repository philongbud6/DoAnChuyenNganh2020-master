using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication13.Models
{
    public class NhaCungCap
    {
        public int NhaCungCapId { get; set; }
        [Required]
        public string TenNhaCungCap { get; set; }
        public string DiaChi { get; set; }
        public LoaiSP LoaiSP { get; set; }
        [Required]
        public int LoaiSPId { get; set; }

        public byte[] Image { get; set; }
        public string Url_Image { get; set; }
        public DateTime NgayTao { get; set; }
        public string ChuThich { get; set; }



        public Boolean Xoa { get; set; }
    }
}
