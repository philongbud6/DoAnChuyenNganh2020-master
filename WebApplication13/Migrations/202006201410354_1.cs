namespace WebApplication13.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CTDonHangs",
                c => new
                    {
                        CTDonHangId = c.Int(nullable: false, identity: true),
                        SoLuong = c.Int(nullable: false),
                        DonGia = c.Double(nullable: false),
                        GiamGia = c.Double(nullable: false),
                        DonhangId = c.Int(nullable: false),
                        SanPhamId = c.Int(nullable: false),
                        NhanVienId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CTDonHangId)
                .ForeignKey("dbo.DonHangs", t => t.DonhangId, cascadeDelete: true)
                .ForeignKey("dbo.SanPhams", t => t.SanPhamId, cascadeDelete: true)
                .Index(t => t.DonhangId)
                .Index(t => t.SanPhamId);
            
            CreateTable(
                "dbo.DonHangs",
                c => new
                    {
                        DonHangId = c.Int(nullable: false, identity: true),
                        SoLuongBan = c.Int(nullable: false),
                        HinhThucTT = c.String(),
                        TongTien = c.Int(nullable: false),
                        NgayMua = c.DateTime(nullable: false),
                        KhachHangId = c.Int(nullable: false),
                        CuaHangId = c.Int(nullable: false),
                        TrangThaiDH = c.String(),
                        TokenKey = c.String(),
                    })
                .PrimaryKey(t => t.DonHangId)
                .ForeignKey("dbo.CuaHangs", t => t.CuaHangId, cascadeDelete: true)
                .ForeignKey("dbo.KhachHangs", t => t.KhachHangId, cascadeDelete: true)
                .Index(t => t.KhachHangId)
                .Index(t => t.CuaHangId);
            
            CreateTable(
                "dbo.CuaHangs",
                c => new
                    {
                        CuaHangId = c.Int(nullable: false, identity: true),
                        TenCuaHang = c.String(nullable: false),
                        displayName = c.String(),
                        DiaChi = c.String(),
                        Show = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CuaHangId);
            
            CreateTable(
                "dbo.KhachHangs",
                c => new
                    {
                        KhachHangId = c.Int(nullable: false, identity: true),
                        TenKH = c.String(nullable: false),
                        DiaChi = c.String(nullable: false),
                        NgaySinh = c.DateTime(nullable: false),
                        SoDT = c.String(),
                        EmailKH = c.String(),
                    })
                .PrimaryKey(t => t.KhachHangId);
            
            CreateTable(
                "dbo.SanPhams",
                c => new
                    {
                        SanPhamId = c.Int(nullable: false, identity: true),
                        MaSanPham = c.String(nullable: false),
                        TenSP = c.String(),
                        SoLuong = c.Int(nullable: false),
                        tempSoLuong = c.Int(nullable: false),
                        MoTa = c.String(),
                        DonGia = c.Int(nullable: false),
                        NhaCungCapId = c.Int(nullable: false),
                        LoaiSPId = c.Int(nullable: false),
                        TopOffer = c.Boolean(nullable: false),
                        Ghim = c.Int(nullable: false),
                        Image2 = c.Binary(),
                        Url_img2 = c.String(),
                        Image1 = c.Binary(),
                        Url_img1 = c.String(),
                        NgayTao = c.DateTime(nullable: false),
                        Xoa = c.Boolean(nullable: false),
                        KhoHangId = c.Int(nullable: false),
                        Show = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SanPhamId)
                .ForeignKey("dbo.KhoHangs", t => t.KhoHangId, cascadeDelete: true)
                .ForeignKey("dbo.LoaiSPs", t => t.LoaiSPId, cascadeDelete: true)
                .Index(t => t.LoaiSPId)
                .Index(t => t.KhoHangId);
            
            CreateTable(
                "dbo.KhoHangs",
                c => new
                    {
                        KhoHangId = c.Int(nullable: false, identity: true),
                        TenKho = c.String(nullable: false),
                        CuaHangId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.KhoHangId);
            
            CreateTable(
                "dbo.LoaiSPs",
                c => new
                    {
                        LoaiSPId = c.Int(nullable: false, identity: true),
                        TenLoai = c.String(nullable: false),
                        ChuThich = c.String(),
                        Xoa = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LoaiSPId);
            
            CreateTable(
                "dbo.CTPhieuChuyenKhoes",
                c => new
                    {
                        CTPhieuChuyenKhoId = c.Int(nullable: false, identity: true),
                        SoLuongChuyen = c.Int(nullable: false),
                        PhieuChuyenKhoId = c.Int(nullable: false),
                        SanPhamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CTPhieuChuyenKhoId)
                .ForeignKey("dbo.PhieuChuyenKhoes", t => t.PhieuChuyenKhoId, cascadeDelete: true)
                .ForeignKey("dbo.SanPhams", t => t.SanPhamId, cascadeDelete: true)
                .Index(t => t.PhieuChuyenKhoId)
                .Index(t => t.SanPhamId);
            
            CreateTable(
                "dbo.PhieuChuyenKhoes",
                c => new
                    {
                        PhieuChuyenKhoId = c.Int(nullable: false, identity: true),
                        TenPhieuChuyenKho = c.String(),
                        SoLuongChuyen = c.Int(nullable: false),
                        TrangThaiId = c.Int(nullable: false),
                        FromKho = c.Int(nullable: false),
                        ToKho = c.Int(nullable: false),
                        NgayTao = c.DateTime(nullable: false),
                        ConfirmFrom = c.Boolean(nullable: false),
                        NgayConfirmFrom = c.DateTime(),
                        ConfirmTo = c.Boolean(nullable: false),
                        NgayConfirmTo = c.DateTime(),
                        KhoHang_KhoHangId = c.Int(),
                    })
                .PrimaryKey(t => t.PhieuChuyenKhoId)
                .ForeignKey("dbo.KhoHangs", t => t.KhoHang_KhoHangId)
                .ForeignKey("dbo.TrangThais", t => t.TrangThaiId, cascadeDelete: true)
                .Index(t => t.TrangThaiId)
                .Index(t => t.KhoHang_KhoHangId);
            
            CreateTable(
                "dbo.TrangThais",
                c => new
                    {
                        TrangThaiId = c.Int(nullable: false, identity: true),
                        TenTrangThai = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.TrangThaiId);
            
            CreateTable(
                "dbo.NhaCungCaps",
                c => new
                    {
                        NhaCungCapId = c.Int(nullable: false, identity: true),
                        TenNhaCungCap = c.String(nullable: false),
                        DiaChi = c.String(),
                        LoaiSPId = c.Int(nullable: false),
                        Image = c.Binary(),
                        Url_Image = c.String(),
                        NgayTao = c.DateTime(nullable: false),
                        ChuThich = c.String(),
                        Xoa = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.NhaCungCapId)
                .ForeignKey("dbo.LoaiSPs", t => t.LoaiSPId, cascadeDelete: true)
                .Index(t => t.LoaiSPId);
            
            CreateTable(
                "dbo.NhanViens",
                c => new
                    {
                        NhanVienId = c.Int(nullable: false, identity: true),
                        TenNhanVien = c.String(nullable: false),
                        SoDienThoai = c.String(),
                        DiaChi = c.String(),
                        CuaHangId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NhanVienId)
                .ForeignKey("dbo.CuaHangs", t => t.CuaHangId, cascadeDelete: true)
                .Index(t => t.CuaHangId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CuaHangId = c.Int(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        FullName = c.String(),
                        SoDT = c.String(),
                        DiaChi = c.String(),
                        IsAccount = c.Boolean(nullable: false),
                        Xoa = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CuaHangs", t => t.CuaHangId, cascadeDelete: true)
                .Index(t => t.CuaHangId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "CuaHangId", "dbo.CuaHangs");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.NhanViens", "CuaHangId", "dbo.CuaHangs");
            DropForeignKey("dbo.NhaCungCaps", "LoaiSPId", "dbo.LoaiSPs");
            DropForeignKey("dbo.CTPhieuChuyenKhoes", "SanPhamId", "dbo.SanPhams");
            DropForeignKey("dbo.CTPhieuChuyenKhoes", "PhieuChuyenKhoId", "dbo.PhieuChuyenKhoes");
            DropForeignKey("dbo.PhieuChuyenKhoes", "TrangThaiId", "dbo.TrangThais");
            DropForeignKey("dbo.PhieuChuyenKhoes", "KhoHang_KhoHangId", "dbo.KhoHangs");
            DropForeignKey("dbo.CTDonHangs", "SanPhamId", "dbo.SanPhams");
            DropForeignKey("dbo.SanPhams", "LoaiSPId", "dbo.LoaiSPs");
            DropForeignKey("dbo.SanPhams", "KhoHangId", "dbo.KhoHangs");
            DropForeignKey("dbo.CTDonHangs", "DonhangId", "dbo.DonHangs");
            DropForeignKey("dbo.DonHangs", "KhachHangId", "dbo.KhachHangs");
            DropForeignKey("dbo.DonHangs", "CuaHangId", "dbo.CuaHangs");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "CuaHangId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.NhanViens", new[] { "CuaHangId" });
            DropIndex("dbo.NhaCungCaps", new[] { "LoaiSPId" });
            DropIndex("dbo.PhieuChuyenKhoes", new[] { "KhoHang_KhoHangId" });
            DropIndex("dbo.PhieuChuyenKhoes", new[] { "TrangThaiId" });
            DropIndex("dbo.CTPhieuChuyenKhoes", new[] { "SanPhamId" });
            DropIndex("dbo.CTPhieuChuyenKhoes", new[] { "PhieuChuyenKhoId" });
            DropIndex("dbo.SanPhams", new[] { "KhoHangId" });
            DropIndex("dbo.SanPhams", new[] { "LoaiSPId" });
            DropIndex("dbo.DonHangs", new[] { "CuaHangId" });
            DropIndex("dbo.DonHangs", new[] { "KhachHangId" });
            DropIndex("dbo.CTDonHangs", new[] { "SanPhamId" });
            DropIndex("dbo.CTDonHangs", new[] { "DonhangId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.NhanViens");
            DropTable("dbo.NhaCungCaps");
            DropTable("dbo.TrangThais");
            DropTable("dbo.PhieuChuyenKhoes");
            DropTable("dbo.CTPhieuChuyenKhoes");
            DropTable("dbo.LoaiSPs");
            DropTable("dbo.KhoHangs");
            DropTable("dbo.SanPhams");
            DropTable("dbo.KhachHangs");
            DropTable("dbo.CuaHangs");
            DropTable("dbo.DonHangs");
            DropTable("dbo.CTDonHangs");
        }
    }
}
