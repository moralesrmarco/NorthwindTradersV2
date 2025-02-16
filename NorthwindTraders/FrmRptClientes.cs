using System;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptClientes : Form
    {
        public FrmRptClientes()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e)
        {
            Utils.GrbPaint(this, sender, e);
        }

        private void FrmRptClientes_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'northwindDataSet.Customers' Puede moverla o quitarla según sea necesario.
            this.customersTableAdapter.Fill(this.northwindDataSet.Customers);

            this.reportViewer1.RefreshReport();
        }

    }
}
