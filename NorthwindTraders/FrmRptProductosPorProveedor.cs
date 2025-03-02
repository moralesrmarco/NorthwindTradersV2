using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptProductosPorProveedor: Form
    {

        public FrmRptProductosPorProveedor()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptProductosPorProveedor_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmRptProductosPorProveedor_Load(object sender, EventArgs e)
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    using (SqlCommand cmd = new SqlCommand("Sp_ProductosPorProveedor", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter dap = new SqlDataAdapter(cmd);
                        DataTable tbl = new DataTable();
                        dap.Fill(tbl);
                        MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {tbl.Rows.Count} registros");
                        if (tbl.Rows.Count > 0)
                        {
                            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", tbl);
                            reportViewer1.LocalReport.DataSources.Clear();
                            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                            reportViewer1.LocalReport.Refresh();
                            reportViewer1.RefreshReport();
                        }
                        else
                        {
                            reportViewer1.LocalReport.DataSources.Clear();
                            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", new DataTable());
                            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                            reportViewer1.LocalReport.Refresh();
                            reportViewer1.RefreshReport();
                            MessageBox.Show(Utils.noDatos, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (SqlException ex) { Utils.MsgCatchOueclbdd(ex); }
            catch (Exception ex) { Utils.MsgCatchOue(ex); }
        }

    }
}
