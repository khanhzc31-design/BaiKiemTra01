using System;
using System.Collections.Generic;
using System.Linq;

namespace LogisticsAutoSpeed
{
    public abstract class PhuongTien
    {
        private string _maPT = "PT000";
        private string _tenHang = string.Empty;
        private int _namSanXuat;
        private decimal _giaGoc;

        public string MaPT
        {
            get => _maPT;
            set => _maPT = string.IsNullOrWhiteSpace(value) ? "PT000" : value.Trim();
        }

        public string TenHang
        {
            get => _tenHang;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Ten hang khong duoc de trong!");
                _tenHang = value.Trim();
            }
        }

        public int NamSanXuat
        {
            get => _namSanXuat;
            set
            {
                int currentYear = DateTime.Now.Year;
                if (value < 1900 || value > currentYear)
                    throw new ArgumentException("Nam san xuat khong hop le!");
                _namSanXuat = value;
            }
        }

        public decimal GiaGoc
        {
            get => _giaGoc;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Gia goc phai lon hon 0!");
                _giaGoc = value;
            }
        }

        protected PhuongTien(string maPT, string tenHang, int namSanXuat, decimal giaGoc)
        {
            MaPT = maPT;
            TenHang = tenHang;
            NamSanXuat = namSanXuat;
            GiaGoc = giaGoc;
        }

        public abstract decimal TinhGiaLanBanh();

        public virtual string GetInfo()
        {
            return $"[Ma: {MaPT}] - Hang: {TenHang} - Nam SX: {NamSanXuat} - Gia goc: {GiaGoc:N0} VND";
        }
    }

    public class OTo : PhuongTien
    {
        private int _soChoNgoi;
        private double _dungTichDongCo;

        public int SoChoNgoi
        {
            get => _soChoNgoi;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("So cho ngoi phai lon hon 0!");
                _soChoNgoi = value;
            }
        }

        public double DungTichDongCo
        {
            get => _dungTichDongCo;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Dung tich dong co phai lon hon 0!");
                _dungTichDongCo = value;
            }
        }

        public OTo(string maPT, string tenHang, int namSanXuat, decimal giaGoc, int soChoNgoi, double dungTichDongCo)
            : base(maPT, tenHang, namSanXuat, giaGoc)
        {
            SoChoNgoi = soChoNgoi;
            DungTichDongCo = dungTichDongCo;
        }

        public override decimal TinhGiaLanBanh()
        {
            if (SoChoNgoi <= 9)
            {
                return GiaGoc + (0.12m * GiaGoc) + (0.30m * GiaGoc);
            }
            else
            {
                return GiaGoc + (0.10m * GiaGoc);
            }
        }

        public override string GetInfo()
        {
            return $"{base.GetInfo()} - Loai: O to ({SoChoNgoi} cho, {DungTichDongCo}L) - Gia lan banh: {TinhGiaLanBanh():N0} VND";
        }
    }

    public class XeMay : PhuongTien
    {
        private int _dungTichXylanh;

        public int DungTichXylanh
        {
            get => _dungTichXylanh;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Dung tich xi lanh phai lon hon 0!");
                _dungTichXylanh = value;
            }
        }

        public XeMay(string maPT, string tenHang, int namSanXuat, decimal giaGoc, int dungTichXylanh)
            : base(maPT, tenHang, namSanXuat, giaGoc)
        {
            DungTichXylanh = dungTichXylanh;
        }

        public override decimal TinhGiaLanBanh()
        {
            if (DungTichXylanh < 175)
            {
                return GiaGoc + (0.02m * GiaGoc);
            }
            else
            {
                return GiaGoc + (0.05m * GiaGoc);
            }
        }

        public override string GetInfo()
        {
            return $"{base.GetInfo()} - Loai: Xe may ({DungTichXylanh}cc) - Gia lan banh: {TinhGiaLanBanh():N0} VND";
        }
    }

    public class QuanLyPhuongTien
    {
        private readonly List<PhuongTien> _danhSach = new();

        public void AddPhuongTien(PhuongTien pt)
        {
            if (pt != null)
            {
                _danhSach.Add(pt);
            }
        }

        public void DisplayAll()
        {
            if (_danhSach.Count == 0)
            {
                Console.WriteLine("\nDanh sach phuong tien trong!");
                return;
            }

            Console.WriteLine("\n--- DANH SACH PHUONG TIEN GIAO THONG ---");
            foreach (var pt in _danhSach)
            {
                Console.WriteLine(pt.GetInfo());
            }
        }

        public void TinhGiaLanBanhDanhSach()
        {
            if (_danhSach.Count == 0)
            {
                Console.WriteLine("\nDanh sach phuong tien trong!");
                return;
            }

            Console.WriteLine("\n--- BANG TINH GIA LAN BANH CAC PHUONG TIEN ---");
            foreach (var pt in _danhSach)
            {
                Console.WriteLine($"[Ma: {pt.MaPT}] - Hang: {pt.TenHang} - Gia goc: {pt.GiaGoc:N0} VND => Gia lan banh: {pt.TinhGiaLanBanh():N0} VND");
            }
        }

        public PhuongTien? FindMaxGiaLanBanh()
        {
            if (_danhSach.Count == 0) return null;

            return _danhSach.OrderByDescending(pt => pt.TinhGiaLanBanh()).FirstOrDefault();
        }

        public List<PhuongTien> SearchByName(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<PhuongTien>();

            return _danhSach
                .Where(pt => pt.TenHang.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            QuanLyPhuongTien ql = new QuanLyPhuongTien();

            while (true)
            {
                Console.WriteLine("\n================ MENU QUAN LY ================");
                Console.WriteLine("1. Nhap O to");
                Console.WriteLine("2. Nhap Xe may");
                Console.WriteLine("3. Hien thi danh sach");
                Console.WriteLine("4. Tinh gia lan banh cac phuong tien trong danh sach");
                Console.WriteLine("5. Tim phuong tien co gia lan banh cao nhat");
                Console.WriteLine("6. Tim kiem phuong tien theo ten hang");
                Console.WriteLine("0. Thoat");
                Console.Write("Chon chuc nang (0-6): ");

                string chon = Console.ReadLine() ?? "";
                if (chon == "0") break;

                switch (chon)
                {
                    case "1":
                        ThemOTo(ql);
                        break;
                    case "2":
                        ThemXeMay(ql);
                        break;
                    case "3":
                        ql.DisplayAll();
                        break;
                    case "4":
                        ql.TinhGiaLanBanhDanhSach();
                        break;
                    case "5":
                        var max = ql.FindMaxGiaLanBanh();
                        if (max != null)
                        {
                            Console.WriteLine($"\nPhuong tien gia lan banh cao nhat:\n{max.GetInfo()}");
                        }
                        else
                        {
                            Console.WriteLine("\nDanh sach trong!");
                        }
                        break;
                    case "6":
                        Console.Write("\nNhap ten hang can tim: ");
                        string kw = Console.ReadLine() ?? "";
                        var kq = ql.SearchByName(kw);
                        if (kq.Count > 0)
                        {
                            Console.WriteLine("\nKet qua tim kiem:");
                            foreach (var item in kq)
                            {
                                Console.WriteLine(item.GetInfo());
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nKhong tim thay ket qua!");
                        }
                        break;
                    default:
                        Console.WriteLine("\nChuc nang khong hop le!");
                        break;
                }
            }
        }

        private static void ThemOTo(QuanLyPhuongTien ql)
        {
            try
            {
                Console.WriteLine("\n--- NHAP THONG TIN O TO ---");
                Console.Write("Ma PT: ");
                string ma = Console.ReadLine() ?? "";
                Console.Write("Ten hang: ");
                string hang = Console.ReadLine() ?? "";
                Console.Write("Nam san xuat: ");
                int nam = int.Parse(Console.ReadLine() ?? "0");
                Console.Write("Gia goc: ");
                decimal gia = decimal.Parse(Console.ReadLine() ?? "0");
                Console.Write("So cho ngoi: ");
                int soCho = int.Parse(Console.ReadLine() ?? "0");
                Console.Write("Dung tich dong co (L): ");
                double dungTich = double.Parse(Console.ReadLine() ?? "0");

                OTo oto = new OTo(ma, hang, nam, gia, soCho, dungTich);
                ql.AddPhuongTien(oto);
                Console.WriteLine("Them O to thanh cong!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Loi: {ex.Message}");
            }
        }

        private static void ThemXeMay(QuanLyPhuongTien ql)
        {
            try
            {
                Console.WriteLine("\n--- NHAP THONG TIN XE MAY ---");
                Console.Write("Ma PT: ");
                string ma = Console.ReadLine() ?? "";
                Console.Write("Ten hang: ");
                string hang = Console.ReadLine() ?? "";
                Console.Write("Nam san xuat: ");
                int nam = int.Parse(Console.ReadLine() ?? "0");
                Console.Write("Gia goc: ");
                decimal gia = decimal.Parse(Console.ReadLine() ?? "0");
                Console.Write("Dung tich xi lanh (cc): ");
                int dungTich = int.Parse(Console.ReadLine() ?? "0");

                XeMay xm = new XeMay(ma, hang, nam, gia, dungTich);
                ql.AddPhuongTien(xm);
                Console.WriteLine("Them Xe may thanh cong!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Loi: {ex.Message}");
            }
        }
    }
}