using GDU_Management.IDao;
using GDU_Management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDU_Management.DaoImpl
{
    class LopImpl : IDaoLop
    {
        //tạo kết nối database 
        GDUDataConnectionsDataContext db;
        List<Lop> listLop;
        public Lop CreateLop(Lop lop)
        {
            db = new GDUDataConnectionsDataContext();
            db.Lops.InsertOnSubmit(lop);
            db.SubmitChanges();
            return lop;
        }

        public void DeleteLop(string maLop)
        {
            db = new GDUDataConnectionsDataContext();
            Lop lop = new Lop();
            lop = db.Lops.Single(x => x.MaLop == maLop);
            db.Lops.DeleteOnSubmit(lop);
            db.SubmitChanges();
        }

        public List<Lop> getAllLop()
        {
            //code content
            return null;
        }

        public List<Lop> GetDanhSachLopByMaNganhVaMaKhoaHoc(string maNganh, string maKhoaHoc)
        {
            db = new GDUDataConnectionsDataContext();
            var lop = from x in db.Lops where x.MaNganh == maNganh && x.MaKhoaHoc == maKhoaHoc select x;
            listLop = new List<Lop>();
            listLop = lop.ToList();
            return listLop;
        }

        public void UpdateLop(Lop lop)
        {
            db = new GDUDataConnectionsDataContext();
            Lop lp = new Lop();
            lp = db.Lops.Single(x => x.MaKhoaHoc == lop.MaKhoaHoc);
            lp.TenLop = lop.TenLop;
            lp.MaKhoaHoc = lop.MaKhoaHoc;
            lp.MaNganh = lop.MaNganh;
            db.SubmitChanges();
            MessageBox.Show("sai o lopImpl");

            //db = new GDUDataConnectionsDataContext();
            //KhoaHoc khoaHocs = new KhoaHoc();
            //khoaHocs = db.KhoaHocs.Single(x => x.MaKhoaHoc == khoaHoc.MaKhoaHoc);
            //khoaHocs.TenKhoaHoc = khoaHoc.TenKhoaHoc;
            //khoaHocs.NienKhoa = khoaHoc.NienKhoa;
            //db.SubmitChanges();

            //db = new GDUDataConnectionsDataContext();
            //NganhHoc nh = new NganhHoc();
            //nh = db.NganhHocs.Single(x => x.MaNganh == nganhHoc.MaNganh);
            //nh.TenNganh = nganhHoc.TenNganh;
            //db.SubmitChanges();
        }
    }
}
