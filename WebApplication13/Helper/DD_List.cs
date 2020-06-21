using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication13.Helper
{
    public class DD_List
    {
        public List<DD_LoaiSP> LoaiSP_Model { get; set; }
        public SelectList FilteredCity { get; set; }
    }
    public class DD_LoaiSP
    {
        public int DD_LoaiSPId { get; set; }
        public string DD_TenLoai { get; set; }
    }
    public class DD_NhaCungCap
    {
        public int DD_NhaCungCapId { get; set; }
        public int DD_LoaiSPId { get; set; }
        public string DD_TenNhaCungCap { get; set; }
    }
}
