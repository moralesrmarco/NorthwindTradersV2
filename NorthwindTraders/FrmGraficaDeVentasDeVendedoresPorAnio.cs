using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NorthwindTraders
{
    public partial class FrmGraficaDeVentasDeVendedoresPorAnio : Form
    {
        public FrmGraficaDeVentasDeVendedoresPorAnio()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmGraficaDeVentasDeVendedoresPorAnio_Load(object sender, EventArgs e)
        {
            LlenarComboBox();
            CargarVentasPorVendedores(DateTime.Now.Year); // Cargar ventas del año actual por defecto
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                MessageBox.Show("Seleccione un año válido.", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            CargarVentasPorVendedores(Convert.ToInt32(comboBox1.SelectedItem));
        }

        private void LlenarComboBox()
        {
            MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
            comboBox1.Items.Add("»--- Seleccione ---«");
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
                                comboBox1.Items.Add(year);
                            }
                        }
                    }
                }
                comboBox1.SelectedIndex = 0;
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
        }

        private void CargarVentasPorVendedores(int anio)
        {
            chart1.Series.Clear();
            chart1.Titles.Clear();

            chart1.Legends.Clear();
            var leyenda = new Legend("Vendedores")
            {
                Title = "Vendedores",
                TitleFont = new Font("Arial", 10, FontStyle.Bold),
                Docking = Docking.Right,
                LegendStyle = LegendStyle.Table
            };
            chart1.Legends.Add(leyenda);

            // Título del gráfico
            Title titulo = new Title
            {
                Text = $"» Gráfica de ventas por vendedores del año {anio} «",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 51, 102)
            };
            chart1.Titles.Add(titulo);
            groupBox1.Text = titulo.Text; // Actualizar el texto del GroupBox
            // Configuración de la serie
            Series serie = new Series
            {
                Name = "Ventas",
                Color = Color.FromArgb(0, 51, 102),
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Doughnut,
                Label = "#AXISLABEL: #VALY{C2}",
                ToolTip = "Vendedor: #AXISLABEL\nTotal ventas: #VALY{C2}",
                Legend = leyenda.Name,
                LegendText = "#AXISLABEL: #VALY{C2}"
            };
            // 1. Configurar ChartArea 3D
            var area = chart1.ChartAreas[0];
            area.Area3DStyle.Enable3D = true;
            area.Area3DStyle.Inclination = 40;
            area.Area3DStyle.Rotation = 60;
            area.Area3DStyle.LightStyle = LightStyle.Realistic;
            area.Area3DStyle.WallWidth = 0;

            serie["PieLabelStyle"] = "Outside";
            serie["PieDrawingStyle"] = "Cylinder";
            serie["DoughnutRadius"] = "60";
            chart1.Series.Clear(); 
            chart1.Series.Add(serie);
            // Consulta SQL para obtener las ventas por vendedor del año seleccionado
            string query = @"
                SELECT 
                    CONCAT(e.FirstName, ' ', e.LastName) AS Vendedor,
                    SUM(od.UnitPrice * od.Quantity) AS TotalVentas
                FROM 
                    Employees e
                JOIN 
                    Orders o ON e.EmployeeID = o.EmployeeID
                JOIN 
                    [Order Details] od ON o.OrderID = od.OrderID
                WHERE 
                    YEAR(o.OrderDate) = @Anio
                GROUP BY 
                    e.FirstName, e.LastName
                ORDER BY 
                    TotalVentas DESC";
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                using (var cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@Anio", anio);
                    cn.Open();
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            string vendedor = reader.GetString(0);
                            decimal totalVentas = reader.GetDecimal(1);

                            int idx = serie.Points.AddXY(vendedor, totalVentas);
                            serie.Points[idx].LegendText = string.Format(
                            CultureInfo.GetCultureInfo("es-MX"),
                            "{0}: {1:C2}",
                            vendedor,
                            totalVentas
                            );
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
        }
    }
}
