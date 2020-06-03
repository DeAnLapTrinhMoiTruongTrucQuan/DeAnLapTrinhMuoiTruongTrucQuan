using GDU_Management.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDU_Management
{
    public partial class frmDanhSachLop : Form
    {
        public frmDanhSachLop()
        {
            InitializeComponent();
        }

        //khai báo các service 
        LopService lopService = new LopService();

        //---------------HÀM PUBLIC-------------------

            // hàm nhận mã ngành và khóa học từ frmQLSV
        public void FunDatafrmDanhSachLopToFrmQLSV(string txtMaNganh, string txtMaKhoaHoc)
        {
            lblMaNganh.Text = txtMaNganh;
            lblKhoa.Text = txtMaKhoaHoc;
        }

        //hàm load data từ datagridview lên textbox
        public void ShowdataTuDatagridviewToTextbox()
        {
            txtMaLop.Clear();
            txtMaLop.DataBindings.Add("text",dgvDanhSachLop.DataSource, "MaLop");
            txtTenLop.Clear();
            txtTenLop.DataBindings.Add("text",dgvDanhSachLop.DataSource,"TenLop");
        }


        //show danh sách lớp theo ngành và khóa học
        public void LoadDanhSachLopToDatagridview()
        {
            string maKhoaHoc = lblKhoa.Text;
            string maNganh = lblMaNganh.Text;
            dgvDanhSachLop.DataSource = lopService.GetDanhSachLopByMaNganhVaMaKhoaHoc(maNganh,maKhoaHoc).ToList();
        }

        //----------KẾT THÚC HÀM PUPLIC-----------
        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDanhSachLop_Load(object sender, EventArgs e)
        {
            LoadDanhSachLopToDatagridview();
            ShowdataTuDatagridviewToTextbox();
        }
    }
}
