using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDU_Management.IDao;
using GDU_Management.Model;


namespace GDU_Management.DaoImpl
{
    class AdminImpl : IDaoAdmin
    {
        //tao ket noi database
        GDUDataConnectionsDataContext db = new GDUDataConnectionsDataContext();
        List<Admin> ad;
        public Admin CreateAdmin(Admin ad)
        {
            //code contnt
            return null;
        }

        public void DeleteAdmin(string maAD)
        {
           //code content
        }

        public List<Admin> GetAllAdmin()
        {
            //code content
            return null;
        }

        public void UpdateAdmin(Admin ad)
        {
            //code content
        }
    }
}
