using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ApplicationLayer.AdminDataTools;

namespace AirportAdmin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dgvAdmin.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void aIRPORTSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvAdmin.DataSource = AdminTools.GetAirports();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvAdmin.DataSource = AdminTools.GetUsers();
        }

        private void planeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvAdmin.DataSource = AdminTools.GetPlanes();
        }

        private void flightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvAdmin.DataSource = AdminTools.GetFlights();
        }
    }
}
