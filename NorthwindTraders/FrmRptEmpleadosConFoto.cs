using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptEmpleadosConFoto : Form
    {

        public FrmRptEmpleadosConFoto()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void FrmRptEmpleadosConFoto_Load(object sender, EventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
            // Obtener la lista de empleados
            List<EmpleadoConReportsTo> empleados = new List<EmpleadoConReportsTo>();
            EmpleadoConReportsTo empleadoConReportsTo = new EmpleadoConReportsTo(this);
            empleados = empleadoConReportsTo.ObtenerEmpleados();
            Utils.ActualizarBarraDeEstado(this, $"Se encntraron {empleados.Count} registros");
            ReportDataSource reportDataSource = new ReportDataSource("EmpleadoConReportsToDataSet", empleados);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            reportViewer1.LocalReport.ReportPath = @"RptEmpleadosConFoto.rdlc"; // Asegúrate de que la ruta sea correcta
            reportViewer1.RefreshReport();
        }

        private void GrbPaint(object sender, PaintEventArgs e)
        {
            Utils.GrbPaint(this, sender, e);
        }

        private void FrmRptEmpleadosConFoto_FormClosed(object sender, FormClosedEventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this);
        }
    }
}
