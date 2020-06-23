using GDU_Management.IDao;
using GDU_Management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDU_Management.DaoImpl
{
    class ThoiKhoaBieuImpl : IDaoThoiKhoaBieu
    {
        GDUDataConnectionsDataContext db = new GDUDataConnectionsDataContext();
        List<ThoiKhoaBieu> thoiKhoaBieu;

        public ThoiKhoaBieu CreateThoiKhoaBieu(ThoiKhoaBieu thoiKhoaBieu)
        {

            db = new GDUDataConnectionsDataContext();
            ThoiKhoaBieu tkb = new ThoiKhoaBieu();
            tkb = thoiKhoaBieu;
            db.ThoiKhoaBieus.InsertOnSubmit(thoiKhoaBieu);
            db.SubmitChanges();
            return thoiKhoaBieu;

        }

        public List<ThoiKhoaBieu> GetAllTKB()
        {
        
            db = new GDUDataConnectionsDataContext();
            var tkb = from x in db.ThoiKhoaBieus select x;
            thoiKhoaBieu = tkb.ToList();
            return thoiKhoaBieu;

        }
    }
}
