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
    public partial class MyInfoForm : Form
    {
        private string userName;
        public MyInfoForm(string userName)
        {
            InitializeComponent();
            this.userName = userName;
            GetUserInfo();

        }

        private void GetUserInfo()
        {
            Task.Run(async delegate ()
            {
                var info = await Operations.GetUserInfo(this.userName); // tol, kol funkcija Operations.GetUserInfo(this.userName) neparodys teigiamo atsakymo, tolesnis kodas nerodomas
                this.Invoke(new MethodInvoker(delegate
                {
                    txtFullName.Text = info.FullName;
                    txtEmail.Text = info.Email;
                    txtAddress.Text = info.Address;
                    txtCode.Text = info.Code.ToString();
                }));
            });
        }
        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            var newInfo = new UserInfo()
            {
                FullName = txtFullName.Text,
                Email = txtEmail.Text,
                Address = txtAddress.Text,
                Code = Convert.ToUInt64(txtCode.Text)
            };

            if(await Operations.UpdateMyInfo(newInfo))
            {
                MessageBox.Show("Info updated");
                //GetUserInfo();
                this.Close();
            }
        }
    }
}
