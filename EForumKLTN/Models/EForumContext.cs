using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EForumKLTN.Models;

public partial class EForumContext : DbContext
{
    public EForumContext()
    {
    }

    public EForumContext(DbContextOptions<EForumContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BaiViet> BaiViets { get; set; }

    public virtual DbSet<ChiTietHd> ChiTietHds { get; set; }

    public virtual DbSet<HangHoa> HangHoas { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<LichSuChatbot> LichSuChatbots { get; set; }

    public virtual DbSet<Loai> Loais { get; set; }

    public virtual DbSet<ChuDe> ChuDes { get; set; }

    public virtual DbSet<BinhLuan> BinhLuans { get; set; }
    public DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<ChatBotFaQ> ChatBotFaQs { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //      => optionsBuilder.UseSqlServer("Data Source=DESKTOP-66DOCVE;Initial Catalog=EForumDB;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaiViet>(entity =>
        {
            entity.HasKey(e => e.MaBv).HasName("PK__BaiViet__272475951AEDD0C2");

            entity.ToTable("BaiViet");

            entity.Property(e => e.MaBv).HasColumnName("MaBV");
            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.NgayDang)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TieuDe).HasMaxLength(250);

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.BaiViets)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__BaiViet__MaKH__5CD6CB2B");

            entity.HasOne(d => d.MaCdNavigation).WithMany(p => p.BaiViets)
                .HasForeignKey(d => d.MaCd)
                .HasConstraintName("FK_BaiViet_ChuDe");
        });

        modelBuilder.Entity<ChiTietHd>(entity =>
        {
            entity.HasKey(e => e.MaCt).HasName("PK__ChiTietH__27258E74B04A59B0");

            entity.ToTable("ChiTietHD");

            entity.Property(e => e.MaCt).HasColumnName("MaCT");
            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.MaHh).HasColumnName("MaHH");

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.ChiTietHds)
                .HasForeignKey(d => d.MaHd)
                .HasConstraintName("FK__ChiTietHD__MaHD__5812160E");

            entity.HasOne(d => d.MaHhNavigation).WithMany(p => p.ChiTietHds)
                .HasForeignKey(d => d.MaHh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHD__MaHH__59063A47");
        });

        modelBuilder.Entity<HangHoa>(entity =>
        {
            entity.HasKey(e => e.MaHh).HasName("PK__HangHoa__2725A6E439B46D93");

            entity.ToTable("HangHoa");

            entity.Property(e => e.MaHh).HasColumnName("MaHH");
            entity.Property(e => e.Hinh).HasMaxLength(50);
            entity.Property(e => e.TenHh)
                .HasMaxLength(50)
                .HasColumnName("TenHH");

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.HangHoas)
                .HasForeignKey(d => d.MaLoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HangHoa__MaLoai__4F7CD00D");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHd).HasName("PK__HoaDon__2725A6E0823BA0F1");

            entity.ToTable("HoaDon");

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.CachThanhToan)
                .HasMaxLength(50)
                .HasDefaultValue("Tiền mặt");
            entity.Property(e => e.DiaChi).HasMaxLength(60);
            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.MaTrangThai).HasDefaultValue(0);
            entity.Property(e => e.NgayDat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaKh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HoaDon__MaKH__52593CB8");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK__KhachHan__2725CF1E24894199");

            entity.ToTable("KhachHang");

            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.DiaChi).HasMaxLength(60); //duma no thieu code deo chay dc :))
            entity.Property(e => e.DienThoai).HasMaxLength(24);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Hinh)
                .HasMaxLength(50)
                .HasDefaultValue("Photo.gif"); //hoi AI cai nay` chua hieu lam 
            entity.Property(e => e.HieuLuc).HasDefaultValue(true);
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MatKhau).HasMaxLength(50);
            entity.Property(e => e.RandomKey)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LichSuChatbot>(entity =>
        {
            entity.HasKey(e => e.MaChat).HasName("PK__LichSuCh__1B56CB6B0994B5D6");

            entity.ToTable("LichSuChatbot");

            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.NgayChat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TraLoiAi).HasColumnName("TraLoiAI");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.LichSuChatbots)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__LichSuChat__MaKH__60A75C0F");
        });

        modelBuilder.Entity<Loai>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK__Loai__730A5759EC29BC0E");

            entity.ToTable("Loai");

            entity.Property(e => e.TenLoai).HasMaxLength(50);
        });

        modelBuilder.Entity<ChuDe>(entity =>
        {
            entity.HasKey(e => e.MaCd).HasName("PK_ChuDe");

            entity.ToTable("ChuDe");

            entity.Property(e => e.MaCd).HasColumnName("MaCD");
            entity.Property(e => e.TenChuDe).HasMaxLength(100);
            entity.Property(e => e.MoTa).HasMaxLength(500);
            entity.HasMany(d => d.BaiViets)
            .WithOne(p => p.MaCdNavigation)
            .HasForeignKey(p => p.MaCd)
            .HasConstraintName("FK_BaiViet_ChuDe");
        });

        modelBuilder.Entity<BinhLuan>(entity =>
        {
            entity.HasKey(e => e.MaBl).HasName("PK_BinhLuan");

            entity.ToTable("BinhLuan");

            entity.Property(e => e.MaBl).HasColumnName("MaBL");
            entity.Property(e => e.MaBv).HasColumnName("MaBV");
            entity.Property(e => e.MaKh).HasMaxLength(20).HasColumnName("MaKH");
            entity.Property(e => e.NoiDung).HasColumnType("nvarchar(max)");

            entity.HasOne(d => d.MaBvNavigation).WithMany(p => p.BinhLuans)
                .HasForeignKey(d => d.MaBv)
                .HasConstraintName("FK_BinhLuan_BaiViet");
            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.BinhLuans)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK_BinhLuan_KhachHang");
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.MaCoupon).HasName("PK_Coupon");

            entity.ToTable("Coupon");

            entity.Property(e => e.MaCoupon)
                .HasMaxLength(50);

            entity.Property(e => e.MaKH_NV)
                .HasMaxLength(20)
                .HasColumnName("MaKH_NV");

            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.KhachHang)
                .WithMany(p => p.Coupons)
                .HasForeignKey(d => d.MaKH_NV)
                .HasConstraintName("FK_Coupon_KhachHang");
        });

        modelBuilder.Entity<ChatBotFaQ>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("ChatbotFAQ");

            entity.Property(e => e.TieuDe)
                .HasMaxLength(255);

            entity.Property(e => e.NgayTao)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
