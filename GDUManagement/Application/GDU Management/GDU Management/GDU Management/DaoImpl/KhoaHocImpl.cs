using GDU_Management.IDao;
using GDU_Management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GDU_Management.DaoImpl
{
    class KhoaHocImpl : IDaoKhoaHoc
    {
        //tạo kết nối database 
        GDUDataConnectionsDataContext db;
        List<KhoaHoc> listkhoaHoc;

        public KhoaHocImpl()
        {
            db = new GDUDataConnectionsDataContext();
            using (db)
            {
                var khoaHoc = from x in db.KhoaHocs select x;
                db.DeferredLoadingEnabled = true;
                listkhoaHoc = khoaHoc.ToList();
            }
        }
        public KhoaHoc CreateKhoaHoc(KhoaHoc khoaHoc)
        {
            db = new GDUDataConnectionsDataContext();
            KhoaHoc khoaHocs = new KhoaHoc();

            return null;
        }

        public void DeleteKhoaHoc(string maKhoaHoc)
        {
            //code content
        }

        public List<KhoaHoc> GetAllKhoaHoc()
        {
            db = new GDUDataConnectionsDataContext();
            var kh = from x in db.KhoaHocs select x;
            listkhoaHoc = kh.ToList();
            return listkhoaHoc; ;
        }

        public void UpdateKhoaHoc(KhoaHoc khoaHoc)
        {
            //code content
        }
    }
}
