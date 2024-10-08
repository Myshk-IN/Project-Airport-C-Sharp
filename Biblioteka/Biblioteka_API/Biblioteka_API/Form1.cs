using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Biblioteka_API
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                MessageBox.Show("Please enter user name", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter password", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

          
                if (await Operations.CheckLogin(txtUserName.Text, txtPassword.Text))
                {
                    MessageBox.Show("Login successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //open userForm
                    new UserForm(txtUserName.Text).Show();
                //hide login form
                //this.Invoke(new MethodInvoker(delegate { this.Hide(); })); //mozhet byt liambda expression this.Invoke(new MethodInvoker(() => { }));
                this.Hide();
            }
                else
                {
                    MessageBox.Show("Please check username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
           
        }
    }
}
