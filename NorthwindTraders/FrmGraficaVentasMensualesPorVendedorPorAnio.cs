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
using System.Windows.Forms.DataVisualization.Charting;

namespace NorthwindTraders
{
    public partial class FrmGraficaVentasMensualesPorVendedorPorAnio : Form
    {
        public FrmGraficaVentasMensualesPorVendedorPorAnio()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmGraficaVentasMensualesPorVendedorPorAnio_Load(object sender, EventArgs e)
        {
            LlenarComboBox();

        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                MessageBox.Show("Seleccione un año válido.", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


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
                comboBox1.SelectedIndex = 0; // Selecciona el primer elemento
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

        private void CargarVentasMensualesPorVendedorPorAnio(int anio)
        {
            chart1.Series.Clear();
            chart1.Titles.Clear();
            Title titulo = new Title
            {
                Text = $"» Ventas mensuales por vendedor del año {anio} «",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 51, 102)
            };
            chart1.Titles.Add(titulo);
            // Configuración de la serie
            Series serie = new Series
            {
                Name = "Ventas",
                Color = Color.FromArgb(0, 51, 102),
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Column,
                Label = "#VALX: #VALY{C2}"
            };
            serie.SmartLabelStyle.Enabled = true;
            serie.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
            serie.SmartLabelStyle.CalloutLineColor = Color.Black;
            serie.LabelForeColor = Color.DarkSlateGray;
            serie.LabelBackColor = Color.WhiteSmoke;
            ChartVentasMensualesPorVendedor.Series.Add(serie);
            // Consulta SQL para obtener las ventas mensuales por vendedor
            string query = @"
                SELECT 
                    CONCAT(e.FirstName, ' ', e.LastName) AS Vendedor,
                    MONTH(o.OrderDate) AS Mes,
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
                    e.FirstName, e.LastName, MONTH(o.OrderDate)
                ORDER BY 
                    e.FirstName, e.LastName, MONTH(o.OrderDate)";
            // Conexión a la base de datos y ejecución de la consulta
            using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
            {
                SqlCommand command = new SqlCommand(query, cn);
                command.Parameters.AddWithValue("@Anio", anio);
                cn.Open();
                SqlDataReader reader = command
    }
}
