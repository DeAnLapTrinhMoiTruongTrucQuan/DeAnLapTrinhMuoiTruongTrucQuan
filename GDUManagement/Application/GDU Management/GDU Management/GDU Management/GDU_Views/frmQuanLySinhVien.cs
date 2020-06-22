
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
    public partial class frmQuanLySinhVien : Form
    {
        public frmQuanLySinhVien()
        {
            InitializeComponent();
            NgayGio();
            EnableFalseButton();
        }

        //các delegate dùng để truyền id qua các form con
        delegate void SendMaKhoaToFrmDanhSachKhoa(TextBox dlgtxtMaKhoa);
        delegate void SendMaKhoaHocMaNganhToFrmDanhSachLop(string dlgtMaKhoaHoc, string MaNganh);


        //khai báo service 
        SinhVienService sinhVienService = new SinhVienService();
        KhoaService khoaService = new KhoaService();
        KhoaHocService khoaHocService = new KhoaHocService();
        NganhHocService nganhHocService = new NganhHocService();
        LopService lopService = new LopService();


        //value public
        string maLopSV;

        //---------------------------DANH SÁCH HÀM PUBLIC------------------------------//
        //--------------------------------------------------------------------------------------//

        //hàm lấy ngày giờ và điếm thời gian
        public void NgayGio()
        {
            //get ngày
            DateTime ngay = DateTime.Now;
            lblDay.Text = ngay.ToString("dd/MM/yyyy");
            lblDay2.Text = ngay.ToString("dd/MM/yyyy");

            //get thời gian + điếm thời gian
            timerTime_QLSV.Start();
        }

        //hàm trở lại menu chính
        void goToGDUmanagement()
        {
            this.Hide();
            GDUManagement gdu = new GDUManagement();
            gdu.ShowDialog();
        }


        //laod danh sach khoa lên datagridview
        public void LoadDanhSachKhoaToDatagridview()
        {
            dgvDanhSachKhoa.DataSource = khoaService.GetAllKhoa();
        }

        //load danh sách khóa lên datagridview
        public void LoadDanhSachKhoaHocToDatagridview()
        {
            dgvDanhSachKhoaHoc.DataSource = khoaHocService.GetAllKhoaHoc();
        }

        //Load danh sách lớp học vào treeview 
        public void LoadDanhSachLopHocToTreeview()
        {
            string maNganh = cboChonNganhSV.SelectedValue.ToString();
            string maKhoaHoc = cboChonKhoaHocSV.SelectedValue.ToString();
            GDUDataConnectionsDataContext db = new GDUDataConnectionsDataContext();
            var listLp = from x in db.Lops where x.MaNganh == maNganh && x.MaKhoaHoc == maKhoaHoc select x;
            trvDSLop.Nodes.Clear();
            foreach (var lp in listLp)
            {
                TreeNode treeNode = new TreeNode("Danh Sách Lớp", 0, 0);
                treeNode.Name = lp.MaLop;
                treeNode.Text = lp.TenLop;
                trvDSLop.Nodes.Add(treeNode);
            }
            trvDSLop.ExpandAll();
        }

        //hàm show dữ liệu dgv lên textbox
        public void ShowDataTuDataGridViewToTextBox()
        {
            //Tab Khoa & Nganh
            txtMaKhoa.DataBindings.Clear();
            txtMaKhoa.DataBindings.Add("text", dgvDanhSachKhoa.DataSource, "MaKhoa");
            txtTenKhoa.DataBindings.Clear();
            txtTenKhoa.DataBindings.Add("text", dgvDanhSachKhoa.DataSource, "TenKhoa");

            lblMaKhoa.DataBindings.Clear();
            lblMaKhoa.DataBindings.Add("text", dgvDanhSachKhoa.DataSource, "MaKhoa");
            lblTenKhoa.DataBindings.Clear();
            lblTenKhoa.DataBindings.Add("text", dgvDanhSachKhoa.DataSource, "TenKhoa");

            //Tab Khóa & Lớp
            txtMaKhoaHoc.DataBindings.Clear();
            txtMaKhoaHoc.DataBindings.Add("text", dgvDanhSachKhoaHoc.DataSource, "MaKhoaHoc");
            txtTenKhoaHoc.DataBindings.Clear();
            txtTenKhoaHoc.DataBindings.Add("text", dgvDanhSachKhoaHoc.DataSource, "TenKhoaHoc");
            txtNienKhoa.DataBindings.Clear();
            txtNienKhoa.DataBindings.Add("text", dgvDanhSachKhoaHoc.DataSource, "NienKhoa");     

        }

        //show dữ liệu lên combox
        public void LoadDataToCombox()
        {
            //tab Khóa & Lớp
            cboChonKhoa.DataSource = khoaService.GetAllKhoa();
            cboChonKhoa.DisplayMember = "TenKhoa";
            cboChonKhoa.ValueMember = "MaKhoa";

            //tab Sinh Viên
            cboChonKhoaSV.DataSource = khoaService.GetAllKhoa();
            cboChonKhoaSV.DisplayMember = "TenKhoa";
            cboChonKhoaSV.ValueMember = "MaKhoa";

            string maKhoa = cboChonKhoa.SelectedValue.ToString();
            cboChonNganh.DataSource = nganhHocService.GetNganhHocByKHOA(maKhoa);
            cboChonNganh.DisplayMember = "TenNganh";
            cboChonNganh.ValueMember = "MaNganh";
            btnXemDanhSachLop.Enabled = true;
        }

        //hàm check data khoa
        public bool checkDataKHOA()
        {
            if (string.IsNullOrEmpty(txtTenKhoa.Text))
            {
                MessageBox.Show("Tên Khoa Không được bỏ trống, vui lòng kiểm tra lại...","Cảnh Báo",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txtTenKhoa.Focus();
                return false;
            }
            return true;
        }

        //hàm check data khóa học
        public bool checkDataKHOAHOC()
        {
            if (string.IsNullOrEmpty(txtMaKhoaHoc.Text))
            {
                MessageBox.Show("Mã Khóa Học Không được bỏ trống, vui lòng kiểm tra lại...","Cảnh Báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaKhoaHoc.Focus();
                txtMaKhoaHoc.ReadOnly = false;
                return false;
            }
            if (string.IsNullOrEmpty(txtTenKhoaHoc.Text))
            {
                MessageBox.Show("Tên Khóa Học Không được bỏ trống, vui lòng kiểm tra lại...", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKhoaHoc.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtNienKhoa.Text))
            {
                MessageBox.Show("Niên Khóa Không được bỏ trống, vui lòng kiểm tra lại...", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNienKhoa.Focus();
                return false;
            }
            return true;
        }

        //hàm check data Sinh Viên
        public bool checkDataSINHVIEN()
        {
            if (string.IsNullOrEmpty(txtTenSV.Text))
            {
                MessageBox.Show("Tên Sinh Viên Không được bỏ trống, vui lòng kiểm tra lại...", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSV.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Email Không được bỏ trống, vui lòng kiểm tra lại...", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtSdt.Text))
            {
                MessageBox.Show("SĐT Không được bỏ trống, vui lòng kiểm tra lại...", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSdt.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(rtxtDiaChi.Text))
            {
                MessageBox.Show("SĐT Không được bỏ trống, vui lòng kiểm tra lại...", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtxtDiaChi.Focus();
                return false;
            }
            return true;
        }

        //hàm các đóng kích hoạt các button khi hệ thống bắt đầu
        public void EnableFalseButton()
        {
            //tab  Khoa & Ngành
            btnSaveKhoa.Enabled = false;
            btnUpdateKhoa.Enabled = false;
            btnDeleteKhoa.Enabled = false;
           // btnDSNganh.Enabled = false;

            //tab Khóa & Lớp
            btnSaveKhoaHoc.Enabled = false;
            btnUpdateKhoaHoc.Enabled = false;
            btnDeleteKhoaHoc.Enabled = false;
           // btnXemDanhSachLop.Enabled = false;
        }

        //hàm show dữ liệu sih viên vên textbox, cbo,...
        public void ShowDataSinhVienTuDatagridview()
        {
            lblMaSV.DataBindings.Clear();
            lblMaSV.DataBindings.Add("text", dgvDanhSachSinhVien.DataSource, "MaSV");

            txtTenSV.DataBindings.Clear();
            txtTenSV.DataBindings.Add("text", dgvDanhSachSinhVien.DataSource, "TenSV");

            //lblTenLop.DataBindings.Clear();
            //lblTenLop.Text = maLopSV;

            txtEmail.DataBindings.Clear();
            txtEmail.DataBindings.Add("text", dgvDanhSachSinhVien.DataSource, "Email");

            txtSdt.DataBindings.Clear();
            txtSdt.DataBindings.Add("text", dgvDanhSachSinhVien.DataSource, "SDT");

            rtxtDiaChi.DataBindings.Clear();
            rtxtDiaChi.DataBindings.Add("text", dgvDanhSachSinhVien.DataSource, "DiaChi");

            rtxtGhiChu.DataBindings.Clear();
            rtxtGhiChu.DataBindings.Add("text", dgvDanhSachSinhVien.DataSource, "GhiChu");
        }

        //kiểm tra chuỗi nhập vào có phải số hay không
        public bool checkNumber(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsNumber(s[i]) == true)
                    return true;
            }
            return false;
        }

        //hàm autoID khoa
        public void AutoIDKhoa()
        {
            int count;
            string chuoi_id;
            int chuoi_id_key;

            count = dgvDanhSachKhoa.Rows.Count; //đếm tất cả các dòng trong datagridview
            chuoi_id = Convert.ToString(dgvDanhSachKhoa.Rows[count-1].Cells[1].Value);
            chuoi_id_key = Convert.ToInt32((chuoi_id.Remove(0, 9))); //loai bo cac ki tu so o dau id
            if (chuoi_id_key+1 < 10)
            {
                txtMaKhoa.Text = "GD8085990" + (chuoi_id_key + 1).ToString();
            }
            else if (chuoi_id_key + 1 >= 10)
            {
                txtMaKhoa.Text = "GD808599" + (chuoi_id_key + 1).ToString();
            }
        }

        public void AutoIDSinhVien()
        {
            int count = dgvDanhSachSinhVien.Rows.Count;  //đếm tất cả các dòng trong datagridview

            string maKhoaHoc = cboChonKhoaHocSV.SelectedValue.ToString();
            string lastID = maKhoaHoc.Substring(1);

            MessageBox.Show(maKhoaHoc);
            MessageBox.Show(lastID);

            if (count == 0)
            {
                lblMaSV.Text = "GDU1" + lastID + "000";
            }
            else
            {
                string chuoi_id;
                int chuoi_id_key;

                chuoi_id = Convert.ToString(dgvDanhSachSinhVien.Rows[count - 1].Cells[1].Value);
                chuoi_id_key = Convert.ToInt32((chuoi_id.Remove(0, 5))); //loai bo cac ki tu so o dau id

                if (chuoi_id_key + 1 < 10)
                {
                    txtMaKhoa.Text = "GDU1" + lastID +"00"+ (chuoi_id_key + 1).ToString();
                }
                else if (chuoi_id_key + 1 >= 10)
                {
                    txtMaKhoa.Text = "GDU1" + lastID + "0" + (chuoi_id_key + 1).ToString();
                }
                else if (chuoi_id_key + 1 >= 100)
                {
                    txtMaKhoa.Text = "GDU1" + lastID  + (chuoi_id_key + 1).ToString();
                }
            }
        }




        //-------------------------KẾT THÚC DS HÀM PUBLIC------------------------------//
        //--------------------------------------------------------------------------------------//
        private void timerTime_QLSV_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblTime2.Text = DateTime.Now.ToLongTimeString();

            int giay_qlk = Convert.ToInt32(lblGiay_QLK.Text);
            int phut_qlk = Convert.ToInt32(lblPhut_QLK.Text);

            int giay_qlkh = Convert.ToInt32(lblGiay_QLKH.Text);
            int phut_qlkh = Convert.ToInt32(lblPhut_QLKH.Text);
            giay_qlk++;
            giay_qlkh++;
            if (giay_qlk > 59 & giay_qlkh > 59)
            {
                giay_qlk = 0;
                phut_qlk++;

                giay_qlkh = 0;
                phut_qlkh++;
            }

            if (giay_qlk < 10  & giay_qlkh <10)
            {
                lblGiay_QLK.Text = "0" + giay_qlk;
                lblGiay_QLKH.Text = "0" + giay_qlkh;
            }
            else
            {
                lblGiay_QLK.Text = "" + giay_qlk;
                lblGiay_QLKH.Text = "" + giay_qlkh;
            }
            if (phut_qlk < 10 & phut_qlkh < 10)
            {
                lblPhut_QLK.Text = "0" + phut_qlk;
                lblPhut_QLKH.Text = "0" + phut_qlkh;
            }
            else
            {
                lblPhut_QLK.Text = "" + phut_qlk;
                lblPhut_QLKH.Text = "" + phut_qlkh;
            }
            if (giay_qlk % 2 == 0  & giay_qlkh % 2 == 0)
            {
                lblAnimation1_QKL.Text = "(^_^)";
                lblAnimation2_QLK.Text = "(+_+)";
                lblAnimation3_QLK.Text = "(-_^)";

                lblAnimation1_QLKH.Text = "(^_^)";
                lblAnimation2_QLKH.Text = "(+_+)";
                lblAnimation3_QLKH.Text = "(-_^)";
            }
            else if (giay_qlk % 2 != 0 & giay_qlkh % 2 != 0)
            {
                lblAnimation2_QLK.Text = "(^_^)";
                lblAnimation1_QKL.Text = "(+_+)";
                lblAnimation3_QLK.Text = "(^_-)";

                lblAnimation2_QLKH.Text = "(^_^)";
                lblAnimation2_QLKH.Text = "(+_+)";
                lblAnimation3_QLKH.Text = "(^_-)";
            }
        }

        private void frmQuanLySinhVien_Load(object sender, EventArgs e)
        {
           // LoadDanhSachSinhVienToDatagridview();
            LoadDanhSachKhoaToDatagridview();
            LoadDanhSachKhoaHocToDatagridview();
            ShowDataTuDataGridViewToTextBox();
            LoadDataToCombox();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void btnDSNganh_Click(object sender, EventArgs e)
        {
            try
            {
                frmDanhSachNganh frmDSNganh = new frmDanhSachNganh();
                SendMaKhoaToFrmDanhSachKhoa sendMaKhoa = new SendMaKhoaToFrmDanhSachKhoa(frmDSNganh.FunData);
                sendMaKhoa(this.txtMaKhoa);
                frmDSNganh.ShowDialog();
            }
            catch 
            {
                MessageBox.Show("Chưa Chọn Ngành, Vui Lòng Chọn 1 Ngành", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void btnXemDanhSachLop_Click(object sender, EventArgs e)
        {
            try
            {
                frmDanhSachLop frmDSLop = new frmDanhSachLop();
                SendMaKhoaHocMaNganhToFrmDanhSachLop senMaKhoaHocMaNganh = new SendMaKhoaHocMaNganhToFrmDanhSachLop(frmDSLop.FunDatafrmDanhSachLopToFrmQLSV);
                string maNganhKL = cboChonNganh.SelectedValue.ToString();
                string maKhoaHocKL = txtMaKhoaHoc.Text.Trim();
                senMaKhoaHocMaNganh(maNganhKL, maKhoaHocKL);
                frmDSLop.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Chưa Chọn Ngành Hoặc Khóa, Vui Lòng Kiểm Tra Lại...", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
        }

        private void btnHome_QLK_Click(object sender, EventArgs e)
        {
            goToGDUmanagement();
        }

        private void btnHome_QLKH_Click(object sender, EventArgs e)
        {
            goToGDUmanagement();
        }

        private void btnExit_QLK_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Bạn có muốn thoát khỏi chương trình không ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnExit_QLKH_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Bạn có muốn thoát khỏi chương trình không ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void dgvDanhSachKhoa_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            btnDSNganh.Enabled = true;
            btnSaveKhoa.Enabled = false;
            btnUpdateKhoa.Enabled = true;
            btnDeleteKhoa.Enabled = true;
            ShowDataTuDataGridViewToTextBox();
        }

        private void btnSaveKhoa_Click(object sender, EventArgs e)
        {
            if (checkDataKHOA())
            {
                Khoa khoa = new Khoa();
                khoa.MaKhoa = txtMaKhoa.Text.Trim();
                khoa.TenKhoa = txtTenKhoa.Text.Trim();

                khoaService.CreateKhoa(khoa);
                LoadDanhSachKhoaToDatagridview();
                MessageBox.Show("Thêm Thành Công...", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSaveKhoa.Enabled = false;
            }
            else
            {
                MessageBox.Show("Lỗi, Thêm Thất Bại", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNewKhoa_Click(object sender, EventArgs e)
        {
            AutoIDKhoa();
            btnSaveKhoa.Enabled = true;
            btnUpdateKhoa.Enabled = false;
            btnDeleteKhoa.Enabled = false;
            txtTenKhoa.Text = "";
        }

        private void btnUpdateKhoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaKhoa.Text))
            {
                MessageBox.Show("Cập nhật thông tin '" + txtMaKhoa.Text + "' Thất bại, Vui Lòng Kiểm Tra Lại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Khoa kh = new Khoa();
                kh.MaKhoa = txtMaKhoa.Text;
                kh.TenKhoa = txtTenKhoa.Text;
                khoaService.UpdateKhoa(kh);
                MessageBox.Show("Cập nhật thông tin '" + txtMaKhoa.Text + "' Thành Công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachKhoaToDatagridview();
            }
        }

        private void btnDeleteKhoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn Có Muốn Xóa '"+ txtMaKhoa.Text +"' ?", "THÔNG BÁO" ,MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string maKhoa;
                maKhoa = txtMaKhoa.Text.Trim();
                if (string.IsNullOrEmpty(txtMaKhoa.Text))
                {
                    MessageBox.Show("Xóa Thất Bại", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    khoaService.DeleteKhoa(maKhoa);
                    txtMaKhoa.Text = "";
                    txtTenKhoa.Text = "";
                    btnNewKhoa.Enabled = true;
                    btnSaveKhoa.Enabled = false;
                    btnUpdateKhoa.Enabled = false;
                    btnDeleteKhoa.Enabled = false;
                    LoadDanhSachKhoaToDatagridview();
                    MessageBox.Show("Đã Xóa...!!!", "Thông Báo", MessageBoxButtons.OK);
                }
            }
        }

        private void dgvDanhSachKhoa_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            btnSaveKhoa.Enabled = false;
            btnUpdateKhoa.Enabled = true;
            btnDeleteKhoa.Enabled = true;
            ShowDataTuDataGridViewToTextBox();
        }

        private void cboChonKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            string maKhoa = cboChonKhoa.SelectedValue.ToString();
            cboChonNganh.DataSource = nganhHocService.GetNganhHocByKHOA(maKhoa);
            cboChonNganh.DisplayMember = "TenNganh";
            cboChonNganh.ValueMember = "MaNganh";
            btnXemDanhSachLop.Enabled = true;
        }

        private void cboChonNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
           // MessageBox.Show(cboChonNganh.SelectedValue.ToString());
        }

        private void btnNewKhoaHoc_Click(object sender, EventArgs e)
        {
            txtMaKhoaHoc.Clear();
            txtTenKhoaHoc.Clear();
            txtNienKhoa.Clear();

            btnSaveKhoaHoc.Enabled = true;
            btnUpdateKhoaHoc.Enabled = false;
            btnDeleteKhoaHoc.Enabled = false;
        }

        private void txtTimKiem_QLK_TextChanged(object sender, EventArgs e)
        {
            dgvDanhSachKhoa.DataSource = khoaService.SearchKhoaByTenKhoa(txtTimKiem_QLK.Text).ToList();
            dgvDanhSachKhoa.DataSource = khoaService.SearchKhoaByMaKhoa(txtTimKiem_QLK.Text).ToList();
        }

        private void txtTimKiem_QLK_MouseClick(object sender, MouseEventArgs e)
        {
            txtTimKiem_QLK.Clear();
        }

        private void btnSaveKhoaHoc_Click(object sender, EventArgs e)
        {
            if (checkDataKHOAHOC())
            {
                KhoaHoc khoaHoc = new KhoaHoc();
                khoaHoc.MaKhoaHoc = txtMaKhoaHoc.Text.Trim();
                khoaHoc.TenKhoaHoc = txtTenKhoaHoc.Text.Trim();
                khoaHoc.NienKhoa = txtNienKhoa.Text.Trim();

                khoaHocService.CreateKhoaHoc(khoaHoc);
                LoadDanhSachKhoaHocToDatagridview();
                MessageBox.Show("Thêm Thành Công...", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaKhoaHoc.Clear();
                txtTenKhoaHoc.Clear();
                txtNienKhoa.Clear();
                btnSaveKhoaHoc.Enabled = false;
            }
            else
            {
                MessageBox.Show("Thêm Thất Bại, Vui Lòng Kiểm Tra Lạ...", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateKhoaHoc_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaKhoaHoc.Text))
            {
                MessageBox.Show("Cập Nhật Thông Tin '" + txtMaKhoaHoc.Text + "' Thất bại, Vui Lòng Kiểm Tra lại...", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                KhoaHoc khoaHoc = new KhoaHoc();
                khoaHoc.MaKhoaHoc = txtMaKhoaHoc.Text.Trim();
                khoaHoc.TenKhoaHoc = txtTenKhoaHoc.Text.Trim();
                khoaHoc.NienKhoa = txtNienKhoa.Text.Trim();
                khoaHocService.UpdateKhoaHoc(khoaHoc);
                LoadDanhSachKhoaHocToDatagridview();
                MessageBox.Show("Cập Nhật Thông Tin '" + txtMaKhoaHoc.Text + "' Thành Công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgvDanhSachKhoaHoc_MouseClick(object sender, MouseEventArgs e)
        {
            ShowDataTuDataGridViewToTextBox();
            btnSaveKhoaHoc.Enabled = false;
            btnUpdateKhoaHoc.Enabled = true;
            btnDeleteKhoaHoc.Enabled = true;
        }

        private void btnDeleteKhoaHoc_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn Có Muốn Xóa '" + txtMaKhoaHoc.Text + "' ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string maKhoaHoc = txtMaKhoaHoc.Text;
                if (string.IsNullOrEmpty(txtMaKhoaHoc.Text))
                {
                    MessageBox.Show("Xóa Thất Bại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    khoaHocService.DeleteKhoaHoc(maKhoaHoc);
                    LoadDanhSachKhoaHocToDatagridview();
                    MessageBox.Show("Xóa Thành Công...", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtMaKhoaHoc.Text = "";
                    txtTenKhoaHoc.Text = "";
                    txtNienKhoa.Text = "";
                    btnDeleteKhoa.Enabled = false;
                    btnUpdateKhoa.Enabled = false;
                }
            }
         }

        private void txtTimKiemKhoaHocKL_MouseClick(object sender, MouseEventArgs e)
        {
            txtTimKiemKhoaHocKL.Clear();
        }


        private void txtTimKiemKhoaHocKL_TextChanged(object sender, EventArgs e)
        {
            //dgvDanhSachKhoaHoc.DataSource = khoaHocService.SearchKhoaHocByMaKhoaHoc(txtTimKiemKhoaHocKL.Text.Trim()).ToList();
            //dgvDanhSachKhoaHoc.DataSource = khoaHocService.SearchKhoaHocByTenKhoaHoc(txtTimKiemKhoaHocKL.Text.Trim()).ToList();

            //dgvDanhSachKhoaHoc.DataSource = khoaHocService.SearchKhoaHocByNienKhoa(txtTimKiemKhoaHocKL.Text.Trim()).ToList();

            if (checkNumber(txtTimKiemKhoaHocKL.Text))
            {
                dgvDanhSachKhoaHoc.DataSource = khoaHocService.SearchKhoaHocByNienKhoa(txtTimKiemKhoaHocKL.Text.Trim()).ToList();
            }
            else
            {
                dgvDanhSachKhoaHoc.DataSource = khoaHocService.SearchKhoaHocByTenKhoaHoc(txtTimKiemKhoaHocKL.Text.Trim()).ToList();
            }
        }

        private void cboChonKhoaSV_SelectedIndexChanged(object sender, EventArgs e)
        {
            string maKhoa = cboChonKhoaSV.SelectedValue.ToString();
            cboChonNganhSV.DataSource = nganhHocService.GetNganhHocByKHOA(maKhoa);
            cboChonNganhSV.DisplayMember = "TenNganh";
            cboChonNganhSV.ValueMember = "MaNganh";
        }

        private void cboChonNganhSV_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboChonKhoaHocSV.DataSource = khoaHocService.GetAllKhoaHoc();
            cboChonKhoaHocSV.DisplayMember = "TenKhoaHoc";
            cboChonKhoaHocSV.ValueMember = "MaKhoaHoc";
        }

        private void cboChonKhoaHocSV_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            LoadDanhSachLopHocToTreeview();
        }

        private void trvDSLop_Click(object sender, EventArgs e)
        {
            //LoadDanhSachSinhVienToDatagridview();
        }

        private void trvDSLop_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            maLopSV = e.Node.Name;
            lblTenLop.Text = e.Node.Text; 
            dgvDanhSachSinhVien.DataSource = sinhVienService.GetSinhVienByMaLop(maLopSV);
            btnNewSV.Enabled = true;
        }

        private void dgvDanhSachSinhVien_MouseClick(object sender, MouseEventArgs e)
        {
            ShowDataSinhVienTuDatagridview();
        }

        private void btnHome_SV_Click(object sender, EventArgs e)
        {
            goToGDUmanagement();
        }

        private void btnExitSV_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Bạn có muốn thoát khỏi chương trình không ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnSaveSV_Click(object sender, EventArgs e)
        {
            if (checkDataSINHVIEN())
            {
                SinhVien sv = new SinhVien();
                sv.MaSV = lblMaSV.Text;
                sv.TenSV = txtTenSV.Text;
                if (radNam.Checked)
                {
                    sv.GioiTinh = radNam.Text;
                }
                else if (radNu.Checked)
                {
                    sv.GioiTinh = radNu.Text;
                }
                sv.Email = txtEmail.Text;
                sv.Password = lblMaSV.Text;
                sv.NamSinh = dtpNamSinh.Value.ToString();
                sv.SDT = txtSdt.Text;
                sv.DiaChi = rtxtDiaChi.Text;
                sv.GhiChu = rtxtGhiChu.Text;
                sv.MaLop = maLopSV;
                sinhVienService.CreateSinhVien(sv);
                dgvDanhSachSinhVien.DataSource = sinhVienService.GetSinhVienByMaLop(maLopSV);
                MessageBox.Show("Thêm Thành Công...", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenSV.Clear();
                txtEmail.Clear();
                txtSdt.Clear();
                rtxtDiaChi.Clear();
                rtxtGhiChu.Clear();
            }
        }

        private void btnNewSV_Click(object sender, EventArgs e)
        {
            AutoIDSinhVien();
            txtTenSV.Clear();
            txtEmail.Clear();
            txtSdt.Clear();
            rtxtDiaChi.Clear();
            rtxtGhiChu.Clear();
        }
    }
}
