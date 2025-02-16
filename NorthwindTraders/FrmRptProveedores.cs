using System;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptProveedores : Form
    {
        public FrmRptProveedores()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e)
        {
            Utils.GrbPaint(this, sender, e);
        }

        private void FrmRptProveedores_Load(object sender, EventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
            // TODO: esta línea de código carga datos en la tabla 'northwindDataSet.Suppliers' Puede moverla o quitarla según sea necesario.
            this.suppliersTableAdapter.Fill(this.northwindDataSet.Suppliers);
            Utils.ActualizarBarraDeEstado(this, $"Se encontraron {northwindDataSet.Suppliers.Rows.Count} registros");
            this.reportViewer1.RefreshReport();
        }

        private void FrmRptProveedores_FormClosed(object sender, FormClosedEventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this);
        }
    }
}
