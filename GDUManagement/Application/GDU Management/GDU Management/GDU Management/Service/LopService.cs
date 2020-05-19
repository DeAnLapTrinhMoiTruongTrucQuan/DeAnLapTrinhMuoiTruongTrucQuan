﻿using GDU_Management.DaoImpl;
using GDU_Management.IDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDU_Management.Model;

namespace GDU_Management.Service
{
    class LopService
    {
        IDaoLop lopIDao = new LopImpl();

        //lấy danh sách lớp
        public List<Lop> GetAllLop()
        {
            return lopIDao.getAllLop();
        }
        //tạo lớp
        public Lop CreateKhoa(Lop lop)
        {
            return lopIDao.CreateLop(lop);
        }
        //xóa lớp
        public void DeleteLop(string maLop)
        {
            lopIDao.DeleteLop(maLop);
        }
        //cập nhật lớp
        public void UpdateLop(Lop lop)
        {
            lopIDao.UpdateLop(lop);
        }

    }
}
