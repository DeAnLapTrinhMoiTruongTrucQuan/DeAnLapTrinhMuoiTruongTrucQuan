using GDU_Management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDU_Management.IDao
{
    interface IDaoNganhHoc
    {
        List<NganhHoc> GetAllNganhhoc();
        NganhHoc CreateNganhHoc(NganhHoc nganhHoc);
        void DeleteNganhHoc(string maNganhHoc);
        void UpdateNganhHoc(NganhHoc nganhHoc);
        List<NganhHoc> GetNganhHocByKHOA(string maKhoa);
       
    }
}
