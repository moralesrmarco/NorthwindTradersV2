using System;
using System.Data.SqlClient;
using System.Web.Management;
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
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                // TODO: esta línea de código carga datos en la tabla 'northwindDataSet.Suppliers' Puede moverla o quitarla según sea necesario.
                this.suppliersTableAdapter.Fill(this.northwindDataSet.Suppliers);
                Utils.ActualizarBarraDeEstado(this, $"Se encontraron {northwindDataSet.Suppliers.Rows.Count} registros");
                this.reportViewer1.RefreshReport();
            }
            catch (SqlException ex)
            {
                Utils.MsgCatchOueclbdd(this, ex);
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(this, ex);
            }
        }

        private void FrmRptProveedores_FormClosed(object sender, FormClosedEventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this);
        }
    }
}
