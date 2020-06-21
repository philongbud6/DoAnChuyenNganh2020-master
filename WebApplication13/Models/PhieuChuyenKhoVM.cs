using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication13.Models
{
    public class PhieuChuyenKhoVM
    {
        public int vPhieuChuyenKhoId { get; set; }
        public string vTenPhieuChuyenKho { get; set; }
        public int vSoLuongChuyen { get; set; }
        public string vTrangThai { get; set; }
        public string vFromKho { get; set; }
        public string vToKho { get; set; }
        public DateTime vNgayTao { get; set; }
        public string vConfirmFrom { get; set; }
        public string vConfirmTo { get; set; }
        public DateTime? vNgayConfirmFrom { get; set; }
        public DateTime? vNgayConfirmTo { get; set; }





    }
}
