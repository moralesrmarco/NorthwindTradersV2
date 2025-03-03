using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptProductosAlfabetico: Form
    {
        public FrmRptProductosAlfabetico()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptProductosAlfabetico_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmRptProductosAlfabetico_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    using (SqlCommand cmd = new SqlCommand("Sp_Productos_All_Rpt", cn))
                    {
                        MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter dap = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        dap.Fill(dt);
                        MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {dt.Rows.Count} registros");
                        if (dt.Rows.Count > 0)
                        {
                            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
                            reportViewer1.LocalReport.DataSources.Clear();
                            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                            reportViewer1.RefreshReport();
                        }
                        else
                        {
                            reportViewer1.LocalReport.DataSources.Clear();
                            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", new DataTable());
                            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
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
