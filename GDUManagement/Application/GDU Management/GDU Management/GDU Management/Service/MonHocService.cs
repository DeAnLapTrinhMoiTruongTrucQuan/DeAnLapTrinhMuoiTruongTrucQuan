using GDU_Management.DaoImpl;
using GDU_Management.IDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDU_Management.Model; 

namespace GDU_Management.Service
{
    class MonHocService
    {
        IDaoMonHoc monHocIDao = new MonHocImpl();

        //lấy tất cả môn học
        public List<MonHoc> GetAllMonHoc()
        {
            return monHocIDao.GetAllMonHoc();
        }
        //thêm mới một môn học
        public SinhVien CreateMonHoc(MonHoc monHoc)
        {
            return monHocIDao.CreateMonHoc(monHoc);
        }
        //xóa môn học
        public void DeleteMonHoc(string maMonHoc)
        {
            monHocIDao.DeleteMonHoc(maMonHoc);
        }
        //cập nhật môn học
        public void UpdateMonHoc(MonHoc monHoc)
        {
            monHocIDao.UpdateMonHoc(monHoc);
        }
    }
}
