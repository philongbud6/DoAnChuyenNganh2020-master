using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Threading.Tasks;
using WebApplication13.Helper;

namespace WebApplication13.Models
{
    
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public static string CS { get; set; }
        
        public DbSet<CTDonHang> CTDonHangs { get; set; }
        public DbSet<CuaHang> cuaHangs { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<LoaiSP> LoaiSPs { get; set; }
        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<KhoHang> KhoHangs { get; set; }
        public DbSet<PhieuChuyenKho> PhieuChuyenKhos { get; set; }
        public DbSet<CTPhieuChuyenKho> CTPhieuChuyenKhos { get; set; }
        public DbSet<TrangThai> TrangThais { get; set; }

        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {

        }

        //public ApplicationDbContext() : base(KeyVault.connectionString, throwIfV1Schema: false)
        //{
        //}
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
