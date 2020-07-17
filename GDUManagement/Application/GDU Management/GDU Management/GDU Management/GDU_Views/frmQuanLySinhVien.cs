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
using System.Net.Mail;
using System.Net;
using GDU_Management.Controller;

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
        KhoasHocService khoaHocService = new KhoasHocService();
        NganhHocService nganhHocService = new NganhHocService();
        LopService lopService = new LopService();
        MonHocService monHocService = new MonHocService();
        DiemMonHocService diemMonHocService = new DiemMonHocService();
        ContactService contactService = new ContactService();

        //khai bao controller
        SendMessage sendMessage = new SendMessage();
        


        //value public
        string maLopSV;             //giá trị mã lớp lấy từ treeview trên tabSinhVien

        //---------------------------DANH SÁCH HÀM PUBLIC------------------------------//
        //______________________________________________________//

        //hàm lấy ngày giờ và điếm thời gian
        public void NgayGio()
        {
            //get ngày
            DateTime ngay = DateTime.Now;
            lblDay.Text = ngay.ToString("dddd, dd-MM-yyyy");
            lblDayKL.Text = ngay.ToString("dddd, dd-MM-yyyy");

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
            CountRowsKhoa();
        }

        //load danh sách khóa lên datagridview
        public void LoadDanhSachKhoasHocToDatagridview()
        {
            dgvDanhSachKhoasHoc.DataSource = khoaHocService.GetAllKhoaHoc();
            CountRowsKhoasHoc();
        }

        //Load danh sách lớp học vào treeview 
        public void LoadDanhSachLopHocToTreeview()
        {
            string maNganh = cboChonNganhSV.SelectedValue.ToString();
            string maKhoaHoc = cboChonKhoasHocSV.SelectedValue.ToString();
            var listLp = lopService.GetDanhSachLopByMaNganhVaMaKhoaHoc(maNganh, maKhoaHoc);
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

        public void LoadDanhSachSinhVienToDgv()
        {
            dgvDanhSachSinhVien.DataSource = sinhVienService.GetSinhVienByMaLop(maLopSV);
            CountRowsSinhVien();
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
            cboChonKhoaKL.DataSource = khoaService.GetAllKhoa();
            cboChonKhoaKL.DisplayMember = "TenKhoa";
            cboChonKhoaKL.ValueMember = "MaKhoa";

            //tab Sinh Viên
            cboChonKhoaSV.DataSource = khoaService.GetAllKhoa();
            cboChonKhoaSV.DisplayMember = "TenKhoa";
            cboChonKhoaSV.ValueMember = "MaKhoa";

            string maKhoa = cboChonKhoaKL.SelectedValue.ToString();
            cboChonNganhKL.DataSource = nganhHocService.GetNganhHocByKHOA(maKhoa);
            cboChonNganhKL.DisplayMember = "TenNganh";
            cboChonNganhKL.ValueMember = "MaNganh";
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

        //hàm  đóng kích hoạt các button khi hệ thống bắt đầu
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
                lblMaKhoaKN.Text = "GD80859901";
                dgvDanhSachKhoa.Rows[1].Cells[0].Value = "1";
                
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

                lblMaKhoaHocKL.Text = "K0" + (chuoi_id_key + 1).ToString();
                txtTenKhoaHoc.Text = "Khóa " + (chuoi_id_key + 1).ToString();
            }
            else if (countRows >= 9)
            {
                string chuoi_id = "";
                int chuoi_id_key = 0;
                chuoi_id = Convert.ToString(dgvDanhSachKhoasHoc.Rows[countRows - 1].Cells[1].Value);
                chuoi_id_key = Convert.ToInt32(chuoi_id.Remove(0, 1));

                lblMaKhoaHocKL.Text = "K" + (chuoi_id_key + 1).ToString();
                txtTenKhoaHoc.Text = "Khóa " + (chuoi_id_key + 1).ToString();
            }

            string nk = lblDayKL.Text.ToString();               //lấy ngày tháng năm hiện tại
            string getNowYear = nk.Substring(17);           //cắt lấy 4 số cuối năm
            txtNienKhoa.Text = getNowYear;                  //gán cho txtNienKhoa 4 số cuối của năm
        }

        //hàm auto id sinh viên
        public void AutoIDSinhVien()
        {
            string LastIdKhoas = cboChonKhoasHocSV.SelectedValue.ToString().Substring(1);                //lấy 2 số cuối mã khóa
            string LastIdLop = maLopSV.Substring(8);                                                                             //lấy 2 số cuối mã lớp
            int SLSV = sinhVienService.CountSinhVien();                                                                         //đếm số lượng sinh viên có trong data

            if (SLSV < 10)
            {
                lblMaSV.Text = "GD" + LastIdKhoas + LastIdLop + "000" + SLSV;
            }
            else if(SLSV < 100)
            {
                lblMaSV.Text = "GD" + LastIdKhoas + LastIdLop + "00" + SLSV;
            }
            else if(SLSV < 1000)
            {
                lblMaSV.Text = "GD" + LastIdKhoas + LastIdLop + "0" + SLSV;
            }
            else if (SLSV < 10000)
            {
                lblMaSV.Text = "GD" + LastIdKhoas + LastIdLop  + SLSV;
            }
        }

        //đếm số thứ tự danh sách khoa
        public void CountRowsKhoa()
        {
            int countRowsKhoa = dgvDanhSachKhoa.Rows.Count;
            for(int i = 0; i < countRowsKhoa; i++)
            {
                dgvDanhSachKhoa.Rows[i].Cells[0].Value = (i + 1);
            }
        }

        // đếm số thứ tự danh sách khóa học 
        public void CountRowsKhoasHoc()
        {
            int countRowsKhoasHoc = dgvDanhSachKhoasHoc.Rows.Count;
            for (int i = 0; i < countRowsKhoasHoc; i++)
            {
                dgvDanhSachKhoasHoc.Rows[i].Cells[0].Value = (i + 1);
            }
        }

 

        //đếm số thứ tự danh sách sinh viên
        public void CountRowsSinhVien()
        {
            int countRowsSinhVien = dgvDanhSachSinhVien.Rows.Count;
            for (int i = 0; i < countRowsSinhVien; i++)
            {
                dgvDanhSachSinhVien.Rows[i].Cells[0].Value = (i + 1);
            }
        }

        //check data khoa trước khi xóa
        public void checkDataDeleteKhoa()
        {
            string[] nganh;
            string[] khoa;
            var listNganh = nganhHocService.GetAllNganhHoc();
            foreach (var ng in listNganh)
            {
                if (ng.MaKhoa == lblMaKhoaKN.Text)
                {
                    btnDeleteKhoa.Enabled = false;
                }
                else
                {
                    btnDeleteKhoa.Enabled = true;
                }
            }
        }

        //check data khóa học trước khi xóa
        public void CheckDataDeleteKhoasHoc()
        {
            var listLop = lopService.GetAllLop();
            foreach(var lp in listLop)
            {
                if(lp.MaKhoaHoc == lblMaKhoaHocKL.Text)
                {
                    btnDeleteKhoaHoc.Enabled = false;
                }
                else
                {
                    btnDeleteKhoa.Enabled = true;
                }
            }
        }
        //-------------------------KẾT THÚC DS HÀM PUBLIC------------------------------//
        //______________________________________________________//
        private void timerTime_QLSV_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblTimeKL.Text = DateTime.Now.ToLongTimeString();

            int giay_KN = Convert.ToInt32(lblGiay_QLK.Text);
            int phut_KN = Convert.ToInt32(lblPhut_QLK.Text);

            int giay_KL = Convert.ToInt32(lblGiay_QLKH.Text);
            int phut_KL = Convert.ToInt32(lblPhut_QLKH.Text);
            giay_KN++;
            giay_KL++;
            if (giay_KN > 59 & giay_KL > 59)
            {
                giay_KN = 0;
                phut_KN++;

                giay_KL = 0;
                phut_KL++;
            }

            //dem gio
            if (giay_KN < 10  & giay_KL < 10)
            {
                lblGiay_QLK.Text = "0" + giay_KN;
                lblGiay_QLKH.Text = "0" + giay_KL;
            }
            else
            {
                lblGiay_QLK.Text = "" + giay_KN;
                lblGiay_QLKH.Text = "" + giay_KL;
            }
            if (phut_KN < 10 & phut_KL < 10)
            {
                lblPhut_QLK.Text = "0" + phut_KN;
                lblPhut_QLKH.Text = "0" + phut_KL;
            }
            else
            {
                lblPhut_QLK.Text = "" + phut_KN;
                lblPhut_QLKH.Text = "" + phut_KL;
            }

            //tao chuyen dong animation
            if (giay_KN % 2 == 0  & giay_KL % 2 == 0)
            {
                lblAnimation1_KN.Text = "(^_^)";
                lblAnimation2_KN.Text = "(+_+)";
                lblAnimation3_KN.Text = "(-_^)";

                lblAnimation1_KL.Text = "(^_^)";
                lblAnimation2_KL.Text = "(+_+)";
                lblAnimation3_KL.Text = "(-_^)";
            }
            else if (giay_KN % 2 != 0 & giay_KL % 2 != 0)
            {
                lblAnimation1_KN.Text = "(+_+)";
                lblAnimation2_KN.Text = "(^_^)";
                lblAnimation3_KN.Text = "(^_-)";

                lblAnimation1_KL.Text = "(+_+)";
                lblAnimation2_KL.Text = "(^_^)";
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
                string maNganhKL = cboChonNganhKL.SelectedValue.ToString();
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
                LoadDataToCombox();
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
            checkDataDeleteKhoa();
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
                LoadDataToCombox();
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
                    //-------------------check data-----------------//




                    //--------------------Xóa Khoa--------------//
                    khoaService.DeleteKhoa(maKhoa);
                    lblMaKhoaKN.Text = "null";
                    txtTenKhoa.Text = "";
                    btnNewKhoa.Enabled = true;
                    btnSaveKhoa.Enabled = false;
                    btnUpdateKhoa.Enabled = false;
                    btnDeleteKhoa.Enabled = false;
                    LoadDanhSachKhoaToDatagridview();
                    LoadDataToCombox();
                    MessageBox.Show("Đã Xóa...!!!", "Thông Báo", MessageBoxButtons.OK);
                }
            }
        }

        private void dgvDanhSachKhoa_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvDanhSachKhoa.Rows.Count == 0)
            {
                dgvDanhSachKhoa.Enabled = false;
                btnSaveKhoa.Enabled = false;
                btnUpdateKhoa.Enabled = false;
                btnDeleteKhoa.Enabled = false;
            }
            else
            {
                txtTenKhoa.Focus();
                btnSaveKhoa.Enabled = false;
                btnUpdateKhoa.Enabled = true;
                btnDeleteKhoa.Enabled = true;
                ShowDataTuDataGridViewToTextBox();
                checkDataDeleteKhoa();
            }
        }

        private void cboChonKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboChonNganhKL.Text = "";
            string maKhoa = cboChonKhoaKL.SelectedValue.ToString();
            cboChonNganhKL.DataSource = nganhHocService.GetNganhHocByKHOA(maKhoa);
            cboChonNganhKL.DisplayMember = "TenNganh";
            cboChonNganhKL.ValueMember = "MaNganh";
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

                KhoasHoc khoaHoc = new KhoasHoc();
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
                KhoasHoc khoaHoc = new KhoasHoc();
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
            if(dgvDanhSachKhoasHoc.Rows.Count == 0)
            {
                dgvDanhSachKhoasHoc.Enabled = false;
                btnUpdateKhoaHoc.Enabled = false;
                btnDeleteKhoaHoc.Enabled = false;
            }
            else
            {
                ShowDataTuDataGridViewToTextBox();
                btnSaveKhoaHoc.Enabled = false;
                btnUpdateKhoaHoc.Enabled = true;
                btnDeleteKhoaHoc.Enabled = true;
                nubNienKhoaKL.Enabled = false;
            }
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
                    MessageBox.Show("Đã Xóa ["+lblMaKhoaHocKL.Text+"]", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            cboChonNganhSV.Text = "";
            string maKhoa = cboChonKhoaSV.SelectedValue.ToString();
            cboChonNganhSV.DataSource = nganhHocService.GetNganhHocByKHOA(maKhoa);
            cboChonNganhSV.DisplayMember = "TenNganh";
            cboChonNganhSV.ValueMember = "MaNganh";
        }

        private void cboChonNganhSV_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboChonKhoasHocSV.Text = "";
            cboChonKhoasHocSV.DataSource = khoaHocService.GetAllKhoaHoc();
            cboChonKhoasHocSV.DisplayMember = "TenKhoaHoc";
            cboChonKhoasHocSV.ValueMember = "MaKhoaHoc";
        }

        private void cboChonKhoaHocSV_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cboChonNganhSV.Text=="")
            {
                MessageBox.Show("Danh Sách Lớp Null", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                LoadDanhSachLopHocToTreeview();
            } 
        }

        private void trvDSLop_Click(object sender, EventArgs e)
        {

        }

        private void trvDSLop_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            dgvDanhSachSinhVien.Enabled = true;
            lblMaSV.Text = "null";
            maLopSV = e.Node.Name;
            lblTenLop.Text = e.Node.Text;
            LoadDanhSachSinhVienToDgv();
            txtTenSV.Clear();
            txtEmail.Clear();
            txtSdt.Clear();
            rtxtDiaChi.Clear();
            rtxtGhiChu.Clear();
            btnNewSV.Enabled = true;
            btnSaveSV.Enabled = false;
            btnDeleteSV.Enabled = false;
            btnUpdateSV.Enabled = false;
        }

        private void dgvDanhSachSinhVien_MouseClick(object sender, MouseEventArgs e)
        {
            if (dgvDanhSachSinhVien.Rows.Count == 0)
            {
                dgvDanhSachSinhVien.Enabled = false;
            }
            else
            {
                ShowDataSinhVienTuDatagridview();
                btnSaveSV.Enabled = false;
                btnUpdateSV.Enabled = true;
                btnDeleteSV.Enabled = true;
                txtEmail.Enabled = false;
            }
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
                //gui email thong bao da add tai khoang;
                Contact contacts = new Contact();
                contacts = contactService.GetContact();
                string from = contacts.Email;
                string to = txtEmail.Text;
                string title = contacts.Title;
                string message = contacts.Message +"\n"+ contacts.Info;
                sendMessage.SendMaillToSinhVien(from, to, title, message);
                
                //thêm sinh viên
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
                LoadDanhSachSinhVienToDgv();
                MessageBox.Show("Thêm Thành Công...", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSaveSV.Enabled = false;
                btnUpdateSV.Enabled = true;
                btnDeleteSV.Enabled = true;

                //tao danh sach diem cho sinh vien
                List<MonHoc> listMonHoc = monHocService.GetMonHocByNganh(cboChonNganhSV.SelectedValue.ToString()).ToList();
                foreach (var mh in listMonHoc)
                {
                    string maMon = mh.MaMonHoc;
                    DiemMonHoc diemMonHoc = new DiemMonHoc();
                    diemMonHoc.MaSV = lblMaSV.Text;
                    diemMonHoc.MaMonHoc = maMon;
                    diemMonHoc.Diem30 = null;
                    diemMonHoc.Diem70L1 = null;
                    diemMonHoc.Diem70L2 = null;
                    diemMonHoc.DTB = null;
                    diemMonHoc.Diem4 = null;
                    diemMonHoc.DiemSo = null;
                    diemMonHoc.DiemChu = null;
                    diemMonHoc.GhiChu = null;
                    diemMonHocService.AddDiemMonHoc(diemMonHoc);
                }
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
            txtEmail.Enabled = true;
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
                sinhVienService.UpdateSinhVien(sv);
                LoadDanhSachSinhVienToDgv();

                //gửi mail thông báo cập nhật thành công
                //
                string other = "Thông Tin Sinh viên đã được cập nhật:";
                string maSV = "-Mã SV: "+lblMaSV.Text;
                string emailSV = "-Email: " + txtEmail.Text;
                string nameSV = "-Tên SV: " + txtTenSV.Text;
                string lopSV = "-Lớp: " + lblTenLop.Text;
                string sdtSV = "-SĐT: " + txtSdt.Text;
                string dcSV = "-Địa Chỉ: " + rtxtDiaChi.Text;
                //
                Contact contacts = new Contact();
                contacts = contactService.GetContact();
                string from = contacts.Email;
                string to = txtEmail.Text;
                string title = "Future Team - Thông Báo Cập NHật thông tin sinh viên";
                string message = other + "\n" + maSV + "\n" + nameSV + "\n" + emailSV + "\n" + lopSV + "\n" + sdtSV + "\n" + dcSV + "\n" + contacts.Info;
                sendMessage.SendMaillToSinhVien(from, to, title, message);

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
                    diemMonHocService.DeleteAllDiemMonHocByMaSinhVien(lblMaSV.Text);
                    sinhVienService.DeleteSinhVien(maSV);
                    LoadDanhSachSinhVienToDgv();
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

        private void dgvDanhSachKhoa_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            frmDanhSachNganh frmDSNganh = new frmDanhSachNganh();
            SendMaKhoaToFrmDanhSachKhoa sendMaKhoa = new SendMaKhoaToFrmDanhSachKhoa(frmDSNganh.FunData);
            sendMaKhoa(this.lblMaKhoaKN);
            frmDSNganh.ShowDialog();
        }

        private void dgvDanhSachKhoasHoc_MouseDoubleClick(object sender, MouseEventArgs e)
        {
                frmDanhSachLop frmDSLop = new frmDanhSachLop();
                SendMaKhoaHocMaNganhToFrmDanhSachLop senMaKhoaHocMaNganh = new SendMaKhoaHocMaNganhToFrmDanhSachLop(frmDSLop.FunDatafrmDanhSachLopToFrmQLSV);
                string maNganhKL = cboChonNganhKL.SelectedValue.ToString();
                string maKhoaHocKL = lblMaKhoaHocKL.Text.Trim();
                senMaKhoaHocMaNganh(maNganhKL, maKhoaHocKL);
                frmDSLop.ShowDialog();
        }
    }
}
