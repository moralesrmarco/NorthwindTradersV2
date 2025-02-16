using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptClientesyProveedoresDirectorio : Form
    {

        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);
        string titulo = string.Empty;

        public FrmRptClientesyProveedoresDirectorio()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e)
        {
            Utils.GrbPaint(this, sender, e);
        }

        private void FrmRptClientesyProveedoresDirectorio_FormClosed(object sender, FormClosedEventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this);
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (!checkBoxClientes.Checked & !checkBoxProveedores.Checked)
            {
                MessageBox.Show(Utils.errorCriterioSelec, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                SqlCommand cmd = new SqlCommand();
                // Vw_ClientesProveedores_DirectorioPorCiudad_Rpt también me es util para este reporte
                if (checkBoxClientes.Checked & checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand("Select * from Vw_ClientesProveedores_DirectorioPorCiudad_Rpt Order by Relacion, NombreCompania", cn);
                    titulo = "» Reporte directorio de clientes y proveedores «";
                }
                else if (checkBoxClientes.Checked & !checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand("Select * from Vw_ClientesProveedores_DirectorioPorCiudad_Rpt Where Relacion = 'Cliente' Order by NombreCompania", cn);
                    titulo = "» Reporte directorio de clientes «";
                }
                else if (!checkBoxClientes.Checked & checkBoxProveedores.Checked)
                {
                    cmd = new SqlCommand("Select * from Vw_ClientesProveedores_DirectorioPorCiudad_Rpt Where Relacion = 'Proveedor' Order by NombreCompania", cn);
                    titulo = "» Reporte directorio de proveedores «";
                }
                groupBox1.Text = titulo;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                Utils.ActualizarBarraDeEstado(this);
                if (dt.Rows.Count > 0)
                {
                    ReportDataSource rds = new ReportDataSource("DataSet1", dt);
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(rds);
                    ReportParameter rp = new ReportParameter("titulo", titulo);
                    reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp });
                    reportViewer1.LocalReport.Refresh();
                    reportViewer1.RefreshReport();
                }
                else
                {
                    reportViewer1.LocalReport.DataSources.Clear();
                    ReportDataSource rds = new ReportDataSource("DataSet1", new DataTable());
                    reportViewer1.LocalReport.DataSources.Add(rds);
                    ReportParameter rp = new ReportParameter("titulo", titulo);
                    reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp });
                    reportViewer1.LocalReport.Refresh();
                    reportViewer1.RefreshReport();
                    MessageBox.Show(Utils.noDatos, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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

        private void checkBoxClientes_CheckedChanged(object sender, EventArgs e)
        {
            if ((checkBoxClientes.Checked & checkBoxProveedores.Checked) | (!checkBoxClientes.Checked & !checkBoxProveedores.Checked))
                groupBox1.Text = "» Reporte directorio de clientes y proveedores «";
            else if (checkBoxClientes.Checked & !checkBoxProveedores.Checked)
                groupBox1.Text = "» Reporte directorio de clientes «";
            else if (!checkBoxClientes.Checked & checkBoxProveedores.Checked)
                groupBox1.Text = "» Reporte directorio de proveedores «";
        }

        private void checkBoxProveedores_CheckedChanged(object sender, EventArgs e)
        {
            if ((checkBoxClientes.Checked & checkBoxProveedores.Checked) | (!checkBoxClientes.Checked & !checkBoxProveedores.Checked))
                groupBox1.Text = "» Reporte directorio de clientes y proveedores «";
            else if (checkBoxClientes.Checked & !checkBoxProveedores.Checked)
                groupBox1.Text = "» Reporte directorio de clientes «";
            else if (!checkBoxClientes.Checked & checkBoxProveedores.Checked)
                groupBox1.Text = "» Reporte directorio de proveedores «";
        }
    }
}
