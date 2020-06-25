using GDU_Management.IDao;
using GDU_Management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GDU_Management.DaoImpl
{
    class SinhVienImpl : IDaoSinhVien
    {
        //tao ket noi database
        GDUDataConnectionsDataContext db;
        List<SinhVien> listSinhViens;

        //thêm mới một sinh viên
        public SinhVien CreateSinhVien(SinhVien sinhVien)
        {
            db = new GDUDataConnectionsDataContext();
            db.SinhViens.InsertOnSubmit(sinhVien);
            db.SubmitChanges();
            return null;
        }

        //xóa tất cả sinh viên
        public void DeleteAllSinhVienByMaLop(string  maLop)
        {
            db = new GDUDataConnectionsDataContext();
            List<SinhVien> sv = db.SinhViens.Where(p => p.MaLop.Equals(maLop)).ToList();
            listSinhViens = sv.ToList();
            db.SinhViens.DeleteAllOnSubmit(listSinhViens);
            db.SubmitChanges();
        }

        //xóa sinh viên
        public void DeleteSinhVien(string maSV)
        {
            db = new GDUDataConnectionsDataContext();
            SinhVien sv = new SinhVien();
            sv = db.SinhViens.Single(p => p.MaSV == maSV);
            db.SinhViens.DeleteOnSubmit(sv);
            db.SubmitChanges();
        }
        public List<SinhVien> GetAllSinhVien()
        {
            db = new GDUDataConnectionsDataContext();
            var sv = from x in db.SinhViens select x;
            listSinhViens = sv.ToList();
            return listSinhViens;
        }

        //lấy danh sách sinh viên theo mã lớp
        public List<SinhVien> GetSinhVienByMaLop(string maLop)
        {
            db = new GDUDataConnectionsDataContext();
            List<SinhVien> sv = db.SinhViens.Where(p => p.MaLop.Equals(maLop)).ToList();
            listSinhViens = new List<SinhVien>();
            listSinhViens = sv;
            return listSinhViens;
        }

        //tìm kiếm sinh viên theo tên
        public List<SinhVien> SearchSinhVienByTenSinhVien(string tenSV)
        {
            db = new GDUDataConnectionsDataContext();
            var sv = from x in db.SinhViens where x.TenSV.Contains(tenSV) select x;
            listSinhViens = sv.ToList();
            return listSinhViens;
        }

        //cập nhật sinh viên
        public void UpdateSinhVien(SinhVien sinhVien)
        {
            db = new GDUDataConnectionsDataContext();
            SinhVien sv = new SinhVien();
            sv = db.SinhViens.Single(p => p.MaSV == sinhVien.MaSV);
            sv.TenSV = sinhVien.TenSV;
            sv.GioiTinh = sinhVien.GioiTinh;
            sv.Email = sinhVien.Email;
            //sv.Password = sinhVien.Password;
            sv.NamSinh = sinhVien.NamSinh;
            sv.SDT = sinhVien.SDT;
            sv.DiaChi = sinhVien.DiaChi;
            sv.GhiChu = sinhVien.GhiChu;
            sv.MaLop = sinhVien.MaLop;
            db.SubmitChanges();
        }
    }
}
