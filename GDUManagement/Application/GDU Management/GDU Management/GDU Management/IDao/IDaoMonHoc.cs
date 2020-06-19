using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDU_Management.DaoImpl;
using GDU_Management.Model;

namespace GDU_Management.IDao
{
    interface IDaoMonHoc
    {
        List<MonHoc> GetAllMonHoc();
        SinhVien CreateMonHoc(MonHoc monHoc);
        void DeleteMonHoc(string maMonHoc);
        void UpdateMonHoc(MonHoc monHoc);
        //List<MaMonHoc> GetMaMHByTenMH(string maMH);
        List<MonHoc> GetMonHocByMaNganh(string maNganh);
        string GetSTCByMaMonHoc(string maMonHoc);
    }
}
