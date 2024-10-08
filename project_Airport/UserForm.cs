using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project_Airport
{
    public partial class UserForm : Form
    {
        public UserForm()
        {
            InitializeComponent();

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            pnlShow.Controls.Add(new FlightControl(this.MdiParent) { Dock = DockStyle.Fill });
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            var history = new HistoryForm();
            history.MdiParent = this.MdiParent;
            history.Show();
        }
    }
}
