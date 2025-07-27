using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NorthwindTraders
{
    public partial class FrmGraficaVentasPorVendedores : Form
    {
        public FrmGraficaVentasPorVendedores()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmGraficaVentasPorVendedores_Load(object sender, EventArgs e)
        {
            CargarVentasPorVendedores();
        }

        private void CargarVentasPorVendedores()
        {
            ChartVentasPorVendedores.Series.Clear();
            ChartVentasPorVendedores.Titles.Clear();
            // Título del gráfico
            Title titulo = new Title
            {
                Text = "» Gráfica ventas por vendedores de todos los años «",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 51, 102)
            };
            ChartVentasPorVendedores.Titles.Add(titulo);
            // Configuración de la serie
            Series serie = new Series
            {
                Name = "Ventas",
                Color = Color.FromArgb(0, 51, 102),
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Doughnut,
                Label = "#VALX: #VALY{C2}"
            };
            serie["PieLabelStyle"] = "Outside";
            serie.SmartLabelStyle.Enabled = true;
            serie.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
            serie.SmartLabelStyle.CalloutLineColor = Color.Black;
            serie.LabelForeColor = Color.DarkSlateGray;
            serie.LabelBackColor = Color.WhiteSmoke;
            ChartVentasPorVendedores.Series.Add(serie);
            // Consulta SQL para obtener las ventas por vendedor
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
                GROUP BY 
                    e.FirstName, e.LastName
                ORDER BY 
                    TotalVentas DESC
                ";
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                // Conexión a la base de datos y ejecución de la consulta
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    SqlCommand command = new SqlCommand(query, cn);
                    cn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string vendedor = reader["Vendedor"].ToString();
                        decimal totalVentas = Convert.ToDecimal(reader["TotalVentas"]);
                        // Agregar datos a la serie del gráfico
                        serie.Points.AddXY(vendedor, totalVentas);
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
            // Configuración del eje X y Y
            ChartVentasPorVendedores.ChartAreas[0].AxisX.Title = "Vendedores";
            ChartVentasPorVendedores.ChartAreas[0].AxisY.Title = "Total Ventas ($)";
        }
    }
}
