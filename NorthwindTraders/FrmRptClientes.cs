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
            Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
            // TODO: esta línea de código carga datos en la tabla 'northwindDataSet.Customers' Puede moverla o quitarla según sea necesario.
            this.customersTableAdapter.Fill(this.northwindDataSet.Customers);
            Utils.ActualizarBarraDeEstado(this, $"Se encontraron {this.northwindDataSet.Customers.Rows.Count} registros");
            this.reportViewer1.RefreshReport();
        }

        private void FrmRptClientes_FormClosed(object sender, FormClosedEventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this);
        }
    }
}
