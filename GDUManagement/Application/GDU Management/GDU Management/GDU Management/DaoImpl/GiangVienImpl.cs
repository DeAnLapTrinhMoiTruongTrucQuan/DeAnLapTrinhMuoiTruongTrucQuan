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
            //code content
            return null;
        }

        public void DeleteGiangVien(string maGV)
        {
           //code content
        }

        public List<GiangVien> GetAllGiangVien()
        {
            //code content
            return null;
        }

        public void UpdateGiangVien(GiangVien giangVien)
        {
            //code content
        }
    }
}
