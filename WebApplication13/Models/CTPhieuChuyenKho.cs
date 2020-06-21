using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication13.Models
{
    public class CTPhieuChuyenKho
    {

        public int CTPhieuChuyenKhoId { get; set; }
        public int SoLuongChuyen { get; set; }
        public PhieuChuyenKho PhieuChuyenKho { get; set; }
        public int PhieuChuyenKhoId { get; set; }
        public SanPham SanPham { get; set; }
        public int SanPhamId { get; set; }

    }
}
