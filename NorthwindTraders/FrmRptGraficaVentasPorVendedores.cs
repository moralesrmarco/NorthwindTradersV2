using Microsoft.Reporting.WinForms;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptGraficaVentasPorVendedores : Form
    {
        public FrmRptGraficaVentasPorVendedores()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptGraficaVentasPorVendedores_Load(object sender, EventArgs e)
        {
            DataTable dt = ObtenerDatos();
            reportViewer1.LocalReport.DataSources.Clear();
            var rds = new ReportDataSource("DataSet1", dt);
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.LocalReport.SetParameters(new ReportParameter("Subtitulo", "Gráfico ventas por vendedores de todos los años"));
            reportViewer1.RefreshReport();
        }

        private DataTable ObtenerDatos()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Vendedor", typeof(string));
            dt.Columns.Add("TotalVentas", typeof(decimal));
            try
            {
                using (var cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    using (var cmd = new SqlCommand(@"
                        SELECT 
                            CONCAT(e.FirstName, ' ', e.LastName) AS Vendedor,
                            SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)) AS TotalVentas
                        FROM 
                            Employees e
                        JOIN 
                            Orders o ON e.EmployeeID = o.EmployeeID
                        JOIN 
                            [Order Details] od ON o.OrderID = od.OrderID
                        GROUP BY 
                            e.FirstName, e.LastName
                        ORDER BY TotalVentas Desc", cn))
                    {
                        cn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dt.Rows.Add(reader["Vendedor"], reader["TotalVentas"]);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Utils.MsgCatchOueclbdd(ex);
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            return dt;
        }
    }
}
