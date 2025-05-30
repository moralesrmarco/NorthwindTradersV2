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

        private void button4_Click(object sender, EventArgs e)
        {
            FrmRptNotaRemision6 frmRptNotaRemision6 = new FrmRptNotaRemision6();
            frmRptNotaRemision6.Id = Id;
            frmRptNotaRemision6.ShowDialog();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FrmRptNotaRemision7 frmRptNotaRemision7 = new FrmRptNotaRemision7();
            frmRptNotaRemision7.Id = Id;
            frmRptNotaRemision7.ShowDialog();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FrmRptNotaRemision8 frmRptNotaRemision8 = new FrmRptNotaRemision8();
            frmRptNotaRemision8.Id = Id;
            frmRptNotaRemision8.ShowDialog();
            this.Close();
        }
    }
}
