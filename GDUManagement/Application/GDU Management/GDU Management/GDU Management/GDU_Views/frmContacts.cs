using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GDU_Management.Service;
using GDU_Management.Model;
using System.Net;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;
using FluentValidation.Validators;


namespace GDU_Management.GDU_Views
{
    public partial class frmContacts : Form
    {
        public frmContacts()
        {
            InitializeComponent();
        }

        //khai báo các service 
        ContactService contactService = new ContactService();

        //khai báo các value public
        int IDcontacts;

        //các hàm public
        public void LoadContacts()
        {
            Contact contacts = new Contact();
            contacts = contactService.GetContact();
            txtEmailContacts.Text = contacts.Email;
            txtPassContacts.Text = contacts.Pass;
            rtxtTitle.Text = contacts.Title;
            rtxtMessage.Text = contacts.Message;
            rtxtDiaChi.Text = contacts.Info;
        }

        //lay id contacts de insert
        public int getID()
        {
            Contact cts = new Contact();
            cts = contactService.GetContact();
            IDcontacts = cts.id;
            return IDcontacts;
        }

        //check data
        public bool checkDataContacts()
        {
            if (!string.IsNullOrEmpty(txtEmailContacts.Text))
            {
                checkEmail();
            }
            else
            {
                MessageBox.Show("Email không được để trống", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmailContacts.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtEmailContacts.Text))
            {
                MessageBox.Show("Password Email không được để trống", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassContacts.Focus();
                return false;
            }
            return true;
        }

        //kiem tra maill
        public bool VerifyEmail(string emailVerify)
        {
            using (WebClient webclient = new WebClient())
            {
                string url = "http://verify-email.org/";
                NameValueCollection formData = new NameValueCollection();
                formData["check"] = emailVerify;
                byte[] responseBytes = webclient.UploadValues(url, "POST", formData);
                string response = Encoding.ASCII.GetString(responseBytes);
                if (response.Contains("Result: Ok"))
                {
                    return true;
                }
                return false;
            }
        }

        public bool  checkEmail()
        {
            //string input = txtEmailContacts.Text;
            //string pattern = "\n";
            //string[] emails = Regex.Split(input, pattern);

            //for (int i = 0; i < emails.Length; i++)
            //{
            //    ListViewItem itemp = new ListViewItem(emails[i]);
            //    bool check = VerifyEmail(emails[i]);
            //    if (check == true)
            //    {
            //        MessageBox.Show("email ton tai");
            //        return true;
            //    }
            //    else
            //    {
            //        MessageBox.Show("email KHOONG ton tai");
            //        return false;
            //    }
            //}
            return true;
        }


       
        //kết thúc các hàm public

        private void frmContacts_Load(object sender, EventArgs e)
        {
            LoadContacts();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (checkDataContacts())
            {
                Contact contacts = new Contact();
                contacts.id = getID();
                contacts.Email = txtEmailContacts.Text;
                contacts.Pass = txtPassContacts.Text;
                contacts.Title = rtxtTitle.Text;
                contacts.Message = rtxtMessage.Text;
                contacts.Info = rtxtDiaChi.Text;
                contactService.InsertContacts(contacts);
                LoadContacts();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rtxtTitle_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rtxtTitle.Text))
            {
                rtxtTitle.Text = "Title";
            }
        }

        private void rtxtMessage_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rtxtMessage.Text))
            {
                rtxtMessage.Text = "Message";
            }
        }

        private void rtxtDiaChi_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rtxtDiaChi.Text))
            {
                rtxtDiaChi.Text = "Information";
            }
        }
    }
}
