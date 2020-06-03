using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDU_Management.Model;

namespace GDU_Management.IDao
{
    interface IDaoKhoa
    {
        List<Khoa> GetAllKhoa();
        Khoa CreateKhoa(Khoa khoa);
        void DeleteKhoa(string maKhoa);
        void UpdateKhoa(Khoa khoa);
        List<Khoa> GetKhoaByMaKhoa(string maKhoa);

        List<Khoa> TimKiemKhoaByTenKhoa(string tenKhoa);

    }
}
