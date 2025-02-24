using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptEmpleado2: Form
    {
        public FrmRptEmpleado2()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void FrmRptEmpleado2_Load(object sender, EventArgs e)
        {
            try
            {
                Utils.ActualizarBarraDeEstado(this, Utils.clbdd);
                EmpleadoConReportsTo empleado = new EmpleadoConReportsTo();
                List<EmpleadoConReportsTo> empleados = empleado.ObtenerEmpleados();
                Utils.ActualizarBarraDeEstado(this, $"Se encontraron {empleados.Count} registros");
                ReportDataSource reportDataSource = new ReportDataSource("DataSet1", empleados);
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                reportViewer1.RefreshReport();
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

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptEmpleado2_FormClosed(object sender, FormClosedEventArgs e) => Utils.ActualizarBarraDeEstado(this);
    }
}
