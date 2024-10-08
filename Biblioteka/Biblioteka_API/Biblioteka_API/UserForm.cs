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
    public partial class UserForm : Form
    {
        private string userName;
        public UserForm(string userName)
        {
            InitializeComponent();
            this.userName = userName;
            this.FormClosed += UserForm_FormClosed;
        }

        private void UserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void myInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MyInfoForm(userName).ShowDialog();
        }

        private void myTakenBooksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Run(delegate () 
            {
                dgvData.Invoke(new MethodInvoker(async delegate () 
                {
                    dgvData.DataSource = await Operations.GetTakenBook(userName);
                }));
            });
        }

        private void booksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Run(delegate ()
            {
                dgvData.Invoke(new MethodInvoker(async delegate ()
                {
                    dgvData.DataSource = await Operations.GetBooks();
                }));
            });
        }
    }
}
