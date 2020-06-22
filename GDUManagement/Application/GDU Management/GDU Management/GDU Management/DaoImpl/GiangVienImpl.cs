using GDU_Management.IDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDU_Management.Model;

namespace GDU_Management.DaoImpl
{
    class GiangVienImpl : IDaoGiangVien
    {
        //tao ket noi database
        GDUDataConnectionsDataContext db = new GDUDataConnectionsDataContext();
        List<GiangVien> giangVien;
        public GiangVien CreateGiangVien(GiangVien giangVien)
        {
            db = new GDUDataConnectionsDataContext();
            GiangVien gv = new GiangVien();
            gv = giangVien;
            db.GiangViens.InsertOnSubmit(gv);
            db.SubmitChanges();
            return gv;
        }

        public void DeleteGiangVien(string maGV)
        {
            db = new GDUDataConnectionsDataContext();
            GiangVien gv = new GiangVien();
            gv = db.GiangViens.Single(x => x.MaGV == maGV);
            db.GiangViens.DeleteOnSubmit(gv);
            db.SubmitChanges();
        }

        public List<GiangVien> GetAllGiangVien()
        {
            db = new GDUDataConnectionsDataContext();
            var gv = from x in db.GiangViens select x;
            giangVien = gv.ToList();
            return giangVien;
        }

        public void UpdateGiangVien(GiangVien giangVien)
        {
            db = new GDUDataConnectionsDataContext();
            GiangVien gv = new GiangVien();
            gv = db.GiangViens.Single(x => x.MaGV == giangVien.MaGV);
            gv.TenGV = giangVien.TenGV;
            gv.GioiTinh = giangVien.GioiTinh;
            gv.TrinhDo = giangVien.TrinhDo;
            gv.SDT = giangVien.SDT;
            gv.Email = giangVien.Email;
            gv.GhiChu = giangVien.GhiChu;
            gv.DiaChi = giangVien.DiaChi;
            db.SubmitChanges();
        }

        //public List<GiangVien> SearchKhoaByTenKhoa(string tenKhoa)
        //{
        //    db = new GDUDataConnectionsDataContext();
        //    var khoa = from x in db.Khoas where x.TenKhoa.Contains(tenKhoa) select x;
        //    listKhoas = khoa.ToList();
        //    return listKhoas;
        //}
        public List<GiangVien> SearchGiangVienByTenGV(string tenGV)
        {
            db = new GDUDataConnectionsDataContext();
            var giangViens = from x in db.GiangViens where x.TenGV.Contains(tenGV) select x;
            giangVien = giangViens.ToList();
            return giangVien;
        }

        public List<GiangVien> GetMaGVByTenGV(string maGV)
        {
           
            db = new GDUDataConnectionsDataContext();
            List<GiangVien> gv = db.GiangViens.Where(p => p.MaGV.Equals(maGV)).ToList();
            giangVien = new List<GiangVien>();
            giangVien = gv.ToList();
            return giangVien;

        }
    }
}
