using GDU_Management.IDao;
using GDU_Management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GDU_Management.DaoImpl
{
    class NganhHocImpl : IDaoNganhHoc
    {
        //tao ket noi database
        GDUDataConnectionsDataContext db;
        List<NganhHoc> nganhHocs;

        //lấy database từ cơ sở dữ liệu
        public NganhHocImpl()
        {
            db = new GDUDataConnectionsDataContext();
            using (db)
            {
                var nganhHoc = from x in db.NganhHocs select x;
                db.DeferredLoadingEnabled = true;
                nganhHocs = nganhHoc.ToList();
            }
        }

        // thêm ngành
        public NganhHoc CreateNganhHoc(NganhHoc nganhHoc)
        {
            db = new GDUDataConnectionsDataContext();
            NganhHoc nh = new NganhHoc();
            nh = nganhHoc;
            db.NganhHocs.InsertOnSubmit(nh);
            db.SubmitChanges();
            return nh;
        }

        //xóa ngành
        public void DeleteNganhHoc(string maNganhHoc)
        {
            db = new GDUDataConnectionsDataContext();
            NganhHoc nh = new NganhHoc();
            nh = db.NganhHocs.Single(x => x.MaNganh == maNganhHoc);
            db.NganhHocs.DeleteOnSubmit(nh);
            db.SubmitChanges();
        }

        public List<NganhHoc> GetAllNganhhoc()
        {
            return null;
        }

        // lấy ngành học theo KHOA
        //public NganhHoc GetNganhHocByKHOA(string maKhoa)
        //{
        //    db = new GDUDataConnectionsDataContext();
        //    NganhHoc nganhHoc = new NganhHoc();
        //    nganhHocs.ForEach(nganh =>
        //    {
        //        if( nganh.MaKhoa == maKhoa)
        //        {
        //            nganhHoc = nganh;
        //        }
        //    });
        //    return nganhHoc;
        //}

        //cập nhật ngành
        public void UpdateNganhHoc(NganhHoc nganhHoc)
        {

            db = new GDUDataConnectionsDataContext();
            NganhHoc nh = new NganhHoc();
            nh = db.NganhHocs.Single(x => x.MaNganh == nganhHoc.MaNganh);
            nh.TenNganh = nganhHoc.TenNganh;
            db.SubmitChanges();
        }

        List<NganhHoc> IDaoNganhHoc.GetNganhHocByKHOA(string maKhoa)
        {
            db = new GDUDataConnectionsDataContext();
            List<NganhHoc> nganhHoc = db.NganhHocs.Where(p => p.MaKhoa.Equals(maKhoa)).ToList();
            nganhHocs = new List<NganhHoc>();
            nganhHocs= nganhHoc;
            return nganhHocs;
        }
    }
}
