using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GDU_Management.Model;
using GDU_Management.Service;
using GDU_Management.Controller;
using GDU_Management.GDU_Views;

namespace GDU_Management
{
    public partial class frmCheckAdmin : Form
    {
        public frmCheckAdmin()
        {
            InitializeComponent();
        }
        //delegate
        delegate void SendEmailToFrmOption(string emailAdmin);

        //service 
        CheckAccountService checkAccountService = new CheckAccountService();
        ContactService contactService = new ContactService();
        AdminService adminService = new AdminService();
        

        //controller
        RandomCodeControlller rd = new RandomCodeControlller();
        SendMessageController sendMessage = new SendMessageController();

        //public value
        string _ID;
        string _email;
        string _VerificationCode;
        int countVerification = 0;


        //---------------------------DANH SÁCH HÀM PUBLIC------------------------------//
        //__________________________________________________________//

        //hàm fundata nhận email admin
        public void FunDataChkAcc(string emailAd)
        {
            _email = emailAd;
        }

        //hàm tạo mã xác nhận r lưu vào database
        public void CreateVerificationCode()
        {
            _VerificationCode = rd.VerificationCode();
            CheckAccount chkAcc = new CheckAccount();
            chkAcc.MaAdmin = lblIdAdmin.Text.Trim();
            chkAcc.Code = _VerificationCode;
            checkAccountService.CheckAcc(chkAcc);
        }


        //gửi mã xác nhận đến admin
        public void SendVerificationCodeToAdmin()
        {
            CreateVerificationCode();
            InforContact contacts = new InforContact();
            contacts = contactService.InfoContact("5");
            string fromEmail = contacts.Email;
            string toEmail = _email;
            string subEmail = contacts.Subject;
            string messEmail = contacts.Message + "\n";
            string code = "-------------------" + "\n" + _VerificationCode + "\n" + "-------------------";
            sendMessage.SendVerificationCode(fromEmail, toEmail,subEmail,messEmail+code);
        }


        public void LoadAdmin()
        {
            Admin ad = new Admin();
            ad = adminService.GetAdminByEmail(_email);
            lblIdAdmin.Text = ad.MaAdmin;
            lblNameAdmin.Text = ad.TenAdmin; 
        }

        public void LockAccount()
        {
            Admin ad = new Admin();
            ad = adminService.GetAdminByEmail(_email);
            ad.MaAdmin = ad.MaAdmin;
            ad.StatusAcc = "Lock";
            adminService.UpdateStatusAccountByEmail(ad);
            MessageBox.Show("Tài khoản admin đã bị khóa vì xác thực SAI quá " + (countVerification - 1) + " lần." + "\n" + "Thoát Chương Trình", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            checkAccountService.DeleteVerificationCode();
            Application.Exit();
        }
        //-------------------------KẾT THÚC DS HÀM PUBLIC------------------------------//
        //_________________________________________________________//

        private void frmCheckAdmin_Load(object sender, EventArgs e)
        {
            LoadAdmin();
            SendVerificationCodeToAdmin();
            btnGoOnForm.Enabled = false;
        }

        private void lblGuiLaiMatKhau_MouseHover(object sender, EventArgs e)
        {
            this.lblGuiLaiMaXacNhan.ForeColor = Color.Blue;
        }

        private void lblGuiLaiMatKhau_MouseLeave(object sender, EventArgs e)
        {
            this.lblGuiLaiMaXacNhan.ForeColor = Color.Black;
        }

        private void lblGuiLaiMaXacNhan_Click(object sender, EventArgs e)
        {
            checkAccountService.DeleteVerificationCode();
            SendVerificationCodeToAdmin();
        }

        private void btnGoOnForm_Click(object sender, EventArgs e)
        {
            List<CheckAccount> listAcc = new List<CheckAccount>();
            listAcc = checkAccountService.GetVerificationCode();
            foreach(var acc in listAcc)
            {
                if (acc.Code == txtVerificationCode.Text.Trim())
                {
                    this.Hide();
                    frmOptions frm_Opn = new frmOptions();
                    SendEmailToFrmOption sendEmail = new SendEmailToFrmOption(frm_Opn.FunDataOption);
                    sendEmail(_email);
                    checkAccountService.DeleteVerificationCode();
                    frm_Opn.ShowDialog();
                    break;
                }
                else
                {
                    countVerification++;
                    txtVerificationCode.Clear();
                    btnGoOnForm.Enabled = false;
                    MessageBox.Show("Mã xác thực không đúng. Vui lòng kiểm tra lại. "+"\n"+"Bạn đã xác thực "+countVerification
                        +" lần. Nếu vượt quá 3 lần tài khoản sẻ bị khóa","Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.lblNote.ForeColor = Color.Red;
                    lblNote.Text = "Mã Xác Thực không tồn tại. Còn "+(3-countVerification)+" lần xác thực trước khi tài khoản bị khóa.";
                    if(countVerification > 3)
                    {
                        LockAccount();
                    }
                }
            }
        }

        
        private void btnGoOnForm_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void btnGoOnForm_MouseHover(object sender, EventArgs e)
        {
            this.btnGoOnForm.ForeColor = Color.Blue;
        }

        private void txtVerificationCode_TextChanged(object sender, EventArgs e)
        {
            int lenghtCode = txtVerificationCode.Text.Length;
            if(lenghtCode == 8) 
            {
                btnGoOnForm.Enabled = true;
            }
            else
            {
                btnGoOnForm.Enabled = false;
            }
        }

        private void lblCloseCheckAcc_Click(object sender, EventArgs e)
        {
            this.Hide();
            GDUManagement gdu = new GDUManagement();
            gdu.ShowDialog();
        }

        private void btnGoOnForm_MouseLeave(object sender, EventArgs e)
        {
            this.btnGoOnForm.ForeColor = Color.DarkBlue;
        }
    }
}
