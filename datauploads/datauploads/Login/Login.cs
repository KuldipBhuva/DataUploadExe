using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using datauploads;
using DataUploads.DataModels;

namespace DataUploads.Login
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            labelMessage.Text = "";
            if (textBoxUsername.Text.Trim() == "" || textBoxPassword.Text.Trim() == "")
            {
                labelMessage.Text = "Either username or password is blank.";
                return;
            }
            
            ExeUser exeUser = new ExeUser();
            exeUser.Username = textBoxUsername.Text.Trim();
            exeUser.Password = textBoxPassword.Text.Trim();
            
            if (exeUser.ExeUserLogin())
            {
                Form1 frm1 = new Form1();
                this.Hide();
                frm1.ShowDialog();
            }
            else
            {
                labelMessage.Text = "Username and password not matched, please try again.";
            }
            //Application.Run(new Form1());
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
