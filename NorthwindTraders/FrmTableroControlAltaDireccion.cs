using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NorthwindTraders
{
    public partial class FrmTableroControlAltaDireccion : Form
    {

        private readonly string[] categorias = { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"};
        private readonly double[] valores = { 15, 30, 45, 20, 35, 50, 25, 40, 45, 40, 30, 50 };

        public FrmTableroControlAltaDireccion()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }
        
        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmTableroControlAltaDireccion_Load(object sender, EventArgs e)
        {
            LlenarCmbTipoGrafica();
            LlenarCmbVentasMensualesDelAnio();
            //DibujarGraficaChart6((SeriesChartType)cmbTipoGrafica.SelectedItem);

        }

        private void LlenarCmbVentasMensualesDelAnio()
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
                                cmbVentasMensualesDelAnio.Items.Add(year);
                            }
                        }
                    }
                }
                cmbVentasMensualesDelAnio.SelectedIndex = 0; // Selecciona el primer elemento
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

        private void cmbVentasMensualesDelAnio_SelectedIndexChanged(object sender, EventArgs e)
        {
            DibujarGraficaChart1();
        }

        private void DibujarGraficaChart1()
        {
            CargarVentasMensuales(Convert.ToInt32(cmbVentasMensualesDelAnio.SelectedItem));
        }

        private void CargarVentasMensuales(int year)
        {
            // 1. Obtiene los datos ADO.NET
            var datos = ObtenerVentasMensuales(year);
            // 2. Prepara la serie del Chart
            var serie = chart1.Series["Ventas"];
            serie.Points.Clear();
            serie.ChartType = SeriesChartType.Line;
            serie.BorderWidth = 2;
            serie.ToolTip = "Ventas de #VALX: #VALY{C2}";
            serie.IsValueShownAsLabel = true;
            serie.LabelFormat = "C2"; // Formato de moneda con 2 decimales
            // 3. Agrega puntos al gráfico
            foreach (var punto in datos)
            {
                string nombreMes = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(punto.Mes);
                int index = serie.Points.AddXY(nombreMes, punto.Total);
                DataPoint dataPoint = serie.Points[index];

                dataPoint.Label = $"${punto.Total:#,##0.00}";
                dataPoint.Font = new Font("Segoe UI", 7, FontStyle.Bold);
                //dataPoint.LabelForeColor = Color.DarkSlateGray;
            }
            var area = chart1.ChartAreas[0];

            // PRIMERO: forzar cada mes
            area.AxisX.Interval = 1;
            // LUEGO: asignar formato al label
            area.AxisX.LabelStyle.Angle = -45;
            area.AxisX.MajorGrid.Enabled = false;
            // Títulos de ejes
            area.AxisX.Title = "Meses";
            area.AxisX.TitleFont = new Font("Segoe UI", 7, FontStyle.Bold);
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 7, FontStyle.Regular);

            chart1.Legends[0].Enabled = false;
            area.AxisX.MajorGrid.Enabled = true;
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            area.AxisY.LabelStyle.Format = "C0";
            area.AxisY.Title = "Ventas Totales";
            area.AxisY.TitleFont = new Font("Segoe UI", 7, FontStyle.Bold);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 7, FontStyle.Regular);

            // Crear el título
            Title titulo = new Title();
            titulo.Text = $"Ventas mensuales del año: {year}";
            //titulo.Font = new Font("Segoe UI", 7, FontStyle.Bold);
            titulo.ForeColor = Color.Black;
            titulo.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            titulo.Alignment = ContentAlignment.TopCenter;

            // Agregar el título al chart
            chart1.Titles.Clear(); // Limpiar títulos previos
            chart1.Titles.Add(titulo);

            groupBox1.Text = $"» Ventas mensuales del año: {year} «";
        }

        private List<MonthlySales> ObtenerVentasMensuales(int year)
        {
            var lista = new List<MonthlySales>();
            const string sql = @"
                                WITH Meses AS (
                                SELECT 1 AS Mes UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL
                                SELECT 4 UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL
                                SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9 UNION ALL
                                SELECT 10 UNION ALL SELECT 11 UNION ALL SELECT 12
                                ),
                                VentasMensuales AS (
                                SELECT 
                                    MONTH(o.OrderDate) AS Mes,
                                    SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)) AS Total
                                FROM Orders AS o
                                INNER JOIN [Order Details] AS od
                                    ON o.OrderID = od.OrderID
                                WHERE YEAR(o.OrderDate) = @year
                                GROUP BY MONTH(o.OrderDate)
                                )
                                SELECT m.Mes, ISNULL(v.Total, 0) AS Total
                                FROM Meses AS m
                                LEFT JOIN VentasMensuales AS v
                                    ON m.Mes = v.Mes
                                ORDER BY m.Mes;
                            ";
            try
            {
                using (var cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@year", year);
                        cn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lista.Add(new MonthlySales
                                {
                                    Mes = reader.GetInt32(0),
                                    Total = Convert.ToDecimal(reader.GetValue(1))
                                });
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
            return lista;
        }

        private void LlenarCmbTipoGrafica()
        {
            // Obtiene todos los valores del enum
            var tipos = Enum.GetValues(typeof(SeriesChartType))
                            .Cast<SeriesChartType>()
                            .OrderBy(t => t.ToString());
            // Llena el ComboBox
            cmbTipoGrafica.DataSource = tipos.ToList();
        }

        private void DibujarGraficaChart6(SeriesChartType tipo)
        {
            chart6.Series.Clear();
            chart6.Titles.Clear();
            chart6.Titles.Add(new Title
            {
                Text = $"Tipo de gráfica: {tipo}",
                Docking = Docking.Top,
                Font = new Font("Segoe UI", 8, FontStyle.Bold)
            });
            var serie = new Series("Ventas")
            {
                ChartType = tipo,
                BorderWidth = 2,
                MarkerStyle = MarkerStyle.Circle,
                ToolTip = "#SERIESNAME\nMes: #AXISLABEL\nVentas: #VALY{C2}"
            };
            for (int i = 0; i < categorias.Length; i++)
            {
                serie.Points.AddXY(categorias[i], valores[i]);
            }
            chart6.Series.Add(serie);
            // Ajusta automáticamente las escalas de ejes
            chart6.ResetAutoValues();
            // Configuración del eje X
            chart6.ChartAreas[0].AxisX.LabelStyle.Angle = -45; // Inclina los labels 45 grados hacia la izquierda
            chart6.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 7); // Fuente más pequeña
            chart6.ChartAreas[0].AxisX.Interval = 1; // Asegura que se muestren todos los meses (cada categoría)
            // Configuración del eje Y
            chart6.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Segoe UI", 7); // Fuente más pequeña
            chart6.ChartAreas[0].AxisY.LabelStyle.Format = "$#,##0"; // Formato con símbolo de dólar
            double maxValor = valores.Max();
            // Configura el eje Y para que el máximo sea justo un poco mayor (opcional para espacio visual)
            chart6.ChartAreas[0].AxisY.Maximum = Math.Ceiling(maxValor * 1.0); // 5% de margen por estética
            // Si lo deseas, también puedes fijar el mínimo
            chart6.ChartAreas[0].AxisY.Minimum = 0; // Para que siempre comience en cero
            // Establecer fuente más pequeña para el nombre de la serie en la leyenda
            chart6.Legends[0].Font = new Font("Segoe UI", 7); // Tamaño de fuente reducido
        }

        private void cmbTipoGrafica_SelectedIndexChanged(object sender, EventArgs e)
        {
            DibujarGraficaChart6((SeriesChartType)cmbTipoGrafica.SelectedItem);
        }

        private class MonthlySales
        {
            public int Mes { get; set; }
            public decimal Total { get; set; }
        }

    }
}
