using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDU_Management.IDao;
using GDU_Management.Model;

namespace GDU_Management.DaoImpl
{
    class MonHocImpl: IDaoMonHoc
    {
        //tao ket noi database
        GDUDataConnectionsDataContext db;
        List<MonHoc> listMonHocs;

        public MonHoc CreateMonHoc(MonHoc monHoc)
        {
            db = new GDUDataConnectionsDataContext();
            MonHoc mhoc = new MonHoc();
            mhoc = monHoc;
            db.MonHocs.InsertOnSubmit(mhoc);
            db.SubmitChanges();
            return mhoc;
        }

        public void DeleteMonHoc(string maMonHoc)
        {
            db = new GDUDataConnectionsDataContext();
            MonHoc monhoc = new MonHoc();
            monhoc = db.MonHocs.Single(x => x.MaMonHoc == maMonHoc);
            db.MonHocs.DeleteOnSubmit(monhoc);
            db.SubmitChanges();
        }

        public List<MonHoc> GetAllMonHoc()
        {
            //code content
            return null;
        }

        public List<MonHoc> GetMonHocByNganh(string maNganh)
        {
            db = new GDUDataConnectionsDataContext();
            List<MonHoc> monHoc = db.MonHocs.Where(p => p.MaNganh.Equals(maNganh)).ToList();
            listMonHocs = new List<MonHoc>();
            listMonHocs = monHoc.ToList();
            return listMonHocs;
        }

        public void UpdateMonHoc(MonHoc monHoc)
        {
            db = new GDUDataConnectionsDataContext();
            MonHoc mh = new MonHoc();
            mh = db.MonHocs.Single(x => x.MaMonHoc == monHoc.MaMonHoc);
            mh.TenMonHoc = monHoc.TenMonHoc;
            mh.STC = monHoc.STC;
            db.SubmitChanges();
        }
    }
}
