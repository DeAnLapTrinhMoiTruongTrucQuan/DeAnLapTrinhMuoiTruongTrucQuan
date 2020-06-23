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
        List<SinhVien> sinhVien;

        public SinhVien CreateSinhVien(SinhVien sinhVien)
        {
            //code content
            return null;
        }

        public void DeleteSinhVien(string MaSV)
        {
            //code content
        }
        public List<SinhVien> GetAllSinhVien()
        {
            var sv = from x in db.SinhViens select x;
            sinhVien = sv.ToList();
            return sinhVien;
        }

        public void UpdateSinhVien(SinhVien sinhVien)
        {
            //code content
        }
    }
}
