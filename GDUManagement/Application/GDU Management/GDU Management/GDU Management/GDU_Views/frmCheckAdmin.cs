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
    public partial class frmCheckAdmin : Form
    {
        public frmCheckAdmin()
        {
            InitializeComponent();
        }

        private void btnTiepTuc_Click(object sender, EventArgs e)
        {
            string checkText = txtKetQua.Text;
            if (checkText.Equals("Không"))
            {
                MessageBox.Show("Hay Lắm, đúng r", "Đúng Rồi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                frmCheckAdmin2 frm_chk2 = new frmCheckAdmin2();
                frm_chk2.Show();
            }
            else
            {
                MessageBox.Show("M thông minh lắm thằng ngu ạ, Nhưng sai r", "Sai Rồi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTroLai_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đéo đủ trình mà cùng đòi vô đây, Cút đê", "Cút Đê", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Hide();
            GDUManagement gdu = new GDUManagement();
            gdu.ShowDialog();
        }
    }
}
