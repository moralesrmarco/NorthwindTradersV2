using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptCategorias : Form
    {
        public FrmRptCategorias()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptCategorias_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmRptCategorias_Load(object sender, EventArgs e)
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                // Obtener la lista de categorías
                List<Categoria> categorias = new List<Categoria>();
                Categoria categoria = new Categoria();
                categorias = categoria.ObtenerCategorias();
                MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {categorias.Count} registros");
                ReportDataSource reportDataSource = new ReportDataSource("DataSet1", categorias);
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                reportViewer1.LocalReport.Refresh();
                reportViewer1.RefreshReport();
            }
            catch (SqlException ex)
            {
                Utils.MsgCatchOueclbdd(ex);
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }
    }
}
