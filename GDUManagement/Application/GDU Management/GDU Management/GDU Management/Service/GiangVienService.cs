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
    class GiangVienService
    {
        IDaoGiangVien giangVienIDao = new GiangVienImpl();

        //lấy tất cả giảng viên
        public List<GiangVien> GetAllGiangVien()
        {
            return giangVienIDao.GetAllGiangVien();
        }

        //thêm mới một giảng viên
        public GiangVien CreateGiangVien(GiangVien giangVien)
        {
            return giangVienIDao.CreateGiangVien(giangVien);
        }

        //xóa giảng viên
        public void DeleteGiangVien(string maGV)
        {
            giangVienIDao.DeleteGiangVien(maGV);
        }

        //cập nhật giảng viên
        public void UpdateGiangVien(GiangVien giangVien)
        {
            giangVienIDao.UpdateGiangVien(giangVien);
        }

        //Search 
        public List<GiangVien> SearchGiangVienByTenGV(string tenGiangVien)
        {
            return giangVienIDao.SearchGiangVienByTenGV(tenGiangVien);
        }

        
    }
}
