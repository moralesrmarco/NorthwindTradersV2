using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptGraficaDeVentasDeVendedoresPorAnio : Form
    {
        public FrmRptGraficaDeVentasDeVendedoresPorAnio()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptGraficaDeVentasDeVendedoresPorAnio_Load(object sender, EventArgs e)
        {
            LlenarCmbVentas();
        }

        private void LlenarCmbVentas()
        {
            MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
            try
            {
                using (var cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    using (var cmd = new SqlCommand("SELECT DISTINCT YEAR(OrderDate) AS YearOrderDate FROM Orders ORDER BY YearOrderDate DESC", cn))
                    {
                        cn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int year = reader.GetInt32(0);
                                CmbVentas.Items.Add(year);
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
            MDIPrincipal.ActualizarBarraDeEstado();
            CmbVentas.SelectedIndex = 0; // Seleccionar el primer elemento por defecto
        }

        private void CmbVentas_SelectedIndexChanged(object sender, EventArgs e)
        {
            LlenarGrafico(Convert.ToInt32(CmbVentas.Text.ToString()));
        }

        private void LlenarGrafico(int year)
        {
            groupBox1.Text = $"» Reporte gráfico ventas por vendedores del año {year} «";
            DataTable dt = ObtenerDatos(year);
            reportViewer1.LocalReport.DataSources.Clear();
            var rds = new ReportDataSource("DataSet1", dt);
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.LocalReport.SetParameters(new ReportParameter("Subtitulo", $"Ventas por vendedores del año {year}"));
            reportViewer1.LocalReport.SetParameters(new ReportParameter("Anio", year.ToString()));
            reportViewer1.RefreshReport();
        }

        private DataTable ObtenerDatos(int year)
        {
            DataTable dt = new DataTable();
            string query = @"
                            SELECT 
                                CONCAT(e.FirstName, ' ', e.LastName) AS Vendedor,
                                SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)) AS TotalVentas
                            FROM 
                                Employees e
                            JOIN 
                                Orders o ON e.EmployeeID = o.EmployeeID
                            JOIN 
                                [Order Details] od ON o.OrderID = od.OrderID
                            WHERE 
                                YEAR(o.OrderDate) = @Year
                            GROUP BY 
                                e.FirstName, e.LastName
                            ORDER BY TotalVentas DESC
                            ";
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                using (var cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    using (var cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@Year", year);
                        cn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
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
            MDIPrincipal.ActualizarBarraDeEstado();
            return dt;
        }

    }
}
