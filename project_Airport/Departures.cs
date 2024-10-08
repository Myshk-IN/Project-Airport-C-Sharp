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
    public partial class Departures : Form
    {
        public Departures()
        {
            InitializeComponent();
            dataGridView1.DataSource = DataTools.GetArrivalsDepartures("D");
        }
    }
}
