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
    public partial class ArrivalControl : UserControl
    {
        public ArrivalControl()
        {
            InitializeComponent();
           // ShowArrivals();
        }

        private void lblTime_Load(object sender, EventArgs e)
        {

        }

        /*  private void ShowArrivals()
          {
              var lst = DataTools.GetArrivalsDeparturesClass("A");
              if (lst == null) return;
          }*/
    }
}
