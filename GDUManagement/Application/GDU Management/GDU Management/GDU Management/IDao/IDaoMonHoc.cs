using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDU_Management.Model;

namespace GDU_Management.IDao
{
    interface IDaoMonHoc
    {
        List<MonHoc> GetAllMonHoc();
        SinhVien CreateMonHoc(MonHoc monHoc);
        void DeleteMonHoc(string maMonHoc);
        void UpdateMonHoc(MonHoc monHoc);
    }
}
