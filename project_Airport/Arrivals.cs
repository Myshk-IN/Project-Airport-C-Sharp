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
    public partial class Arrivals : Form
    {
        public Arrivals()
        {
            InitializeComponent();
            ShowArrivals();
        }

         private void ShowArrivals()
         {
            var lst = DataTools.GetArrivalsDeparturesClass("A");
             if (lst == null) return;
            for(int i=0;i<lst.Count;i++)
            {
                //kuriam nauja ArrivalControl objekta
                var arr = new ArrivalControl();
                arr.Location = new Point(0,arr.Height*i);
                arr.lblFlightNumber.Text = lst[i].FlightNr;
                arr.lblFrom.Text= lst[i].From;
                arr.lblTime.Text = lst[i].Time.ToString();
                //pridedam i panel'i
                pnlArrivals.Controls.Add(arr);
            }
         }
    }
}
