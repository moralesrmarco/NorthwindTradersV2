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
            // Obtener la lista de empleados
            List<EmpleadoConReportsTo> empleados = new List<EmpleadoConReportsTo>();
            EmpleadoConReportsTo empleadoConReportsTo = new EmpleadoConReportsTo(this);
            empleados = empleadoConReportsTo.ObtenerEmpleados();

            ReportDataSource reportDataSource = new ReportDataSource("EmpleadoConReportsToDataSet", empleados);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            reportViewer1.LocalReport.ReportPath = @"..\..\RptEmpleadosConFoto.rdlc"; // Asegúrate de que la ruta sea correcta
            reportViewer1.RefreshReport();
        }
    }
}
