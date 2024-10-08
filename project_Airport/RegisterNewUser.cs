using ApplicationLayer;
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
    public partial class RegisterNewUser : Form
    {
        public RegisterNewUser()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text))
            {
                MessageBox.Show("User name cannot be empty", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Email cannot be empty", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please check password", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if(!txtPassword.Text.Equals(txtPassword2.Text))
            {
                MessageBox.Show("Please check password", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //check if user exists
            if (DataTools.CheckUserExists(txtUser.Text, txtEmail.Text) == LoginState.NotExists)
            {
                if (DataTools.RegisterNewUser(txtUser.Text, txtEmail.Text, txtPassword.Text) == true)
                {
                    MessageBox.Show("User created. You can login", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();//close current form
                }
                else
                {
                    MessageBox.Show("Something went wrong", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("User name or Email is already taken", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
