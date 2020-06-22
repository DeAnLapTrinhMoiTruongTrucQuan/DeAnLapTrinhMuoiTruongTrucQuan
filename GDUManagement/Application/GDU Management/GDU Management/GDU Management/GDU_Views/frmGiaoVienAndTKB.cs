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
    public partial class frmGiaoVienAndTKB : Form
    {
        public frmGiaoVienAndTKB()
        {
            InitializeComponent();
            timerGiangvien_TKB.Start();
            btnSaveGV.Enabled = false;
            btnUpdateGV.Enabled = false;
            btnDeleteGV.Enabled = false;
        }
        GDUDataConnectionsDataContext db = new GDUDataConnectionsDataContext();
        //CÁC HÀM PUBLIC DÙNG TRONG FORM
        //1-hàm trở về Menu Chính - GDU Management
        public void goHomeMenu()
        {
            this.Hide();
            GDUManagement gdu =new GDUManagement();
            gdu.ShowDialog();
        }


        //Khai báo Service
        GiangVienService giangVienService = new GiangVienService();
        KhoaService khoaService = new KhoaService();
        NganhHocService nganhHocService = new NganhHocService();
        KhoaHocService khoaHocService = new KhoaHocService();
        LopService lopHocService = new LopService();
        //ThoiKhoaBieu thoiKhoaBieuService = new ThoiKhoaBieu();
        MonHocService monHocService = new MonHocService();
        ThoiKhoaBieuService thoiKhoaBieuService = new ThoiKhoaBieuService();


        public void LoadDanhSachGiangVienToDatagridview()
        {
            dataGridView1.DataSource = giangVienService.GetAllGiangVien().ToList();
        }

       

        public void ShowDataFromDatagridviewToTextbox()
        {
            txtMaGV.DataBindings.Clear();
            txtMaGV.DataBindings.Add("text", dataGridView1.DataSource, "MaGV");

            txtTenGV.DataBindings.Clear();
            txtTenGV.DataBindings.Add("text", dataGridView1.DataSource, "TenGV");

            cboGioiTinh.DataBindings.Clear();
            cboGioiTinh.DataBindings.Add("text", dataGridView1.DataSource, "GioiTinh");

            cboTrinhDo.DataBindings.Clear();
            cboTrinhDo.DataBindings.Add("text", dataGridView1.DataSource, "TrinhDo");

            txtSDT.DataBindings.Clear();
            txtSDT.DataBindings.Add("text", dataGridView1.DataSource, "SDT");

            txtEmailGV.DataBindings.Clear();
            txtEmailGV.DataBindings.Add("text", dataGridView1.DataSource, "Email");

            rtxtDiaChi.DataBindings.Clear();
            rtxtDiaChi.DataBindings.Add("text", dataGridView1.DataSource, "DiaChi");

            rtxtGhiChu.DataBindings.Clear();
            rtxtGhiChu.DataBindings.Add("text", dataGridView1.DataSource, "GhiChu");


        }

       // Load data to combobox
        public void LoadDataToCombox()
        {
            //Cbo Khoa
            cboChonKhoa.DataSource = khoaService.GetAllKhoa();
            cboChonKhoa.DisplayMember = "TenKhoa";
            cboChonKhoa.ValueMember = "MaKhoa";

            //Cbo GiangVien
            cboGiangVien.DataSource = giangVienService.GetAllGiangVien();
            cboGiangVien.DisplayMember = "TenGV";
            cboGiangVien.ValueMember = "MaGV";

            //Cbo Phòng

            cboPhong.DataSource = thoiKhoaBieuService.GetAllTKB();
            cboPhong.DisplayMember = "PhongHoc";
            cboPhong.ValueMember = "MaTKB";

        }


        //Load danh sách lớp học vào treeview 
        public void LoadDanhSachLopHocToTreeview()
        {
            TreeNode root = new TreeNode("Danh Sách Lớp", 0, 0);
            root.Tag = 0;

            string maNganh = cboChonNganh.SelectedValue.ToString();

            string maKhoaHoc = cboChonKhoaHoc.SelectedValue.ToString();

            GDUDataConnectionsDataContext db = new GDUDataConnectionsDataContext();
            var listLp = from x in db.Lops where x.MaNganh == maNganh && x.MaKhoaHoc == maKhoaHoc select x;
            trvClass.Nodes.Clear();
            foreach (var cla in listLp)
            {
                TreeNode treeNode = new TreeNode(cla.TenLop, 1, 1);
                treeNode.Tag = cla.MaLop;
                root.Nodes.Add(treeNode);
            }
            trvClass.Nodes.Add(root);
            trvClass.ExpandAll();
        }

        //hàm check data 

        public bool checkDataGV()
        {
            if (string.IsNullOrEmpty(txtMaGV.Text))
            {
                MessageBox.Show("Mã GV không được bỏ trống!");
                txtMaGV.Focus();
                return false;
            }
            return true;
        }

        public bool checkDataTKB()
        {
            if (string.IsNullOrEmpty(cboHoKi.Text))
            {
                MessageBox.Show("Học kì không được bỏ trống!");
                cboHoKi.Focus();
                return false;
            }
            return true;
        }


        //KẾT THÚC CÁC HÀM PUBLIC DÙNG TRONG FORM
        private void timerGiangvien_TKB_Tick(object sender, EventArgs e)
        {
            //hàm lấy ngày giờ
            lblTime.Text = DateTime.Now.ToLongTimeString();
            //lblDay.Text = DateTime.Now.ToLongDateString();
            DateTime ngay = DateTime.Now;
            lblDay.Text = ngay.ToString("dd/MM/yyyy");

            //hàm bộ điếm thời gian online
            int giay_gv = Convert.ToInt32(lblGiay_gv.Text);
            int phut_gv = Convert.ToInt32(lblPhut_gv.Text);
            giay_gv++;

            int giay_tkb = Convert.ToInt32(lblGiay_tkb.Text);
            int phut_tkb = Convert.ToInt32(lblPhut_tkb.Text);
            giay_tkb++;

            if (giay_gv > 59 & giay_tkb > 59)
            {
                giay_gv = 0;
                phut_gv++;

                giay_tkb = 0;
                phut_tkb++;
            }
            
            if (giay_gv < 10 & giay_tkb < 10)
            {
                lblGiay_gv.Text = "0" + giay_gv;
                lblGiay_tkb.Text = "0" + giay_tkb;
            }
            else
            {
                lblGiay_gv.Text = "" + giay_gv;
                lblGiay_tkb.Text = "" + giay_tkb;
            }
            if (phut_gv < 10  & phut_tkb < 10)
            {
                lblPhut_gv.Text = "0" + phut_gv;
                lblPhut_tkb.Text = "0" + phut_tkb;
            }
            else
            {
                lblPhut_gv.Text = "" + phut_gv;
                lblPhut_tkb.Text = "" + phut_tkb;
            }
          
            if (giay_gv % 2 == 0  & giay_tkb % 2 == 0)
            {
                lblAnimation1_gv.Text = "(^_^)";
                lblAnimation2_gv.Text = "(+_+)";
                lblAnimation3_gv.Text = "(-_^)";

                lblAnimation1_tkb.Text = "(^_^)";
                lblAnimation2_tkb.Text = "(+_+)";
                lblAnimation3_tkb.Text = "(-_^)";
            }
            else if (giay_gv % 2 != 0  & giay_tkb % 2 != 0)
            {
                lblAnimation1_gv.Text = "(+_+)";
                lblAnimation2_gv.Text = "(^_^)";
                lblAnimation3_gv.Text = "(^_-)";

                lblAnimation1_tkb.Text = "(+_+)";
                lblAnimation2_tkb.Text = "(^_^)";
                lblAnimation3_tkb.Text = "(^_-)";
            }
        }

        private void btnHomTKB_Click(object sender, EventArgs e)
        {
            goHomeMenu();
        }

        private void btnHomeGV_Click(object sender, EventArgs e)
        {
            goHomeMenu();
        }

        private void btnExit_TKB_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Bạn có muốn thoát khỏi chương trình không ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnExit_GV_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Bạn có muốn thoát khỏi chương trình không ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            btnSaveGV.Enabled = false;
            btnUpdateGV.Enabled = true;
            btnDeleteGV.Enabled = true;
            ShowDataFromDatagridviewToTextbox();
        }

        private void frmGiaoVienAndTKB_Load(object sender, EventArgs e)
        {
            LoadDanhSachGiangVienToDatagridview();
            LoadDataToCombox();
            txtMaGV_TKB.Clear();

            //treeview mẫu
            TreeNode root = new TreeNode("Danh Sách Lớp", 0, 0);
            root.Tag = 0;
            foreach (var cla in db.Lops)
            {
                TreeNode child = new TreeNode(cla.TenLop, 1, 1);
                child.Tag = cla.MaLop;
                root.Nodes.Add(child);
            }
            trvClass.Nodes.Add(root);

            trvClass.ExpandAll();
            txtSTC.Enabled = true;

        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            btnSaveGV.Enabled = false;
            btnUpdateGV.Enabled = true;
            btnDeleteGV.Enabled = true;
            txtMaGV.Enabled = false;
            //Loi chua khac phuc

            dateTimePicker3.Value = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
            ShowDataFromDatagridviewToTextbox();
        }

        private void btnNewGV_Click(object sender, EventArgs e)
        {
            btnSaveGV.Enabled = true;
            btnUpdateGV.Enabled = false;
            btnDeleteGV.Enabled = false;
            txtMaGV.Enabled = true;
            txtMaGV.Text = "";
            txtTenGV.Text = "";
            cboGioiTinh.Text = "";
            cboTrinhDo.Text = "";
            txtEmailGV.Text = "";
            txtSDT.Text = "";
            rtxtDiaChi.Text = "";
            rtxtGhiChu.Text = "";
        }

        private void btnSaveGV_Click(object sender, EventArgs e)
        {
            if (checkDataGV())
            {
                GiangVien giangVien = new GiangVien();
                giangVien.MaGV = txtMaGV.Text.Trim();
                giangVien.TenGV = txtTenGV.Text.Trim();
                giangVien.GioiTinh = cboGioiTinh.Text.Trim();
                giangVien.TrinhDo = cboTrinhDo.Text.Trim();
                giangVien.Email = txtEmailGV.Text.Trim();
                giangVien.SDT = txtSDT.Text.Trim();
                giangVien.GhiChu = rtxtGhiChu.Text.Trim();
                giangVien.DiaChi = rtxtDiaChi.Text.Trim();
                giangVien.Password = txtMaGV.Text.Trim();

                giangVienService.CreateGiangVien(giangVien);
                LoadDanhSachGiangVienToDatagridview();
                MessageBox.Show("Thêm thành công!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSaveGV.Enabled = false;
            }
            else
            {
                MessageBox.Show("Lỗi, Thêm thất bại!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateGV_Click(object sender, EventArgs e)
        {
            GiangVien gv = new GiangVien();
            gv.MaGV = txtMaGV.Text;
            gv.TenGV = txtTenGV.Text;
            gv.GioiTinh = cboGioiTinh.Text;
            gv.TrinhDo = cboTrinhDo.Text;
            gv.Email = txtEmailGV.Text;
            gv.SDT = txtSDT.Text;
            gv.GhiChu = rtxtGhiChu.Text;
            gv.DiaChi = rtxtDiaChi.Text;

            giangVienService.UpdateGiangVien(gv);
            MessageBox.Show("Cập nhật thông tin '" + txtMaGV.Text + "' Thành Công ", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadDanhSachGiangVienToDatagridview();
        }

        private void btnDeleteGV_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn Có Muốn Xóa '" + txtMaGV.Text + "' ?", "THÔNG BÁO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string maGiangVien;
                maGiangVien = txtMaGV.Text.Trim();
                if (string.IsNullOrEmpty(txtMaGV.Text))
                {
                    MessageBox.Show("Xóa Thất Bại", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    giangVienService.DeleteGiangVien(maGiangVien);
                    txtMaGV.Text = "";
                    txtTenGV.Text = "";
                    cboGioiTinh.Text = "";
                    cboTrinhDo.Text = "";
                    txtEmailGV.Text = "";
                    txtSDT.Text = "";
                    rtxtDiaChi.Text = "";
                    rtxtGhiChu.Text = "";
                    btnNewGV.Enabled = true;
                    btnSaveGV.Enabled = false;
                    btnUpdateGV.Enabled = false;
                    btnDeleteGV.Enabled = false;
                    LoadDanhSachGiangVienToDatagridview();
                    MessageBox.Show("Đã xóa!", "THÔNG BÁO", MessageBoxButtons.OK);
                }
            }
        }

        private void txtTimKiemGV_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = giangVienService.SearchGiangVienByTenGV(txtTimKiemGV.Text).ToList();
        }

        private void label34_Click(object sender, EventArgs e)
        {

        }

        private void cboChonKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            string maKhoa = cboChonKhoa.SelectedValue.ToString();
            cboChonNganh.DataSource = nganhHocService.GetNganhHocByKHOA(maKhoa);
            cboChonNganh.DisplayMember = "TenNganh";
            cboChonNganh.ValueMember = "MaNganh";
        }

        private void cboChonNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboChonKhoaHoc.DataSource = khoaHocService.GetAllKhoaHoc();
            cboChonKhoaHoc.DisplayMember = "TenKhoaHoc";
            cboChonKhoaHoc.ValueMember = "MaKhoaHoc";

            string maNganh = cboChonNganh.SelectedValue.ToString();
            cboMonHoc.DataSource = monHocService.GetMonHocByMaNganh(maNganh);
            cboMonHoc.DisplayMember = "TenMonHoc";
            cboMonHoc.ValueMember = "MaMonHoc";

            txtMaMH.Clear();
        }

        private void cboChonKhoaHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDanhSachLopHocToTreeview();
        }

        private void trvClass_AfterSelect(object sender, TreeViewEventArgs e)
        {
            lstTKB.Items.Clear();
            string depid = e.Node.Tag.ToString();

            var result = from stu in db.ThoiKhoaBieus
                         where stu.MaLop == depid
                         select new
                         {
                             stu.MaMonHoc,
                             stu.TenMonHoc,
                             stu.HocKi,
                             stu.MaGV,
                             stu.TenGV,
                             stu.ThoiGianHoc,
                             stu.NgayBatDau,
                             stu.NgayKetthuc,
                             stu.PhongHoc,
                             stu.STC,
                             stu.Thu,
                         };
            foreach (var stu in result)
            {
                ListViewItem item = new ListViewItem(stu.MaMonHoc);
                item.SubItems.Add(stu.TenMonHoc);
                item.SubItems.Add(stu.HocKi);
                item.SubItems.Add(stu.MaGV);
                item.SubItems.Add(stu.TenGV);
                item.SubItems.Add(stu.ThoiGianHoc);
                item.SubItems.Add(stu.STC.Value.ToString());
                item.SubItems.Add(stu.NgayBatDau);
                item.SubItems.Add(stu.NgayKetthuc);
                item.SubItems.Add(stu.PhongHoc);
                item.SubItems.Add(stu.Thu);
                lstTKB.Items.Add(item);
            }
        }

        private void cboMonHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            string maMonHoc = cboMonHoc.SelectedValue.ToString();
            txtMaMH.Clear();
            txtMaMH.SelectedText = maMonHoc;

        }

        private void lstTKB_MouseClick(object sender, MouseEventArgs e)
        {
            string mamh = lstTKB.SelectedItems[0].SubItems[0].Text;
            string tenmh = lstTKB.SelectedItems[0].SubItems[1].Text;
            string hocki = lstTKB.SelectedItems[0].SubItems[2].Text;
            string magv = lstTKB.SelectedItems[0].SubItems[3].Text;
            string tengv = lstTKB.SelectedItems[0].SubItems[4].Text;
            string tghoc = lstTKB.SelectedItems[0].SubItems[5].Text;
            string stc = lstTKB.SelectedItems[0].SubItems[6].Text;
            dateTimePicker1.Value = DateTime.Parse(lstTKB.SelectedItems[0].SubItems[7].Text);
            dateTimePicker2.Value = DateTime.Parse(lstTKB.SelectedItems[0].SubItems[8].Text);
            string phong = lstTKB.SelectedItems[0].SubItems[9].Text;
            string thu = lstTKB.SelectedItems[0].SubItems[10].Text;


            txtMaMH.Text = mamh;
            cboMonHoc.Text = tenmh;
            cboHoKi.Text = hocki;
            txtMaGV_TKB.Text = magv;
            cboGiangVien.Text = tengv;
            cboThoiGian.Text = tghoc;
            txtSTC.Text = stc;
            cboPhong.Text = phong;
            cboThu.Text = thu;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            cboHoKi.Text = "";
            cboThu.Text = "";
            txtMaMH.Text = "";
            cboMonHoc.Text = "";
            txtSTC.Text = "";
            cboThoiGian.Text = "";
            txtMaGV_TKB.Text = "";
            cboGiangVien.Text = "";
            cboPhong.Text = "";
            dateTimePicker1.Text = "";
            dateTimePicker2.Text = "";
            txtMaMH.Enabled = false;
            txtMaGV_TKB.Enabled = false;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(checkDataTKB())
            {
                ThoiKhoaBieu thoiKhoaBieu = new ThoiKhoaBieu();
                thoiKhoaBieu.MaMonHoc = txtMaMH.Text.Trim();
                thoiKhoaBieu.HocKi = cboHoKi.Text.Trim();
                thoiKhoaBieu.Thu = cboThu.Text.Trim();
                // thoiKhoaBieu.STC = txtSTC.Text.Trim();
                thoiKhoaBieu.ThoiGianHoc = cboThoiGian.Text.Trim();
                thoiKhoaBieu.TenGV = cboGiangVien.Text.Trim();
                thoiKhoaBieu.MaGV = txtMaGV_TKB.Text.Trim();
                thoiKhoaBieu.PhongHoc = cboPhong.Text.Trim();
                thoiKhoaBieu.NgayBatDau = dateTimePicker1.Text.Trim();
                thoiKhoaBieu.NgayKetthuc = dateTimePicker2.Text.Trim();


               
                //thoiKhoaBieuService.CreateThoiKhoaBieu(thoiKhoaBieu);
                //MessageBox.Show("Thêm thành công!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;


            }

        }

        private void cboGiangVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            string maGV = cboGiangVien.SelectedValue.ToString();
            txtMaGV_TKB.Clear();
            txtMaGV_TKB.SelectedText = maGV;
        }
    }
}
