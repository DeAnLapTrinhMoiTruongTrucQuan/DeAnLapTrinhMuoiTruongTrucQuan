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

        //------------------------------------HÀM PUBLIC------------------------------------------//
        //---------------------------------------------------------------------------------------------//

        // hàm nhận mã ngành và khóa học từ frmQLSV
        public void FunDatafrmDanhSachLopToFrmQLSV(string txtMaNganh, string txtMaKhoaHoc)
        {
            lblMaNganh.Text = txtMaNganh;
            lblMaKhoasHoc.Text = txtMaKhoaHoc;
        }

        //hàm check data
        public bool checkDataLOP()
        {
            if (string.IsNullOrEmpty(lblMaLop.Text))
            {
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
            lblMaLop.DataBindings.Clear();
            lblMaLop.DataBindings.Add("text", dgvDanhSachLop.DataSource, "MaLop");
            txtTenLop.DataBindings.Clear();
            txtTenLop.DataBindings.Add("text", dgvDanhSachLop.DataSource, "TenLop");
        }

        //show danh sách lớp theo ngành và khóa học
        public void LoadDanhSachLopToDatagridview()
        {
            string maKhoaHoc = lblMaKhoasHoc.Text;
            string maNganh = lblMaNganh.Text;
            dgvDanhSachLop.DataSource = lopService.GetDanhSachLopByMaNganhVaMaKhoaHoc(maNganh, maKhoaHoc).ToList();
        }

        //hàm auto id lớp
        public void AutoIDLop()
        {
            int count;
            count = dgvDanhSachLop.Rows.Count;

            string IdKhoas = lblMaKhoasHoc.Text;
            string LastIdKhoas = IdKhoas.Substring(1);
            //MessageBox.Show(LastIdKhoas);

            string IdNganh = lblMaNganh.Text;
            string LastIDNganh = IdNganh.Substring(8);
            //MessageBox.Show(LastIDNganh);


            if (count == 0)
            {
                lblMaLop.Text = "GDU" + LastIdKhoas + LastIDNganh + "00";
            }
            else
            {
                string chuoi_id = "";
                int chuoi_id_key = 0;

                chuoi_id = Convert.ToString(dgvDanhSachLop.Rows[count - 1].Cells[1].Value);
                chuoi_id_key = Convert.ToInt32(chuoi_id.Remove(0, 7));
                if (chuoi_id_key + 1 < 10)
                {
                    lblMaLop.Text = "GDU" + LastIdKhoas + LastIDNganh + "0" + (chuoi_id_key + 1);
                }
                else if (chuoi_id_key + 1 >= 10)
                {
                    lblMaLop.Text = "GDU" + LastIdKhoas + LastIDNganh + (chuoi_id_key + 1);
                }
            }
        }

        //-----------------------------------------KẾT THÚC HÀM PUPLIC--------------------------------//
        //----------------------------------------------------------------------------------------------------//
        
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
                lop.MaLop = lblMaLop.Text.Trim();
                lop.TenLop = txtTenLop.Text.Trim();
                lop.MaNganh = lblMaNganh.Text.Trim();
                lop.MaKhoaHoc = lblMaKhoasHoc.Text.Trim();
                lopService.CreateLop(lop);
                LoadDanhSachLopToDatagridview();
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
            AutoIDLop();
            txtTenLop.Clear();
            txtTenLop.Focus();
        }

        private void btnUpdateLop_Click(object sender, EventArgs e)
        {
            if (checkDataLOP())
            {
                Lop lp = new Lop();
                lp.MaLop = lblMaLop.Text.Trim();
                lp.TenLop = txtTenLop.Text.Trim();
                lp.MaNganh = lblMaNganh.Text.Trim();
                lp.MaKhoaHoc = lblMaKhoasHoc.Text.Trim();
                lopService.UpdateLop(lp);
                LoadDanhSachLopToDatagridview();
                btnUpdateLop.Enabled = false;
                btnSaveLop.Enabled = false;
                btnDeleteLop.Enabled = false;
                MessageBox.Show("Cập nhật thông tin  ["+lblMaLop.Text+"]' thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (MessageBox.Show("Xóa [" + lblMaLop.Text + "], Việc này sẽ xóa tất cả thông tin liên quan đến lớp bao gồm danh sách sinh viên trong lớp)", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string maLop = lblMaLop.Text.Trim();
                if (maLop.Equals("null"))
                {
                    MessageBox.Show("Xóa thất bại, Không tồn tại mã lớp [--"+maLop+"--]", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    SinhVienService sinhVienService = new SinhVienService();
                    sinhVienService.DeleteAllSinhVienByMaLop(maLop);
                    lopService.DeleteLop(maLop);
                    LoadDanhSachLopToDatagridview();
                    MessageBox.Show("Đã Xóa [" + lblMaLop.Text + "]", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblMaLop.Text="null";
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
