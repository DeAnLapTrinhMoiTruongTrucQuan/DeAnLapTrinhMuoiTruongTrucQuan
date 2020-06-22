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
        GDUDataConnectionsDataContext db = new GDUDataConnectionsDataContext();
        List<SinhVien> listSinhViens;

        public SinhVien CreateSinhVien(SinhVien sinhVien)
        {
            db = new GDUDataConnectionsDataContext();
            db.SinhViens.InsertOnSubmit(sinhVien);
            db.SubmitChanges();
            return null;
        }

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
            var sv = from x in db.SinhViens select x;
            listSinhViens = sv.ToList();
            return listSinhViens;
        }

        public List<SinhVien> GetSinhVienByMaLop(string maLop)
        {
            db = new GDUDataConnectionsDataContext();
            List<SinhVien> sv = db.SinhViens.Where(p => p.MaLop.Equals(maLop)).ToList();
            listSinhViens = new List<SinhVien>();
            listSinhViens = sv;
            return listSinhViens;
        }

        public void UpdateSinhVien(SinhVien sinhVien)
        {
            db = new GDUDataConnectionsDataContext();
            SinhVien sv = new SinhVien();
            sv = db.SinhViens.Single(p => p.MaSV == sinhVien.MaSV);
            sv.TenSV = sinhVien.TenSV;
            sv.GioiTinh = sinhVien.GioiTinh;
            sv.Email = sinhVien.Email;
            sv.Password = sinhVien.Password;
            sv.NamSinh = sinhVien.NamSinh;
            sv.SDT = sinhVien.SDT;
            sv.DiaChi = sinhVien.DiaChi;
            sv.GhiChu = sv.GhiChu;
            sv.MaLop = sv.MaLop;
            db.SubmitChanges();
        }
    }
}
