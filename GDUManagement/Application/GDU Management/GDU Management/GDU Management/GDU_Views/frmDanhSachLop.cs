using GDU_Management.Model;
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

        //---------------HÀM PUBLIC-------------------//

            // hàm nhận mã ngành và khóa học từ frmQLSV
        public void FunDatafrmDanhSachLopToFrmQLSV(string txtMaNganh, string txtMaKhoaHoc)
        {
            lblMaNganh.Text = txtMaNganh;
            lblKhoaHoc.Text = txtMaKhoaHoc;
        }

        //hàm check data
        public bool checkDataLOP()
        {
            if (string.IsNullOrEmpty(txtMaLop.Text))
            {
                txtMaLop.Focus();
                MessageBox.Show("Mã Lớp Không được bỏ trống, vui lòng kiểm tra lại...", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrEmpty(txtTenLop.Text))
            {
                txtTenLop.Focus();
                MessageBox.Show("Tên Lớp Không được bỏ trống, vui lòng kiểm tra lại...", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        //hàm load data từ datagridview lên textbox
        public void ShowdataTuDatagridviewToTextbox()
        {
            txtMaLop.DataBindings.Clear();
            txtMaLop.DataBindings.Add("text", dgvDanhSachLop.DataSource, "MaLop");
            txtTenLop.DataBindings.Clear();
            txtTenLop.DataBindings.Add("text", dgvDanhSachLop.DataSource, "TenLop");
        }

        //show danh sách lớp theo ngành và khóa học
        public void LoadDanhSachLopToDatagridview()
        {
            string maKhoaHoc = lblKhoaHoc.Text;
            string maNganh = lblMaNganh.Text;
            dgvDanhSachLop.DataSource = lopService.GetDanhSachLopByMaNganhVaMaKhoaHoc(maNganh, maKhoaHoc).ToList();
        }

        //----------KẾT THÚC HÀM PUPLIC-----------
        
        private void frmDanhSachLop_Load(object sender, EventArgs e)
        {
            LoadDanhSachLopToDatagridview();
            ShowdataTuDatagridviewToTextbox();
        }

        private void btnSaveLop_Click(object sender, EventArgs e)
        {
            if (checkDataLOP())
            {
                Lop lop = new Lop();
                lop.MaLop = txtMaLop.Text.Trim();
                lop.TenLop = txtTenLop.Text.Trim();
                lop.MaNganh = lblMaNganh.Text.Trim();
                lop.MaKhoaHoc = lblKhoaHoc.Text.Trim();
                lopService.CreateLop(lop);
                LoadDanhSachLopToDatagridview();
                txtMaLop.Text = "";
                txtTenLop.Text = "";
                btnSaveLop.Enabled = false;
                MessageBox.Show("Thêm Mới Thành Công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Thêm Mới Thất Bại, vui lòng kiểm tra lại...", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNewLop_Click(object sender, EventArgs e)
        {
            txtMaLop.Clear();
            txtTenLop.Clear();
        }

        private void btnUpdateLop_Click(object sender, EventArgs e)
        {
            if (checkDataLOP())
            {
                Lop lp = new Lop();
                lp.MaLop = txtMaLop.Text.Trim();
                lp.TenLop = txtTenLop.Text.Trim();
                lp.MaNganh = lblMaNganh.Text.Trim();
                lp.MaKhoaHoc = lblKhoaHoc.Text.Trim();
                lopService.UpdateLop(lp);
                LoadDanhSachLopToDatagridview();
                txtMaLop.Text = "";
                txtTenLop.Text = "";
                btnUpdateLop.Enabled = false;
                btnSaveLop.Enabled = false;
                btnDeleteLop.Enabled = false;
                MessageBox.Show("Cập nhật thông tin '" + txtMaLop.Text + "' thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Cập Nhật thông tin Thất Bại, vui lòng kiểm tra lại...", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExitDSLop_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvDanhSachLop_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            btnUpdateLop.Enabled = true;
            btnSaveLop.Enabled = false;
           btnDeleteLop.Enabled = true;
            ShowdataTuDatagridviewToTextbox();
        }

        private void btnDeleteLop_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn Có Muốn Xóa  '" + txtMaLop.Text + "'", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string maLop = txtMaLop.Text.Trim();
                if (string.IsNullOrEmpty(txtMaLop.Text))
                {
                    MessageBox.Show("Xóa Thất Bại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    lopService.DeleteLop(maLop);
                    LoadDanhSachLopToDatagridview();
                    MessageBox.Show("Xóa Thành Công...", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtMaLop.Clear();
                    txtTenLop.Clear();
                    btnDeleteLop.Enabled = false;
                    btnSaveLop.Enabled = false;
                    btnUpdateLop.Enabled = false;
                }
            }
        }

        private void txtTenLop_TextChanged(object sender, EventArgs e)
        {
            btnSaveLop.Enabled = true;
        }

        private void txtTimKiemLop_MouseClick(object sender, MouseEventArgs e)
        {
            txtTimKiemLop.Clear();
        }

        private void txtTimKiemLop_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTimKiemLop.Text))
            {
                LoadDanhSachLopToDatagridview();
            }
            else
            {
                dgvDanhSachLop.DataSource = lopService.SearchLopHocByTenLop(txtTimKiemLop.Text.Trim());
            }
        }
    }
}
