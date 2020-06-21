using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication13.Models
{
    public class KhoHang
    {
        [Required]
        public int KhoHangId { get; set; }
        [Required]
        public string TenKho { get; set; }

        public int CuaHangId { get; set; }
    }
}
