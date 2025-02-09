using Microsoft.Reporting.WinForms;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptEmpleados : Form
    {

        public FrmRptEmpleados()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void FrmRptEmpleados_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    // Crea un comando para ejecutar la consulta
                    string query = "SELECT e1.*, e2.FirstName As ReportsToFirstName, e2.LastName As ReportsToLastName FROM Employees e1 Left Join Employees e2 On e1.ReportsTo = e2.EmployeeID";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, cn);
                    // Crea una instancia del DataSet y llena los datos
                    NorthwindDataSet northwindDataSet = new NorthwindDataSet();
                    adapter.Fill(northwindDataSet, "Employees");

                    // Configura el origen de datos para el ReportViewer
                    ReportDataSource reportDataSource = new ReportDataSource();
                    reportDataSource.Name = "DataSet1";
                    reportDataSource.Value = northwindDataSet.Tables["Employees"];

                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                    this.reportViewer1.RefreshReport();
                }
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(this, ex);
            }
        }
    }
}
