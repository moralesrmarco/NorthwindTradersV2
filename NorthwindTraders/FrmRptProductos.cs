using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptProductos: Form
    {

        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);
        string strProcedure = "";
        string titulo = "» Reporte de productos «";
        string subtitulo = "";

        public FrmRptProductos()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptProductos_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmRptProductos_Load(object sender, EventArgs e)
        {
            Utils.LlenarCbo(this, cboCategoria, "Sp_Categorias_Seleccionar_V2", "Categoria", "Id", cn);
            Utils.LlenarCbo(this, cboProveedor, "Sp_Proveedores_Seleccionar", "Proveedor", "Id", cn);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtIdInicial.Text = txtIdFinal.Text = txtProducto.Text = "";
            cboCategoria.SelectedIndex = cboProveedor.SelectedIndex = 0;
            MDIPrincipal.ActualizarBarraDeEstado();
        }

        private void btnImprimirTodos_Click(object sender, EventArgs e)
        {
            strProcedure = "Sp_Productos_All_Rpt";
            LlenarReporte(sender);
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            strProcedure = "Sp_Productos_Buscar_V2_RPT";
            LlenarReporte(sender);
        }

        private void txtIdInicial_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtIdFinal_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtIdInicial_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdIni(txtIdInicial, txtIdFinal);

        private void txtIdFinal_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdFin(txtIdInicial, txtIdFinal);

        private void tabcOperacion_Selected(object sender, TabControlEventArgs e) => btnLimpiar.PerformClick();
        
        private void tabcOperacion_SelectedIndexChanged(object sender, EventArgs e) => btnLimpiar.PerformClick();

        private void LlenarReporte(object sender)
        {
            try 
            {
                titulo = "» Reporte de todos los productos «";
                subtitulo = "";
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                SqlCommand cmd = new SqlCommand(strProcedure, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (((Button)sender).Tag.ToString() == "Imprimir")
                {
                    cmd.Parameters.AddWithValue("@IdIni", txtIdInicial.Text);
                    cmd.Parameters.AddWithValue("@IdFin", txtIdFinal.Text);
                    cmd.Parameters.AddWithValue("@Producto", txtProducto.Text);
                    cmd.Parameters.AddWithValue("@Categoria", cboCategoria.SelectedValue);
                    cmd.Parameters.AddWithValue("@Proveedor", cboProveedor.SelectedValue);
                    titulo = "» Reporte de productos filtrados «";
                    subtitulo = $"Filtrado por: ";
                    if (txtIdInicial.Text != "" & txtIdFinal.Text != "")
                        subtitulo += $" [ Id: {txtIdInicial.Text} al {txtIdFinal.Text} ] ";
                    if (txtProducto.Text != "")
                        subtitulo += $" [ Producto: {txtProducto.Text} ] ";
                    if (cboCategoria.SelectedIndex > 0)
                        subtitulo += $" [ Categoría: {cboCategoria.Text}] ";
                    if (cboProveedor.SelectedIndex > 0)
                        subtitulo += $" [ Proveedor: {cboProveedor.Text}] ";
                    if (subtitulo == "Filtrado por: ")
                    {
                        titulo = "» Reporte de todos los productos «";
                        subtitulo = "";
                    }
                }
                groupBox1.Text = titulo;
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                dap.Fill(tbl);
                MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {tbl.Rows.Count} registros");
                if (tbl.Rows.Count > 0)
                {
                    ReportDataSource reportDataSource = new ReportDataSource("DataSet1", tbl);
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                    ReportParameter rp = new ReportParameter("titulo", titulo);
                    ReportParameter rp2 = new ReportParameter("subtitulo", subtitulo);
                    reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp, rp2 });
                    reportViewer1.RefreshReport();
                }
                else
                {
                    reportViewer1.LocalReport.DataSources.Clear();
                    ReportDataSource reportDataSource = new ReportDataSource("DataSet1", new DataTable());
                    reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                    ReportParameter rp = new ReportParameter("titulo", titulo);
                    ReportParameter rp2 = new ReportParameter("subtitulo", subtitulo);
                    reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp, rp2 });
                    reportViewer1.RefreshReport();
                    MessageBox.Show(Utils.noDatos, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (SqlException ex) { Utils.MsgCatchOueclbdd(ex); }
            catch (Exception ex) { Utils.MsgCatchOue(ex); }
        }
    }
}
