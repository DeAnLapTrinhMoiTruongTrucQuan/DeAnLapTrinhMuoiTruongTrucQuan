using GDU_Management.IDao;
using GDU_Management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDU_Management.DaoImpl
{
    class DiemMonHocImpl : IDaoDiemMonHoc
    {
        //tao ket noi database
        GDUDataConnectionsDataContext db = new GDUDataConnectionsDataContext();
        List<DiemMonHoc> listDiemMonHocs;
        public DiemMonHoc AddDiemMonHoc(DiemMonHoc diemMonHoc)
        {
            //code content
            return null;
        }

        public void DeleteDiemMonHoc( string MaSV, string maMonHoc)
        {
            //code content
        }

        public List<DiemMonHoc> GetAllDiemMonHoc()
        {
            //code content
            return null;
        }

        public List<DiemMonHoc> GetDanhSachMonByMaLopAndMaMonHoc(string maLop, string maMonHoc)
        {
            db = new GDUDataConnectionsDataContext();
            var listDiem = from dmh in db.DiemMonHocs
                           join sv in db.SinhViens on dmh.MaSV equals sv.MaSV
                           join lp in db.Lops on sv.MaLop equals lp.MaLop
                           join mh in db.MonHocs on dmh.MaMonHoc equals mh.MaMonHoc
                           where dmh.MaMonHoc == maMonHoc && lp.MaLop == maLop
                           select dmh;
            listDiemMonHocs = listDiem.ToList();
            return listDiemMonHocs;
        }

        public void UpdateDiemMonHoc(DiemMonHoc diemMonHoc)
        {
            db = new GDUDataConnectionsDataContext();
            DiemMonHoc dmh = new DiemMonHoc();
            dmh = db.DiemMonHocs.Single(p => p.MaMonHoc == diemMonHoc.MaMonHoc && p.MaSV == diemMonHoc.MaSV);
            dmh.Diem30 = diemMonHoc.Diem30;
            dmh.Diem70L1 = diemMonHoc.Diem70L1;
            dmh.Diem70L2 = diemMonHoc.Diem70L2;
            dmh.DTB = diemMonHoc.DTB;
            dmh.DiemChu = diemMonHoc.DiemChu;
            dmh.GhiChu = diemMonHoc.GhiChu;
            db.SubmitChanges();
        }
    }
}
