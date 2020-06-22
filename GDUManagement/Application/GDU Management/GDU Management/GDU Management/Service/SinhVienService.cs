using GDU_Management.DaoImpl;
using GDU_Management.IDao;
using GDU_Management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDU_Management.Service
{
    class SinhVienService
    {
        IDaoSinhVien sinhVienIDao = new SinhVienImpl();

        //lấy tất cả sinh viên
        public List<SinhVien> GetAllSinhVien()
        {
            return sinhVienIDao.GetAllSinhVien();
        }

        //thêm mới một sinh viên
        public SinhVien CreateSinhVien(SinhVien sinhVien)
        {
            return sinhVienIDao.CreateSinhVien(sinhVien);
        }

        //xóa sinh viên
        public void DeleteSinhVien(string maSV)
        {
            sinhVienIDao.DeleteSinhVien(maSV);
        }

        //cập nhật sinh viên
        public void UpdateSinhVien(SinhVien sinhVien)
        {
            sinhVienIDao.UpdateSinhVien(sinhVien);
        }

        //lấy danh sách sinh viên theo mã lớp
        public List<SinhVien> GetSinhVienByMaLop(string maLop)
        {
            return sinhVienIDao.GetSinhVienByMaLop(maLop);
        }
    }
}
