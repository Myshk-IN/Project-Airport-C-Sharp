using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ApplicationLayer.UserDataTools;

namespace project_Airport
{
    public partial class FlightControl : UserControl
    {
        Form prnt;
        public FlightControl(Form parent)
        {
            InitializeComponent();
            this.prnt = parent;

            dgvFlights.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFlights.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            var data = DataTools.AllAirports();
            if (data == null) return;
            cmbFrom.DataSource = data;
            cmbTo.Items.AddRange(data.ToArray());
            dtWhen.MinDate = DateTime.Now;
            dtWhen.Format = DateTimePickerFormat.Short;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            //ckeck if combobxes are not same
            if (cmbTo.Text.Equals(cmbFrom.Text))
            {
                MessageBox.Show("Cannot fly to the same airport","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var data = DataTools.GetFlights(cmbFrom.Text, cmbTo.Text, dtWhen.Value);
            if (data == null) return;
            dgvFlights.DataSource = data;
        }

        private void dgvFlights_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           // MessageBox.Show("Test");
            var frm = new SeatSelection(dgvFlights["FlightNr",e.RowIndex].Value.ToString(),(DateTime)dgvFlights["FlightTime",e.RowIndex].Value);
            frm.MdiParent = prnt;
           frm.Show();
        }
    }
}
