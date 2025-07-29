using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptGraficaVentasMensualesPorVendedorPorAnio : Form
    {
        public FrmRptGraficaVentasMensualesPorVendedorPorAnio()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptGraficaVentasMensualesPorVendedorPorAnio_Load(object sender, EventArgs e)
        {
            LlenarCmbVentasDelAño();
        }

        private void LlenarCmbVentasDelAño()
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
                                CmbVentasDelAño.Items.Add(year);
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
            CmbVentasDelAño.SelectedIndex = 0;
        }

        private void CmbVentasDelAño_SelectedIndexChanged(object sender, EventArgs e)
        {
            LlenarGrafico(Convert.ToInt32(CmbVentasDelAño.Text.ToString()));
        }

        private void LlenarGrafico(int year)
        {
            groupBox1.Text = $"» Reporte gráfico comparativo de ventas mensuales por vendedores del año {year} «";
            DataTable dt = ObtenerDatos(year);
            reportViewer1.LocalReport.DataSources.Clear();
            var rds = new ReportDataSource("DataSet1", dt);
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.LocalReport.SetParameters(new ReportParameter("Subtitulo", $"Ventas mensuales por vendedores del año {year}"));
            reportViewer1.LocalReport.SetParameters(new ReportParameter("Anio", year.ToString()));
            reportViewer1.RefreshReport();
        }

        private DataTable ObtenerDatos(int year)
        {
            DataTable dt = new DataTable();
            string query = @"
                            -- CTE de meses (1 a 12)
                            WITH Meses AS (
                            SELECT 1 AS Mes, 'Ene' AS NombreMes
                            UNION ALL SELECT 2, 'Feb'
                            UNION ALL SELECT 3, 'Mar'
                            UNION ALL SELECT 4, 'Abr'
                            UNION ALL SELECT 5, 'May'
                            UNION ALL SELECT 6, 'Jun'
                            UNION ALL SELECT 7, 'Jul'
                            UNION ALL SELECT 8, 'Ago'
                            UNION ALL SELECT 9, 'Sep'
                            UNION ALL SELECT 10, 'Oct'
                            UNION ALL SELECT 11, 'Nov'
                            UNION ALL SELECT 12, 'Dic'
                            ),
                            -- CTE de todos los vendedores (ajusta si solo quieres quienes tienen pedidos en @Anio)
                            Vendedores AS (
                                SELECT 
                                    EmployeeID, 
                                    CONCAT(FirstName, ' ', LastName) AS Vendedor
                                FROM Employees e
                                WHERE EmployeeID IN (SELECT DISTINCT EmployeeID FROM Orders WHERE YEAR(OrderDate) = @Anio)      
                            ),

                            -- CTE con ventas agregadas por vendedor y mes
                            Ventas AS (
                                SELECT 
                                    e.EmployeeID,
                                    MONTH(o.OrderDate) AS Mes,
                                    SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)) AS TotalVentas
                                FROM Employees e
                                JOIN Orders o
                                  ON e.EmployeeID = o.EmployeeID
                                JOIN [Order Details] od
                                  ON o.OrderID = od.OrderID
                                WHERE YEAR(o.OrderDate) = @Anio
                                GROUP BY 
                                    e.EmployeeID,
                                    MONTH(o.OrderDate)
                            )
                            SELECT
                                V.Vendedor,
                                CAST(M.Mes AS INT)        AS Mes,
                                M.NombreMes              AS NombreMes,
                                ISNULL(VT.TotalVentas, 0) AS TotalVentas
                            FROM Vendedores V
                            CROSS JOIN Meses M
                            LEFT JOIN Ventas VT
                              ON VT.EmployeeID = V.EmployeeID
                             AND VT.Mes        = CAST(M.Mes AS INT)
                            ORDER BY
                                V.Vendedor,
                                CAST(M.Mes AS INT);
                            ";
            MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
            try
            {
                using (var cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    using (var da = new SqlDataAdapter(query, cn))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@Anio", year);
                        da.Fill(dt);
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
