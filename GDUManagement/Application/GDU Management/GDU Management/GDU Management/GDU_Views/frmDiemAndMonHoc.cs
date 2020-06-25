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
    public partial class frmDiemAndMonHoc : Form
    {
        public frmDiemAndMonHoc()
        {
            InitializeComponent();
            NgayGio();
        }
        //Khai Báo Các Service
        MonHocService monHocService = new MonHocService();
        DiemMonHocService diemMonHocService = new DiemMonHocService();
        KhoaService khoaService = new KhoaService();
        NganhHocService nganhHocService = new NganhHocService();
        KhoaHocService khoaHocService = new KhoaHocService();
        LopService lopService = new LopService();

        //---------------------------DANH SÁCH HÀM PUBLIC------------------------------//
        //---------------------------------------------------------------------------------------//


        //1-Hàm lấy thời gian
        public void NgayGio()
        {
            //get ngày
            DateTime ngay = DateTime.Now;
            lblDay.Text = ngay.ToString("dd/MM/yyyy");

            //get thời gian
            timerMonHoc.Start();
        }

        //2-Hàm Quay Trở Lại Menu GDUmanagement
        public void GoToGDUmanagement()
        {
            this.Hide();
            GDUManagement gdu = new GDUManagement();
            gdu.ShowDialog();
        }

        public void LoadDataToCombox()
        {
            //tab mon hoc
            cboChonKhoa_MH.DataSource = khoaService.GetAllKhoa();
            cboChonKhoa_MH.DisplayMember = "TenKhoa";
            cboChonKhoa_MH.ValueMember = "MaKhoa";

            //tab điểm
            cboChonKhoa_QLD.DataSource = khoaService.GetAllKhoa();
            cboChonKhoa_QLD.DisplayMember = "TenKhoa";
            cboChonKhoa_QLD.ValueMember = "MaKhoa";
        }

        //laod danh sách môn học vào datagridview
        public void LoadDanhSachMonHocToDatagridview()
        {
            string mmh = cboChonNganh_MH.SelectedValue.ToString();
            dgvDanhSachMonHoc.DataSource = monHocService.GetMonHocByNganh(mmh);
        }

        //laod danh sách điểm vào dgv theo mã lopwx và mã môn học
        public void LoadDanhSachDiemSinhVienToDatagridview()
        {
            string maLop = cboChonLop_QLD.SelectedValue.ToString();
            string maMonHoc = cboChonMon_QLD.SelectedValue.ToString();
            dgvDanhSachDiemSinhVien.DataSource = diemMonHocService.GetDanhSachMonByMaLopAndMaMonHoc(maLop, maMonHoc);
        }

        //show data môn học to textbox
        public void ShowDataMonHocTuGDgvToTextBox()
        {
            int countRows = dgvDanhSachMonHoc.Rows.Count;
            if (countRows == 0)
            {
                dgvDanhSachMonHoc.Enabled = false;
            }
            else
            {
                txtMaMon_MH.DataBindings.Clear();
                txtMaMon_MH.DataBindings.Add("text", dgvDanhSachMonHoc.DataSource, "MaMonHoc");
                txtTenMon_MH.DataBindings.Clear();
                txtTenMon_MH.DataBindings.Add("text", dgvDanhSachMonHoc.DataSource, "TenMonHoc");
                numericSoTinChi_MH.DataBindings.Clear();
                string STC = dgvDanhSachMonHoc.CurrentRow.Cells[3].Value.ToString();
                numericSoTinChi_MH.Value = Convert.ToInt32(STC);
            }
        }

        //check data Môn Học
        public bool checkDataMonHoc()
        {
            if (string.IsNullOrEmpty(txtTenMon_MH.Text))
            {
                MessageBox.Show("Tên Môn Không được bỏ trống, vui lòng kiểm tra lại...", "CảnhBáo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenMon_MH.Focus();
                return false;
            }

            int soTC = Convert.ToInt32(numericSoTinChi_MH.Value.ToString());
            if (soTC <= 0)
            {
                MessageBox.Show("STC không được nhỏ hơn hoặc bằng 0, vui lòng kiểm tra lại...", "CảnhBáo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        public void XetDiem()
        {

        }

        //-------------------------KẾT THÚC DS HÀM PUBLIC------------------------------//
        //--------------------------------------------------------------------------------------//

        private void timerMonHoc_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();

            int giay = Convert.ToInt32(lblGiay.Text);
            int phut = Convert.ToInt32(lblPhut.Text);
            giay++;
            if (giay > 59)
            {
                giay = 0;
                phut++;
            }

            if (giay < 10)
            {
                lblGiay.Text = "0" + giay;
            }
            else
            {
                lblGiay.Text = "" + giay;
            }
            if (phut < 10)
            {
                lblPhut.Text = "0" + phut;
            }
            else
            {
                lblPhut.Text = "" + phut;
            }

            if (giay % 2 == 0)
            {
                lblAnimation1.Text = "(^_^)";
                lblAnimation2.Text = "(+_+)";
                lblAnimation3.Text = "(-_^)";
            }
            else if (giay % 2 != 0)
            {
                lblAnimation2.Text = "(^_^)";
                lblAnimation1.Text = "(+_+)";
                lblAnimation3.Text = "(^_-)";
            }
            else
            {
                lblAnimation1.Text = ".";
                lblAnimation1.Text = "..";
                lblAnimation1.Text = "...";
                lblAnimation2.Text = ".";
                lblAnimation2.Text = "..";
                lblAnimation2.Text = "...";
            }
        }

        private void btnHomeMenu_Click(object sender, EventArgs e)
        {
            GoToGDUmanagement();
        }

        private void btnHomQLD_Click(object sender, EventArgs e)
        {
            GoToGDUmanagement();
        }

        private void frmDiemAndMonHoc_Load(object sender, EventArgs e)
        {
            LoadDataToCombox();
        }

        private void cboChonKhoa_MH_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboChonNganh_MH.Text = "";
            string maKhoa = cboChonKhoa_MH.SelectedValue.ToString();
            cboChonNganh_MH.DataSource = nganhHocService.GetNganhHocByKHOA(maKhoa);
            cboChonNganh_MH.DisplayMember = "TenNganh";
            cboChonNganh_MH.ValueMember = "MaNganh";
        }

        private void cboChonNganh_MH_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDanhSachMonHocToDatagridview();
            ShowDataMonHocTuGDgvToTextBox();
        }

        private void dgvDanhSachMonHoc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDataMonHocTuGDgvToTextBox();
        }

        private void dgvDanhSachMonHoc_MouseClick(object sender, MouseEventArgs e)
        {
            ShowDataMonHocTuGDgvToTextBox();
        }

        private void btnNewMonHoc_Click(object sender, EventArgs e)
        {
            dgvDanhSachMonHoc.Enabled = true;
            btnSaveMonHoc.Enabled = true;
            btnDeleteMonHoc.Enabled = false;
            btnUpdateMonHoc.Enabled = false;
            LoadDanhSachMonHocToDatagridview();
            txtMaMon_MH.Text = "";
            txtTenMon_MH.Text = "";
            txtTenMon_MH.Focus();
        }

        private void btnSaveMonHoc_Click(object sender, EventArgs e)
        {
            if (checkDataMonHoc())
            {
                int soTC = Convert.ToInt32(numericSoTinChi_MH.Value.ToString());
                string mmh = cboChonNganh_MH.SelectedValue.ToString();

                MonHoc monHoc = new MonHoc();
                monHoc.MaMonHoc = txtMaMon_MH.Text.Trim();
                monHoc.TenMonHoc = txtTenMon_MH.Text.Trim();
                monHoc.STC = soTC;
                monHoc.MaNganh = mmh;

                monHocService.CreateMonHoc(monHoc);

                LoadDanhSachMonHocToDatagridview();
                MessageBox.Show("Đã Thêm [" + txtMaMon_MH.Text + "]", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSaveMonHoc.Enabled = false;
                btnDeleteMonHoc.Enabled = true;
                btnUpdateMonHoc.Enabled = true;
            }
            else
            {
                MessageBox.Show("Lỗi, Thêm Thất Bại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateMonHoc_Click(object sender, EventArgs e)
        {
            if (checkDataMonHoc())
            {
                int soTC = Convert.ToInt32(numericSoTinChi_MH.Value.ToString());

                MonHoc monHoc = new MonHoc();
                monHoc.MaMonHoc = txtMaMon_MH.Text.Trim();
                monHoc.TenMonHoc = txtTenMon_MH.Text.Trim();
                monHoc.STC = soTC;

                monHocService.UpdateMonHoc(monHoc);
                LoadDanhSachMonHocToDatagridview();

                MessageBox.Show("Cập nhật thông tin [" + txtMaMon_MH.Text + "] thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSaveMonHoc.Enabled = false;
                btnDeleteMonHoc.Enabled = true;
                btnUpdateMonHoc.Enabled = true;
            }
            else
            {
                MessageBox.Show("Lỗi, Cập nhật thất bại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnDeleteMonHoc_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn Có Muốn Xóa [" + txtMaMon_MH.Text + "] ?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string maMon;
                maMon = txtMaMon_MH.Text.Trim();
                if (string.IsNullOrEmpty(txtMaMon_MH.Text))
                {
                    MessageBox.Show("Xóa Thất Bại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    monHocService.DeleteMonHoc(maMon);
                    LoadDanhSachMonHocToDatagridview();
                    MessageBox.Show("Đã Xóa [" + txtMaMon_MH.Text + "]", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtMaMon_MH.Text = "null";
                    txtTenMon_MH.Text = "";
                    numericSoTinChi_MH.Value = 0;

                    btnNewMonHoc.Enabled = true;
                    btnSaveMonHoc.Enabled = false;
                    btnUpdateMonHoc.Enabled = false;
                    btnDeleteMonHoc.Enabled = true;
                }
            }
        }

        private void cboChonKhoa_QLD_SelectedIndexChanged(object sender, EventArgs e)
        {
            string maKhoa = cboChonKhoa_QLD.SelectedValue.ToString();
            cboChonNganh_QLD.DataSource = nganhHocService.GetNganhHocByKHOA(maKhoa);
            cboChonNganh_QLD.DisplayMember = "TenNganh";
            cboChonNganh_QLD.ValueMember = "MaNganh";
        }

        private void cboChonNganh_QLD_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboChonKhoasHoc_QLD.DataSource = khoaHocService.GetAllKhoaHoc();
            cboChonKhoasHoc_QLD.DisplayMember = "TenKhoaHoc";
            cboChonKhoasHoc_QLD.ValueMember = "MaKhoaHoc";
        }

        private void cboChonKhoasHoc_QLD_SelectedIndexChanged(object sender, EventArgs e)
        {
            string maNganh = cboChonNganh_QLD.SelectedValue.ToString();
            string maKhoasHoc = cboChonKhoasHoc_QLD.SelectedValue.ToString();

            cboChonLop_QLD.DataSource = lopService.GetDanhSachLopByMaNganhVaMaKhoaHoc(maNganh, maKhoasHoc);
            cboChonLop_QLD.DisplayMember = "TenLop";
            cboChonLop_QLD.ValueMember = "MaLop";
        }

        private void cboChonLop_QLD_SelectedIndexChanged(object sender, EventArgs e)
        {
            string maNganh = cboChonNganh_QLD.SelectedValue.ToString();
            cboChonMon_QLD.DataSource = monHocService.GetMonHocByNganh(maNganh);
            cboChonMon_QLD.DisplayMember = "TenMonHoc";
            cboChonMon_QLD.ValueMember = "MaMonHoc";
        }

        private void cboChonMon_QLD_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDanhSachDiemSinhVienToDatagridview();
            int countRows = dgvDanhSachDiemSinhVien.Rows.Count;
            if (countRows == 0)
            {
                dgvDanhSachDiemSinhVien.Enabled = false;
            }
            else
            {
                dgvDanhSachDiemSinhVien.Enabled = true;
            }
        }

        private void dgvDanhSachDiemSinhVien_MouseClick(object sender, MouseEventArgs e)
        {
            string D70L2 = dgvDanhSachDiemSinhVien.CurrentRow.Cells[5].Value.ToString();
            string D70L1 = dgvDanhSachDiemSinhVien.CurrentRow.Cells[4].Value.ToString();
            string D30 = dgvDanhSachDiemSinhVien.CurrentRow.Cells[3].Value.ToString();

            lblMaSV.DataBindings.Clear();
            lblMaSV.DataBindings.Add("text", dgvDanhSachDiemSinhVien.DataSource, "MaSV");
            txtDiem70L2.Text = D70L2;
            txtDiem70L1.Text = D70L1;
            txtDiem30.Text = D30;
        }

        private void btnSaveDiem_Click(object sender, EventArgs e)
        {

            double  dtb;
            string xepLoai;
            if (txtDiem70L2.Text != "null")
            {
                float D30 = (float)Convert.ToDouble(txtDiem30.Text);
                float D70L1 = (float)Convert.ToDouble(txtDiem70L1.Text);
                float D70L2 = (float)Convert.ToDouble(txtDiem70L2.Text);

                double  dtb1 = (D30 * 30 / 100) + (D70L2 * 70 / 100);
                dtb = Math.Round(dtb1, 2);
                MessageBox.Show("dtb " + dtb);
            }
            else
            {
                float D30 = (float)Convert.ToDouble(txtDiem30.Text);
                float D70L1 = (float)Convert.ToDouble(txtDiem70L1.Text);
                //float D70L2 = (float)Convert.ToDouble(txtDiem70L2.Text);

                double dtb1 = (D30 * 30 / 100) + (D70L1 * 70 / 100);
                dtb =Math.Round(dtb1, 2);
                MessageBox.Show("dtb " + dtb);
            }

            if (dtb >= 8.5)
            {
                xepLoai = "A";
            }
            else if (dtb >= 8)
            {
                xepLoai = "B+";
            }
            else if (dtb >= 7)
            {
                xepLoai = "B";
            }
            else if (dtb >= 6.5)
            {
                xepLoai = "C+";
            }
            else if (dtb >= 5.5)
            {
                xepLoai = "C";
            }
            else if (dtb >= 5.0)
            {
                xepLoai = "D+";
            }
            else if (dtb >= 4.0)
            {
                xepLoai = "D";
            }
            else 
            {
                xepLoai = "F";
            }

            DiemMonHoc dmh = new DiemMonHoc();
            dmh.MaSV = lblMaSV.Text;
            dmh.MaMonHoc = cboChonMon_QLD.SelectedValue.ToString();
            dmh.Diem30 = (float)Convert.ToDouble(txtDiem30.Text);
            dmh.Diem70L1 = (float)Convert.ToDouble(txtDiem70L1.Text);
            dmh.Diem70L2 = txtDiem70L2.Text;
            dmh.DTB = (float)dtb;
            dmh.DiemChu = xepLoai;
            dmh.GhiChu = rtxtGhiChu.Text;
            diemMonHocService.UpdateDiemMonHoc(dmh);
            LoadDanhSachDiemSinhVienToDatagridview();
            MessageBox.Show("Cập nhật điểm thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
    }
}
