using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
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
            CargarVentasMensualesPorVendedorPorAnio(DateTime.Now.Year);
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                MessageBox.Show("Seleccione un año válido.", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            CargarVentasMensualesPorVendedorPorAnio(Convert.ToInt32(comboBox1.SelectedItem));
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
                Text = $"» Ventas mensuales por vendedores del año {anio} «",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 51, 102)
            };
            groupBox1.Text = titulo.Text; 
            // ChartArea
            var area = chart1.ChartAreas[0];
            area.AxisX.Interval = 1;
            area.AxisX.CustomLabels.Clear();

            // Genera etiquetas para cada mes
            for (int i = 1; i <= 12; i++)
            {
                var label = new CustomLabel
                {
                    FromPosition = i - 0.5,
                    ToPosition = i + 0.5,
                    Text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(i) // “Ene”, “Feb”, …
                };
                area.AxisX.CustomLabels.Add(label);
            }
            area.AxisX.Title = "Meses";
            area.AxisY.Title = "Ventas totales";
            area.AxisY.LabelStyle.Format = "C0";
            string query = @"
                SELECT 
                    CONCAT(e.FirstName, ' ', e.LastName) AS Vendedor,
                    MONTH(o.OrderDate) AS Mes,
                    SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)) AS TotalVentas
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

            // Leer datos
            var dt = new DataTable();
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                using (var cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    using (var da = new SqlDataAdapter(query, cn))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@Anio", anio);
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

            // Pivot dinámico por vendedor
            var grupos = dt.AsEnumerable()
                          .GroupBy(r => r.Field<string>("Vendedor"));

            // Nombres abreviados de mes (12 elementos)
            var mesesAbrev = CultureInfo.CurrentCulture
                                        .DateTimeFormat
                                        .AbbreviatedMonthNames
                                        .Take(12)
                                        .ToArray();
            foreach (var grupo in grupos)
            {
                // Serie por vendedor
                var serie = new Series(grupo.Key)
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 6,
                    ToolTip = "#SERIESNAME\nMes: #AXISLABEL\nVentas: #VALY{C2}",
                    LabelForeColor = Color.Black,
                    Font = new Font("Segoe UI", 8f, FontStyle.Regular),
                    IsValueShownAsLabel = false,
                    LabelFormat = "C2",
                };

                // Inicializamos meses 1–12 en caso de faltantes
                // Inicializo 12 puntos con el nombre del mes como etiqueta X
                for (int mes = 1; mes <= 12; mes++)
                {
                    string nombreMes = mesesAbrev[mes - 1];
                    serie.Points.AddXY(nombreMes, 0D);
                }
                // Llenamos datos reales
                foreach (var row in grupo)
                {
                    // 1) Obtienes el mes
                    int mes = row.Field<int>("Mes");       // 1–12

                    // 2) Tomas el valor crudo y lo conviertes a double
                    object raw = row["TotalVentas"];
                    double ventas = raw != DBNull.Value
                                    ? Convert.ToDouble(raw)
                                    : 0D;

                    // 3) Asignas el valor al punto correspondiente
                    serie.Points[mes - 1].YValues[0] = ventas;
                }

                // ← Aquí: filtro para mostrar etiqueta solo si Y > 0
                foreach (DataPoint p in serie.Points)
                {
                    if (p.YValues[0] > 0)
                    {
                        p.IsValueShownAsLabel = true;                        
                    }
                }

                chart1.Series.Add(serie);
            }
            // ————— Aquí forzamos el recálculo de la escala del eje Y —————
            chart1.ResetAutoValues();
        }
    }
}
