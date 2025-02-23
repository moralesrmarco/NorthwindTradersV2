using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptEmpleado : Form
    {

        public int Id { get; set; }

        public FrmRptEmpleado()
        {
            InitializeComponent();
        }

        private void FrmRptEmpleado_Load(object sender, EventArgs e)
        {
            try
            {
                Utils.ActualizarBarraDeEstado(this.Owner, Utils.clbdd);
                EmpleadoConReportsTo empleado = new EmpleadoConReportsTo();
                empleado.ObtenerEmpleado(empleado, Id);
                Utils.ActualizarBarraDeEstado(this.Owner, $"Se encontró el registro {empleado.EmployeeID}");
                // Crear una lista con un solo empleado
                List<EmpleadoConReportsTo> empleados = new List<EmpleadoConReportsTo> { empleado };
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

        private void GrbPaint(object sender, PaintEventArgs e)
        {
            Utils.GrbPaint(this, sender, e);
        }

        private void FrmRptEmpleado_FormClosed(object sender, FormClosedEventArgs e)
        {
            Utils.ActualizarBarraDeEstado(this.Owner);
        }
    }
}
