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
using Newtonsoft.Json;
using Newtonsoft;

namespace project_Airport
{
    public partial class SeatSelection : Form
    {
        short columns = 10;
        string flightNr;
        DateTime flightTime;
        List<int> reservedSeats = new List<int>();

        public SeatSelection(string flightNr, DateTime flightTime)
        {
            InitializeComponent();
            this.flightNr = flightNr;
            this.flightTime = flightTime;
            ShowSeats();
        }

        private void ShowSeats()
        {
            var lst = DataTools.GetSeats(flightNr, flightTime);
            if (lst == null)
            {
                MessageBox.Show("Ooooops");
                return;
            }
            //get seatCount
            ushort seatCount = Convert.ToUInt16(lst[0]);
            
          //  if(lst[1] != null)
           // {
                FillTable(seatCount, JsonConvert.DeserializeObject<List<int>>(lst[1]));
           // }
        }

        private void FillTable(ushort seatCount, List<int> taken)
        {
            if (seatCount == taken.Count) btnBuy.Enabled = false;
           
            int rows = 0;
            rows = seatCount % columns == 0 ? seatCount / columns : (seatCount / columns) + 1;

            tblSeats.ColumnCount = columns;
            tblSeats.RowCount = rows;

            for (int i = 0; i < tblSeats.RowCount; i++)
            {
                tblSeats.RowStyles.Add(new RowStyle() { SizeType = SizeType.AutoSize });
            }
            for (int i = 0; i < tblSeats.ColumnCount; i++)
            {
                tblSeats.ColumnStyles.Add(new ColumnStyle() { SizeType = SizeType.AutoSize });
            }


            int counter = 0;

            for (int i = 0; i < tblSeats.RowCount; i++)
            {
                for (int j = 0; j < tblSeats.ColumnCount; j++)
                {
                    counter++;
                    Button btn = new Button();
                    btn.AutoSize = true;
                    btn.Text = counter.ToString();
                    if(taken!=null && taken.Contains(counter))
                    {
                        btn.BackColor = Color.Red;
                        btn.Enabled = false;
                    }
                    else
                    {
                        btn.BackColor = Color.Green;
                    }
                    btn.Click += Btn_Click;
                    tblSeats.Controls.Add(btn, j, i);
                    
                    if (seatCount == counter) break;
                }
                if (seatCount == counter) break;
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if(btn.BackColor==Color.Green)
            {
                btn.BackColor = Color.Yellow;
                reservedSeats.Add(Convert.ToInt32(btn.Text));
            }
            else
            {
                btn.BackColor = Color.Green;
                reservedSeats.Remove(Convert.ToInt32(btn.Text));
            }
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            //get user from toolstrip
            string user = (this.MdiParent as MainForm).lblUser1.Text;
            var lst = DataTools.GetTicketClasses();

            //create new ticketClass form
            var ticket = new TicketClassForm(lst);
            ticket.MdiParent = this.MdiParent;
            ticket.ClassSelected += delegate (object sender, EventArgs e)
              {
                  var data = (e as TicketClassEventArgs).ClassName;
                  if (DataTools.BuyTickets(reservedSeats, flightNr, flightTime, user, data))
                  {
                      ticket.Close();
                      this.Close();
                      MessageBox.Show("Seat Reserved");                      
                  }
              };
            ticket.Show();

            
        }
    }
}
