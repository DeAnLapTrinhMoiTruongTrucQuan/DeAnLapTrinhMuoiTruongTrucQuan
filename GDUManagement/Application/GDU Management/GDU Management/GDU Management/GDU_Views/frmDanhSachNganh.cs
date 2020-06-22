using GDU_Management.IDao;
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
    public partial class frmDanhSachNganh : Form
    {
        public frmDanhSachNganh()
        {
            InitializeComponent();
        }
        //
        
        //khai báo các service 
        NganhHocService nganhHocService = new NganhHocService();  

        //Danh Sách Hàm PUBLIC

            //load danh sách ngành vào dgv
        public void LoadDanhSachNganh()
        {
            string maKhoa;
            maKhoa = lblMaKhoa.Text;
             dgvDSNganh.DataSource = nganhHocService.GetNganhHocByKHOA(maKhoa);
        }

        //load data lên textbox
        public void ShowDataTuDataGridViewToTextBox()
        {
            txtMaNganh.DataBindings.Clear();
            txtMaNganh.DataBindings.Add("text", dgvDSNganh.DataSource, "MaNganh");
            txtTenNganh.DataBindings.Clear();
            txtTenNganh.DataBindings.Add("text", dgvDSNganh.DataSource, "TenNganh");
        }

        //check data
        public bool checkDataNGANH()
        {
            if (string.IsNullOrEmpty(txtTenNganh.Text))
            {
                MessageBox.Show("Tên Ngành Không được bỏ trống, vui lòng kiểm tra lại..." , "Cảnh Báo" , MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNganh.Focus();
                return false;
            }
            return true;
        }

        //hàm nhận text maKhoa từ frmQLSV
        public void FunData(TextBox txtFrmDanhSachKhoa)
        {
            lblMaKhoa.Text = txtFrmDanhSachKhoa.Text;
        }

        //hàm tạo id tự động
        public void AutoIDNganh()
        {
            int count;

            string maKhoa = lblMaKhoa.Text;
            string lastID = maKhoa.Substring(8); //lay 2 so cuoi cua ma khoa

            count = dgvDSNganh.Rows.Count;

            if(count == 0)
            {
                txtMaNganh.Text = "M4716" + lastID + "000";
            }
            else
            {
                string chuoi_id = "";
                int chuoi_id_key = 0;

                chuoi_id = Convert.ToString(dgvDSNganh.Rows[count - 1].Cells[1].Value);
                chuoi_id_key = Convert.ToInt32(chuoi_id.Remove(0, 9));

                if (chuoi_id_key + 1 < 10)
                {
                    txtMaNganh.Text = "M4716" + lastID + "00" + (chuoi_id_key + 1).ToString();
                }
                else if (chuoi_id_key + 1 >= 10)
                {
                    txtMaNganh.Text = "M4716" + lastID + "0" + (chuoi_id_key + 1).ToString();
                }
            }
        }

        //-------------------------KẾT THÚC DS HÀM PUBLIC------------------------------//
        //--------------------------------------------------------------------------------------//



        private void pnDanhSachNganh_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDanhSachNganh_Load(object sender, EventArgs e)
        {
            LoadDanhSachNganh();
            ShowDataTuDataGridViewToTextBox();
        }

        private void dgvDSNganh_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ShowDataTuDataGridViewToTextBox();
            btnSaveNganh.Enabled = false;
            btnUpdateNganh.Enabled = true;
            btnDeleteNganh.Enabled = true;
        }

        private void btnSaveNganh_Click(object sender, EventArgs e)
        {
            if (checkDataNGANH())
            {
                NganhHoc nganhHoc = new NganhHoc();
                nganhHoc.MaNganh = txtMaNganh.Text;
                nganhHoc.TenNganh = txtTenNganh.Text;
                nganhHoc.MaKhoa = lblMaKhoa.Text;
                nganhHocService.CreateNganhHoc(nganhHoc);
                MessageBox.Show("Tạo Mới Thành Công...(^...^) ", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSaveNganh.Enabled = false;
                LoadDanhSachNganh();
            }
            else
            {
                MessageBox.Show("Lỗi, Thêm Thất Bại", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteNganh_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn Có Muốn Xóa '" + txtMaNganh.Text + "' ?", "THÔNG BÁO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string maNganh = txtMaNganh.Text.Trim();
                if (string.IsNullOrEmpty(txtMaNganh.Text))
                {
                    MessageBox.Show("Xóa Thất Bại", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    nganhHocService.DeleteNganhHoc(maNganh);
                    txtMaNganh.Text = "";
                    txtTenNganh.Text = ""; 
                    btnSaveNganh.Enabled = false;
                    btnUpdateNganh.Enabled = false;
                    btnDeleteNganh.Enabled = false;
                    LoadDanhSachNganh();
                    MessageBox.Show("Đã Xóa", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdateNganh_Click(object sender, EventArgs e)
        {
            NganhHoc nganhHoc = new NganhHoc();
            nganhHoc.MaNganh = txtMaNganh.Text;
            nganhHoc.TenNganh = txtTenNganh.Text;
            nganhHocService.UpdateNganhHoc(nganhHoc);
            MessageBox.Show("Cập nhật thông tin '" + txtMaNganh.Text + "' Thành Công", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadDanhSachNganh();
        }

        private void btnNewNganh_Click(object sender, EventArgs e)
        {
            AutoIDNganh();
            txtTenNganh.Text = "";
            btnSaveNganh.Enabled = true;
            btnUpdateNganh.Enabled = false;
            btnDeleteNganh.Enabled = false;
        }

        private void txtTimKiemNganh_MouseClick(object sender, MouseEventArgs e)
        {
            txtTimKiemNganh.Clear();
        }

        private void txtTimKiemNganh_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTimKiemNganh.Text))
            {
                LoadDanhSachNganh();
            }
            else
            {
                dgvDSNganh.DataSource = nganhHocService.SearchNganhHocByMaNganh(txtTimKiemNganh.Text.Trim()).ToList();
                dgvDSNganh.DataSource = nganhHocService.SearchNganhHocByTenNganh(txtTimKiemNganh.Text.Trim()).ToList();
            }
        }
    }
}
