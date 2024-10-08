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
    public partial class HistoryForm : Form
    {
        public HistoryForm()
        {
            InitializeComponent();
            
       
        }

        private void ShowHistory()
        {
            dgvHistory.DataSource = DataTools.ViewHistorty((this.MdiParent as MainForm).lblUser1.Text);
        }

        private void HistoryForm_Load(object sender, EventArgs e)
        {
            ShowHistory();
        }
    }
}
