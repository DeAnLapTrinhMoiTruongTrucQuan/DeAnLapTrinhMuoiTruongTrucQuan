﻿using System;
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
        }

        //DS HÀM PUBLIC
        public void NgayGio()
        {
            //get ngày
            DateTime ngay = DateTime.Now;
            lblDay.Text = ngay.ToString("dd/MM/yyyy");
            lblDay2.Text = ngay.ToString("dd/MM/yyyy");

            //get thời gian
            timerTime_QLSV.Start();
        }

        //KẾT THÚC DS HÀM PUBLIC
        private void btnTroaiMenuChinhKhoa_Click(object sender, EventArgs e)
        {
            this.Hide();
            GDUManagement gdu = new GDUManagement();
            gdu.ShowDialog();
        }

        private void btnTroLaiMenuChinhNganh_Click(object sender, EventArgs e)
        {
            this.Hide();
            GDUManagement gdu = new GDUManagement();
            gdu.ShowDialog();
        }

        private void btnDSNganh_Click(object sender, EventArgs e)
        {
            frmDanhSachNganh frmDSNganh = new frmDanhSachNganh();
            frmDSNganh.ShowDialog();
        }

        private void btnXemDanhSachLop_Click(object sender, EventArgs e)
        {
            frmDanhSachLop frmDSLop = new frmDanhSachLop();
            frmDSLop.ShowDialog();
        }

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

    }
}
