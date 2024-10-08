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
    public partial class TicketClassForm : Form
    {
        public event EventHandler ClassSelected;//EventHandler - delegatas i ivyki

        private void TicketClassSeleceted()
        {
            if (ClassSelected!=null)
            {
                TicketClassEventArgs e = new TicketClassEventArgs();
                e.ClassName = comboBox1.Text;
                ClassSelected(this, e);
            }
        }
        public TicketClassForm(List<string> lst)
        {
            InitializeComponent();
            comboBox1.DataSource = lst;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TicketClassSeleceted();
        }
    }
}
