
using GDU_Management.Model;
using GDU_Management.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        delegate void SendMaKhoaToFrmDanhSachKhoa(Label dlgtxtMaKhoa);
        delegate void SendMaKhoaHocMaNganhToFrmDanhSachLop(string dlgtMaKhoaHoc, string MaNganh);


        //khai báo service 
        SinhVienService sinhVienService = new SinhVienService();
        KhoaService khoaService = new KhoaService();
        KhoaHocService khoaHocService = new KhoaHocService();
        NganhHocService nganhHocService = new NganhHocService();
        LopService lopService = new LopService();


        //value public
        string maLopSV;             //giá trị mã lớp lấy từ treeview trên tabSinhVien

        //---------------------------DANH SÁCH HÀM PUBLIC------------------------------//
        //---------------------------------------------------------------------------------------//

        //hàm lấy ngày giờ và điếm thời gian
        public void NgayGio()
        {
            //get ngày
            DateTime ngay = DateTime.Now;
            lblDay.Text = ngay.ToString("dd/MM/yyyy");
            lblDayKL.Text = ngay.ToString("dd/MM/yyyy");

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
        public void LoadDanhSachKhoasHocToDatagridview()
        {
            dgvDanhSachKhoasHoc.DataSource = khoaHocService.GetAllKhoaHoc();
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
            lblMaKhoaKN.DataBindings.Clear();
            lblMaKhoaKN.DataBindings.Add("text", dgvDanhSachKhoa.DataSource, "MaKhoa");
            txtTenKhoa.DataBindings.Clear();
            txtTenKhoa.DataBindings.Add("text", dgvDanhSachKhoa.DataSource, "TenKhoa");

            lblMaKhoaKN_2.DataBindings.Clear();
            lblMaKhoaKN_2.DataBindings.Add("text", dgvDanhSachKhoa.DataSource, "MaKhoa");
            lblTenKhoa.DataBindings.Clear();
            lblTenKhoa.DataBindings.Add("text", dgvDanhSachKhoa.DataSource, "TenKhoa");

            //Tab Khóa & Lớp
            lblMaKhoaHocKL.DataBindings.Clear();
            lblMaKhoaHocKL.DataBindings.Add("text", dgvDanhSachKhoasHoc.DataSource, "MaKhoaHoc");
            txtTenKhoaHoc.DataBindings.Clear();
            txtTenKhoaHoc.DataBindings.Add("text", dgvDanhSachKhoasHoc.DataSource, "TenKhoaHoc");
            txtNienKhoa.DataBindings.Clear();
            txtNienKhoa.DataBindings.Add("text", dgvDanhSachKhoasHoc.DataSource, "NienKhoa");     

        }

        //hàm show dữ liệu sih viên vên textbox, cbo,...
        public void ShowDataSinhVienTuDatagridview()
        {
            lblMaSV.DataBindings.Clear();
            lblMaSV.DataBindings.Add("text", dgvDanhSachSinhVien.DataSource, "MaSV");

            txtTenSV.DataBindings.Clear();
            txtTenSV.DataBindings.Add("text", dgvDanhSachSinhVien.DataSource, "TenSV");

            txtEmail.DataBindings.Clear();
            txtEmail.DataBindings.Add("text", dgvDanhSachSinhVien.DataSource, "Email");

            txtSdt.DataBindings.Clear();
            txtSdt.DataBindings.Add("text", dgvDanhSachSinhVien.DataSource, "SDT");

            rtxtDiaChi.DataBindings.Clear();
            rtxtDiaChi.DataBindings.Add("text", dgvDanhSachSinhVien.DataSource, "DiaChi");

            rtxtGhiChu.DataBindings.Clear();
            rtxtGhiChu.DataBindings.Add("text", dgvDanhSachSinhVien.DataSource, "GhiChu");

            string gioiTinh = dgvDanhSachSinhVien.CurrentRow.Cells[3].Value.ToString();
            if (gioiTinh.Equals("Nam"))
            {
                radNam.Checked = true;
                radNu.Checked = false;
            }
            else if (gioiTinh.Equals("Nữ"))
            {
                radNam.Checked = false;
                radNu.Checked = true;
            }

            dtpNamSinh.DataBindings.Clear();
            dtpNamSinh.DataBindings.Add("text", dgvDanhSachSinhVien.DataSource, "NamSinh");
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
            if (string.IsNullOrEmpty(lblMaKhoaHocKL.Text))
            {
                MessageBox.Show("Mã Khóa Học Không được bỏ trống, vui lòng kiểm tra lại...","Cảnh Báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            //kiểm tra tên
            if (string.IsNullOrEmpty(txtTenSV.Text))
            {
                MessageBox.Show("Tên Sinh Viên Không được bỏ trống, vui lòng kiểm tra lại...", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSV.Focus();
                return false;
            }

            //kiểm tra email
            if (!string.IsNullOrEmpty(txtEmail.Text))
            {
                string email = txtEmail.Text;
                var value = email.EndsWith("@gmail.com");
                string reEmail = value.ToString();
                if (reEmail.Equals("False"))
                {
                    MessageBox.Show("Định dạng email không đúng, vui lòng kiểm tra lại...", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Email Không được bỏ trống, vui lòng kiểm tra lại...", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            //kiểm tra sđt
            if (!string.IsNullOrEmpty(txtSdt.Text))
            {
                Regex isValidInput = new Regex(@"^\d{10}$");
                string sdt = txtSdt.Text.Trim();
                if (!isValidInput.IsMatch(sdt))
                {
                    MessageBox.Show("SĐT bao gồm 10 số và không có kí tự, vui lòng kiểm tra lại...", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSdt.Focus();
                    return false;
                }
            }
            else
            {
                MessageBox.Show("SĐT Không được bỏ trống, vui lòng kiểm tra lại...", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSdt.Focus();
                return false;
            }
           
            //kiểm tra địa chỉ
            if (string.IsNullOrEmpty(rtxtDiaChi.Text))
            {
                MessageBox.Show("Địa Chỉ Không được bỏ trống, vui lòng kiểm tra lại...", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            //tab Khóa & Lớp
            btnSaveKhoaHoc.Enabled = false;
            btnUpdateKhoaHoc.Enabled = false;
            btnDeleteKhoaHoc.Enabled = false;

            //tab Sinh Viên
            btnSaveSV.Enabled = false;
            btnUpdateSV.Enabled = false;
            btnDeleteSV.Enabled = false;
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
            count = dgvDanhSachKhoa.Rows.Count;

            if (count == 0)
            {
                lblMaKhoaKN.Text = "GD80859900";
            }
            else
            {
                string chuoi_id = "";
                int chuoi_id_key = 0;

                chuoi_id = Convert.ToString(dgvDanhSachKhoa.Rows[count - 1].Cells[1].Value);
                chuoi_id_key = Convert.ToInt32(chuoi_id.Remove(0, 8));

                if (chuoi_id_key + 1 < 10)
                {
                    lblMaKhoaKN.Text = "GD8085990" + (chuoi_id_key + 1).ToString();
                }
                else if (chuoi_id_key + 1 >= 10)
                {
                    lblMaKhoaKN.Text = "GD808599" + (chuoi_id_key + 1).ToString();
                }
            }
        }

        //hàm auto data khóa
        public void AutoDataKhoas()
        {
            int countRows;
            countRows = dgvDanhSachKhoasHoc.Rows.Count;
            if (countRows == 0)
            {
                lblMaKhoaHocKL.Text = "K01";
                txtTenKhoaHoc.Text = "Khóa 1";
            }
            else if (0 < countRows && countRows < 9)
            {
                string chuoi_id = "";
                int chuoi_id_key = 0;
                chuoi_id = Convert.ToString(dgvDanhSachKhoasHoc.Rows[countRows - 1].Cells[1].Value);
                chuoi_id_key = Convert.ToInt32(chuoi_id.Remove(0, 2));

                string chuoi_text = "";
                int chuoi_text_key = 0;
                chuoi_text = Convert.ToString(dgvDanhSachKhoasHoc.Rows[countRows - 1].Cells[2].Value);
                chuoi_text_key = Convert.ToInt32(chuoi_text.Remove(0, 5));

                lblMaKhoaHocKL.Text = "K0" + (chuoi_id_key + 1).ToString();
                txtTenKhoaHoc.Text = "Khóa " + (chuoi_text_key + 1);
            }
            else if (countRows == 9)
            {
                string chuoi_id = "";
                int chuoi_id_key = 0;
                chuoi_id = Convert.ToString(dgvDanhSachKhoasHoc.Rows[countRows-1].Cells[1].Value);
                chuoi_id_key = Convert.ToInt32(chuoi_id.Remove(0, 1));

                string chuoi_text = "";
                int chuoi_text_key = 0;
                chuoi_text = Convert.ToString(dgvDanhSachKhoasHoc.Rows[countRows-1].Cells[2].Value);
                chuoi_text_key = Convert.ToInt32(chuoi_text.Remove(0, 5));

                lblMaKhoaHocKL.Text = "K" + (chuoi_id_key + 1).ToString();
                txtTenKhoaHoc.Text = "Khóa " + (chuoi_text_key + 1);
            }
            else if (countRows >= 10)
            {
                string chuoi_id = "";
                int chuoi_id_key = 0;
                chuoi_id = Convert.ToString(dgvDanhSachKhoasHoc.Rows[countRows - 1].Cells[1].Value);
                chuoi_id_key = Convert.ToInt32(chuoi_id.Remove(0, 1));

                string chuoi_text = "";
                int chuoi_text_key = 0;
                chuoi_text = Convert.ToString(dgvDanhSachKhoasHoc.Rows[countRows - 1].Cells[2].Value);
                chuoi_text_key = Convert.ToInt32(chuoi_text.Remove(0, 5));

                lblMaKhoaHocKL.Text = "K" + (chuoi_id_key + 1).ToString();
                txtTenKhoaHoc.Text = "Khóa " + (chuoi_text_key + 1);
            }

            string nk = lblDayKL.Text.ToString();
            string getNowYear = nk.Substring(6);
            txtNienKhoa.Text = getNowYear;
        }

        //hàm auto id sinh viên
        public void AutoIDSinhVien()
        {
            int count;
            count = dgvDanhSachSinhVien.Rows.Count;

            string IdKhoas = cboChonKhoaHocSV.SelectedValue.ToString();
            string LastIdKhoas = IdKhoas.Substring(1);                  //lấy 2 số cuối mã khóa

            string LastIdLop = maLopSV.Substring(7);                    //lấy 2 số cuối mã lớp

            if (count == 0)
            {
                lblMaSV.Text = "GD1" + LastIdKhoas + LastIdLop + "000";
            }
            else
            {
                string chuoi_id = "";
                int chuoi_id_key = 0;

                chuoi_id = Convert.ToString(dgvDanhSachSinhVien.Rows[count - 1].Cells[0].Value);
                chuoi_id_key = Convert.ToInt32(chuoi_id.Remove(0, 9));

                if (chuoi_id_key + 1 < 10)
                {
                    lblMaSV.Text = "GD1" + LastIdKhoas + LastIdLop + "00" + (chuoi_id_key + 1);
                }
                else if (chuoi_id_key + 1 >= 10)
                {
                    lblMaSV.Text = "GD1" + LastIdKhoas + LastIdLop + "0" + (chuoi_id_key + 1);
                }
                else if (chuoi_id_key + 1 >= 100)
                {
                    lblMaSV.Text = "GD1" + LastIdKhoas + LastIdLop  + (chuoi_id_key + 1);
                }
            }
        }




        //-------------------------KẾT THÚC DS HÀM PUBLIC------------------------------//
        //--------------------------------------------------------------------------------------//
        private void timerTime_QLSV_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblTimeKL.Text = DateTime.Now.ToLongTimeString();

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

                lblAnimation1_KL.Text = "(^_^)";
                lblAnimation2_KL.Text = "(+_+)";
                lblAnimation3_KL.Text = "(-_^)";
            }
            else if (giay_qlk % 2 != 0 & giay_qlkh % 2 != 0)
            {
                lblAnimation2_QLK.Text = "(^_^)";
                lblAnimation1_QKL.Text = "(+_+)";
                lblAnimation3_QLK.Text = "(^_-)";

                lblAnimation2_KL.Text = "(^_^)";
                lblAnimation2_KL.Text = "(+_+)";
                lblAnimation3_KL.Text = "(^_-)";
            }
        }

        private void frmQuanLySinhVien_Load(object sender, EventArgs e)
        {
            LoadDanhSachKhoaToDatagridview();
            LoadDanhSachKhoasHocToDatagridview();
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
                sendMaKhoa(this.lblMaKhoaKN);
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
                string maKhoaHocKL = lblMaKhoaHocKL.Text.Trim();
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
                khoa.MaKhoa = lblMaKhoaKN.Text.Trim();
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
            LoadDanhSachKhoaToDatagridview();
            AutoIDKhoa();
            btnSaveKhoa.Enabled = true;
            btnUpdateKhoa.Enabled = false;
            btnDeleteKhoa.Enabled = false;
            txtTenKhoa.Text = "";
            txtTenKhoa.Focus();
        }

        private void btnUpdateKhoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblMaKhoaKN.Text))
            {
                MessageBox.Show("Cập nhật thông tin '" + lblMaKhoaKN.Text + "' Thất bại, Vui Lòng Kiểm Tra Lại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Khoa kh = new Khoa();
                kh.MaKhoa = lblMaKhoaKN.Text;
                kh.TenKhoa = txtTenKhoa.Text;
                khoaService.UpdateKhoa(kh);
                MessageBox.Show("Cập nhật thông tin '" + lblMaKhoaKN.Text + "' Thành Công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachKhoaToDatagridview();
            }
        }

        private void btnDeleteKhoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn Có Muốn Xóa '"+ lblMaKhoaKN.Text +"' ?", "THÔNG BÁO" ,MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string maKhoa;
                maKhoa = lblMaKhoaKN.Text.Trim();
                if (string.IsNullOrEmpty(lblMaKhoaKN.Text))
                {
                    MessageBox.Show("Xóa Thất Bại", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    khoaService.DeleteKhoa(maKhoa);
                    lblMaKhoaKN.Text = "null";
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
            AutoDataKhoas();
            nubNienKhoaKL.Enabled = true;
            btnSaveKhoaHoc.Enabled = true;
            btnUpdateKhoaHoc.Enabled = false;
            btnDeleteKhoaHoc.Enabled = false;
            LoadDanhSachKhoasHocToDatagridview();
        }

        private void txtTimKiem_QLK_TextChanged(object sender, EventArgs e)
        {
                dgvDanhSachKhoa.DataSource = khoaService.SearchKhoaByTenKhoa(txtTimKiemKhoa_KN.Text).ToList();
        }

        private void txtTimKiem_QLK_MouseClick(object sender, MouseEventArgs e)
        {
            txtTimKiemKhoa_KN.Clear();
        }

        private void btnSaveKhoaHoc_Click(object sender, EventArgs e)
        {
            if (checkDataKHOAHOC())
            {
                int nienKhoa = Convert.ToInt32(txtNienKhoa.Text);
                int soNienKhoa = Convert.ToInt32(nubNienKhoaKL.Value.ToString());

                KhoaHoc khoaHoc = new KhoaHoc();
                khoaHoc.MaKhoaHoc = lblMaKhoaHocKL.Text.Trim();
                khoaHoc.TenKhoaHoc = txtTenKhoaHoc.Text.Trim();
                khoaHoc.NienKhoa = txtNienKhoa.Text.Trim()+"-" + (nienKhoa + soNienKhoa);

                khoaHocService.CreateKhoaHoc(khoaHoc);
                LoadDanhSachKhoasHocToDatagridview();
                MessageBox.Show("Thêm Thành Công...", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSaveKhoaHoc.Enabled = false;
                nubNienKhoaKL.Enabled = false;
            }
            else
            {
                MessageBox.Show("Thêm Thất Bại, Vui Lòng Kiểm Tra Lạ...", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateKhoaHoc_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblMaKhoaHocKL.Text))
            {
                MessageBox.Show("Cập Nhật Thông Tin '" + lblMaKhoaHocKL.Text + "' Thất bại, Vui Lòng Kiểm Tra lại...", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                KhoaHoc khoaHoc = new KhoaHoc();
                khoaHoc.MaKhoaHoc = lblMaKhoaHocKL.Text.Trim();
                khoaHoc.TenKhoaHoc = txtTenKhoaHoc.Text.Trim();
                khoaHoc.NienKhoa = txtNienKhoa.Text.Trim();
                khoaHocService.UpdateKhoaHoc(khoaHoc);
                LoadDanhSachKhoasHocToDatagridview();
                MessageBox.Show("Cập Nhật Thông Tin '" + lblMaKhoaHocKL.Text + "' Thành Công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgvDanhSachKhoaHoc_MouseClick(object sender, MouseEventArgs e)
        {
            ShowDataTuDataGridViewToTextBox();
            btnSaveKhoaHoc.Enabled = false;
            btnUpdateKhoaHoc.Enabled = true;
            btnDeleteKhoaHoc.Enabled = true;
            nubNienKhoaKL.Enabled = false;
        }

        private void btnDeleteKhoaHoc_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn Có Muốn Xóa '" + lblMaKhoaHocKL.Text + "' ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string maKhoaHoc = lblMaKhoaHocKL.Text;
                if (string.IsNullOrEmpty(lblMaKhoaHocKL.Text))
                {
                    MessageBox.Show("Xóa Thất Bại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    khoaHocService.DeleteKhoaHoc(maKhoaHoc);
                    LoadDanhSachKhoasHocToDatagridview();
                    MessageBox.Show("Đã Xóa [-"+lblMaKhoaKN.Text+"-]", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblMaKhoaHocKL.Text = "null";
                    txtTenKhoaHoc.Text = "";
                    txtNienKhoa.Text = "";
                    btnDeleteKhoa.Enabled = false;
                    btnUpdateKhoa.Enabled = false;
                }
            }
         }

        private void txtTimKiemKhoaHocKL_MouseClick(object sender, MouseEventArgs e)
        {
            txtTimKiemKhoasHoc_KL.Clear();
        }


        private void txtTimKiemKhoaHocKL_TextChanged(object sender, EventArgs e)
        {
            //dgvDanhSachKhoaHoc.DataSource = khoaHocService.SearchKhoaHocByMaKhoaHoc(txtTimKiemKhoaHocKL.Text.Trim()).ToList();
            //dgvDanhSachKhoaHoc.DataSource = khoaHocService.SearchKhoaHocByTenKhoaHoc(txtTimKiemKhoaHocKL.Text.Trim()).ToList();
            //dgvDanhSachKhoaHoc.DataSource = khoaHocService.SearchKhoaHocByNienKhoa(txtTimKiemKhoaHocKL.Text.Trim()).ToList();

            if (checkNumber(txtTimKiemKhoasHoc_KL.Text))
            {
                dgvDanhSachKhoasHoc.DataSource = khoaHocService.SearchKhoaHocByNienKhoa(txtTimKiemKhoasHoc_KL.Text.Trim()).ToList();
            }
            else
            {
                dgvDanhSachKhoasHoc.DataSource = khoaHocService.SearchKhoaHocByTenKhoaHoc(txtTimKiemKhoasHoc_KL.Text.Trim()).ToList();
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
            lblMaSV.Text = "null";
            maLopSV = e.Node.Name;
            lblTenLop.Text = e.Node.Text; 
            dgvDanhSachSinhVien.DataSource = sinhVienService.GetSinhVienByMaLop(maLopSV);
            btnNewSV.Enabled = true;
            txtTenSV.Clear();
            txtEmail.Clear();
            txtSdt.Clear();
            rtxtDiaChi.Clear();
            rtxtGhiChu.Clear();
            btnDeleteSV.Enabled = false;
            btnUpdateSV.Enabled = false;
        }

        private void dgvDanhSachSinhVien_MouseClick(object sender, MouseEventArgs e)
        {
            ShowDataSinhVienTuDatagridview();
            btnSaveSV.Enabled = false;
            btnUpdateSV.Enabled = true;
            btnDeleteSV.Enabled = true;
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
                btnSaveSV.Enabled = false;
                btnUpdateSV.Enabled = true;
                btnDeleteSV.Enabled = true;
            }
        }

        private void btnNewSV_Click(object sender, EventArgs e)
        {
            AutoIDSinhVien();
            txtTenSV.Focus();
            txtTenSV.Clear();
            txtEmail.Clear();
            txtSdt.Clear();
            rtxtDiaChi.Clear();
            rtxtGhiChu.Clear();
            btnDeleteSV.Enabled = false;
            btnUpdateSV.Enabled = false;
            btnSaveSV.Enabled = true;
        }

        private void btnUpdateSV_Click(object sender, EventArgs e)
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
                //sv.Password = lblMaSV.Text;
                sv.NamSinh = dtpNamSinh.Text;
                sv.SDT = txtSdt.Text;
                sv.DiaChi = rtxtDiaChi.Text;
                sv.GhiChu = rtxtGhiChu.Text;
                sv.MaLop = maLopSV;
                dgvDanhSachSinhVien.DataSource = sinhVienService.GetSinhVienByMaLop(maLopSV);
                sinhVienService.UpdateSinhVien(sv);
                MessageBox.Show("Cập Nhật Thông Tin [" + lblMaSV.Text + "] thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDeleteSV_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xóa [-" + lblMaSV.Text + "-]", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                string maSV = lblMaSV.Text.Trim();
                if (maSV.Equals("null"))
                {
                    MessageBox.Show("Xóa Thất Bại, Mã sinh viên [-" + lblMaSV.Text + "-] không tồn tại", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }
                else
                {
                    sinhVienService.DeleteSinhVien(maSV);
                    dgvDanhSachSinhVien.DataSource = sinhVienService.GetSinhVienByMaLop(maLopSV);
                    MessageBox.Show("Đã Xóa [-" + lblMaSV.Text + "-]", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblMaSV.Text = "null";
                    txtTenSV.Clear();
                    txtEmail.Clear();
                    txtSdt.Clear();
                    rtxtDiaChi.Clear();
                    rtxtGhiChu.Clear();
                    btnSaveSV.Enabled = false;
                    btnUpdateSV.Enabled = false;
                    btnDeleteSV.Enabled = false;
                }
            }
        }

        private void txtTimKiemSV_TextChanged(object sender, EventArgs e)
        {
            string timKiem = txtTimKiemSV.Text.Trim();
            if (timKiem.Equals(""))
            {
                dgvDanhSachSinhVien.DataSource = sinhVienService.GetSinhVienByMaLop(maLopSV);
            }
            else
            {
                dgvDanhSachSinhVien.DataSource = sinhVienService.SearchSinhVienByTenSinhVien(timKiem);
            }
        }
    }
}
