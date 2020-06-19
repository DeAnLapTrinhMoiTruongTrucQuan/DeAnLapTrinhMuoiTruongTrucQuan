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
        GDUDataConnectionsDataContext db = new GDUDataConnectionsDataContext();
        List<MonHoc> listMonHoc;
        

        public SinhVien CreateMonHoc(MonHoc monHoc)
        {
            //code content
            return null;
        }

        public void DeleteMonHoc(string maMonHoc)
        {
            //code content
        }

        public List<MonHoc> GetAllMonHoc()
        {
            
            db = new GDUDataConnectionsDataContext();
            var k = from x in db.MonHocs select x;
            listMonHoc = k.ToList();
            return listMonHoc;

        }

        public List<MonHoc> GetMonHocByMaNganh(string maNganh)
        {
            db = new GDUDataConnectionsDataContext();
            List<MonHoc> mh = db.MonHocs.Where(p => p.MaNganh.Equals(maNganh)).ToList();
            listMonHoc = new List<MonHoc>();
            listMonHoc = mh.ToList();
            return listMonHoc;
            
        }

        public string GetSTCByMaMonHoc(string maMonHoc)
        {
            db = new GDUDataConnectionsDataContext();
            string stc;
            var soTC = from x in db.MonHocs where x.MaMonHoc == maMonHoc select x;
            stc = soTC.ToString();
            return stc;
        }

        public void UpdateMonHoc(MonHoc monHoc)
        {
           //code content
        }
    }
}
