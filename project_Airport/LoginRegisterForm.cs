using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ApplicationLayer.UserDataTools;

namespace project_Airport
{
    public partial class LoginRegisterForm : Form
    {
        public LoginRegisterForm()
        {
            InitializeComponent();

            //assign event to textboxes
            txtuser.KeyDown += CheckEnterPressed;
            txtpassword.KeyDown += CheckEnterPressed;
        }

        private void CheckEnterPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) btnLogin.PerformClick();
                       
        }

        private void LoginRegisterForm_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtuser.Text))
            {
                MessageBox.Show("User name cannot be empty","Info",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtpassword.Text))
            {
                MessageBox.Show("Please check password", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var rez = DataTools.CheckUserCredentials(txtuser.Text, txtpassword.Text);
            switch (rez)
            {
                case LoginState.Blocked:
                    MessageBox.Show("User blocked. Contact airport administrator", "Eroor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case LoginState.Connected:
                    MessageBox.Show("Access granter", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    new UserForm() { MdiParent = this.MdiParent}.Show();

                    (this.MdiParent as MainForm).lblUser1.Text = txtuser.Text;
                    this.Close();
                    break;
                case LoginState.NotExists:
                    MessageBox.Show("User not found in our database. Contact airport administrator", "Eroor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case LoginState.BadPassword:
                    MessageBox.Show("Please check your password", "Eroor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                default:
                    break;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var register = new RegisterNewUser();
            register.MdiParent = this.MdiParent;
            register.Show();
        }
    }
}
