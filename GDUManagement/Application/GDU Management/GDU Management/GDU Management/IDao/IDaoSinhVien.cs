using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDU_Management.IDao;
using GDU_Management.Model;

namespace GDU_Management.IDao
{
    interface IDaoSinhVien
    {
        List<SinhVien> GetAllSinhVien();
        SinhVien CreateSinhVien(SinhVien sinhVien);
        void DeleteSinhVien(string MaSV);
        void UpdateSinhVien(SinhVien sinhVien);
    }
}
