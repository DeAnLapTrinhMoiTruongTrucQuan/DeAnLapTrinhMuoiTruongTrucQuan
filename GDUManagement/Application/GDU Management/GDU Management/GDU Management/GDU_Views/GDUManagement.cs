using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GDU_Management.GDU_Views;

namespace GDU_Management
{
    public partial class GDUManagement : Form
    {
        public GDUManagement()
        {
            InitializeComponent();
            getThoiGian_Animations();
        }
        private int khoangCach = 5;
        //DANH SÁCH HÀM PUBLIC

        //1-hàm cập nhật ngày giờ online, bộ điếm thời gian, animation
        public void getThoiGian_Animations()
        {
            //get day
            DateTime ngay =  DateTime.Now;
            lblDay.Text = ngay.ToString("dddd, dd-MM-yyyy");

            //get thời gian và bộ điếm 
            timerTime.Start();

            //get animation
            lblTitle.Text = "Hello Admin...!!! ^...^";
            timerChayChu.Start();
           
        }

        //4-hàm vào form QLSV
        public void goToQLSV()
        {
            this.Hide();
            frmQuanLySinhVien qlsv = new frmQuanLySinhVien();
                qlsv.ShowDialog();
        }

        //5-hàm vào form Môn Học - Điểm
        public void goToMonHoc_Diem()
        {
            this.Hide();
            frmDiemAndMonHoc monHoc_diem = new frmDiemAndMonHoc();
            monHoc_diem.ShowDialog();
        }

        //hàm vào form Giang Viên-TKB
        public void goToGiangVien_TKB()
        {
            this.Hide();
            frmGiaoVienAndTKB frmGv_tkb = new frmGiaoVienAndTKB();
            frmGv_tkb.ShowDialog();
        }

        public void goToPCCV()
        {
            this.Hide();
            frmPhanCongCongViec frmPccv = new frmPhanCongCongViec();
            frmPccv.ShowDialog();
        }
        // KẾT THÚC DS HÀM PUBLIC

        private void timerChayChu_Tick(object sender, EventArgs e)
        {
            //lệnh chạy animation Hello Admin
            lblTitle.Location = new Point(lblTitle.Location.X + khoangCach, lblTitle.Location.Y);
            if (lblTitle.Location.X > 477 || lblTitle.Location.X <= 0)
            {
                lblTitle.Location = new Point(lblTitle.Location.X - 477, lblTitle.Location.Y);
            }
        }

        private void timerTime_Tick(object sender, EventArgs e)
        {
            // lấy thời gian online
            lblTime.Text = DateTime.Now.ToLongTimeString();

            //setup bộ điếm thời gian bộ điếm hệ thống
            int gio = Convert.ToInt32(lblGio.Text);
            int giay = Convert.ToInt32(lblGiay.Text);
            int phut = Convert.ToInt32(lblPhut.Text);
            giay++;
            if (giay > 59)
            {
                giay = 0;
                phut++;
            }
            if (phut > 59)
            {
                phut = 0;
                gio++;
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
            if (gio < 10)
            {
                lblGio.Text = "0" + gio;
            }
            else
            {
                lblGio.Text = "" + gio;
            }
            if (giay % 2 == 0)
            {
                lblAnimation.Text = "(^_^)";
                lblAnimation2.Text = "(+_+)";
                lblAnimation3.Text = "(-_^)";
            }
            else if (giay % 2 != 0)
            {
                lblAnimation2.Text = "(^_^)";
                lblAnimation.Text = "(+_+)";
                lblAnimation3.Text = "(^_-)";
            }
        }

        private void GDUManagement_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnThoatChuongTrinh_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Bạn có muốn thoát khỏi chương trình không ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Bạn có muốn Đăng Xuất khỏi chương trình không ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                // Application.Exit();
                this.Hide();
                frmLogin frmlogin = new frmLogin();
                frmlogin.ShowDialog();
            }       
        }

        private void pnQuanLySinhVien_MouseClick(object sender, MouseEventArgs e)
        {
            goToQLSV();
        }
        private void lblIconQLSV_Click(object sender, EventArgs e)
        {
            goToQLSV();
        }
        private void lblQLSV_Click_1(object sender, EventArgs e)
        {
            goToQLSV();
        }

        private void pnMonHoc_Diem_MouseClick(object sender, MouseEventArgs e)
        {
            goToMonHoc_Diem();
        }
        private void lblIconMonHoc_Diem_Click(object sender, EventArgs e)
        {
            goToMonHoc_Diem();
        }
        private void lblDSV_Click(object sender, EventArgs e)
        {
            goToMonHoc_Diem();
        }


        private void pnThoiKhoaBieu_MouseClick(object sender, MouseEventArgs e)
        {
            goToGiangVien_TKB();
        }
        private void lblGV_TKB_Click(object sender, EventArgs e)
        {
            goToGiangVien_TKB();
        }
        private void lblIconGV_TKB_Click(object sender, EventArgs e)
        {
            goToGiangVien_TKB();
        }

        private void pnPhanCongCongViec_MouseClick(object sender, MouseEventArgs e)
        {
            goToPCCV();
        }

        private void lblIconPhanCongCongViec_Click(object sender, EventArgs e)
        {
            goToPCCV();
        }

        private void lblPhanCongCongViec_Click(object sender, EventArgs e)
        {
            goToPCCV();
        }

        private void lblLogoGDU_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Đây là phần dành cho admin bạn cần phải xác minh trước khi bắt đầu truy cập", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                frmOptions frm_opt = new frmOptions();
                frm_opt.ShowDialog();
            }
        }

    }
}
