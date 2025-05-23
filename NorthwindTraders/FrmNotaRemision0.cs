using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmNotaRemision0 : Form
    {

        public int Id;

        public FrmNotaRemision0()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmRptNotaRemision frmRptNotaRemision = new FrmRptNotaRemision();
            frmRptNotaRemision.Id = Id;
            frmRptNotaRemision.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmRptNotaRemision2 frmRptNotaRemision2 = new FrmRptNotaRemision2();
            frmRptNotaRemision2.Id = Id;
            frmRptNotaRemision2.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FrmRptNotaRemision4 frmRptNotaRemision4 = new FrmRptNotaRemision4();
            frmRptNotaRemision4.Id = Id;
            frmRptNotaRemision4.ShowDialog();
            this.Close();
        }
    }
}
