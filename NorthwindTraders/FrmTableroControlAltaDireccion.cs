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
            LlenarCmbUltimosAnios();
            LlenarCmbNumeroProductos();
            CargarVentasPorVendedores();
            LlenarCmbVentasVendedorAnio();
        }

        private void LlenarCmbVentasVendedorAnio()
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
                                cmbVentasVendedorAnio.Items.Add(year);
                            }
                        }
                    }
                }
                cmbVentasVendedorAnio.SelectedIndex = 0; // Selecciona el primer elemento
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

        private void cmbVentasVendedorAnio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVentasVendedorAnio.SelectedIndex < 0)
                return;
            CargarVentasPorVendedoresAnio(Convert.ToInt32(cmbVentasVendedorAnio.SelectedItem));
        }

        private void CargarVentasPorVendedoresAnio(int anio)
        {
            chart5.Series.Clear();
            chart5.Titles.Clear();
            chart5.Legends.Clear();
            var leyenda = new Legend("Vendedores")
            {
                Title = "Vendedores",
                TitleFont = new Font("Segoe UI", 7, FontStyle.Bold),
                Docking = Docking.Right,
                LegendStyle = LegendStyle.Table,
                Font = new Font("Segoe UI", 7, FontStyle.Regular),
                IsTextAutoFit = false
            };

            chart5.Legends.Add(leyenda);
            // Título del gráfico
            Title titulo = new Title
            {
                Text = $"» Gráfica de ventas por vendedores del año {anio} «",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
            };
            chart5.Titles.Add(titulo);
            groupBox5.Text = titulo.Text; // Actualizar el texto del GroupBox
            Series serie = new Series
            {
                Name = "Ventas",
                Color = Color.FromArgb(0, 51, 102),
                IsValueShownAsLabel = false,
                ChartType = SeriesChartType.Doughnut,
                Label = "#VALX: #VALY{C2}",
                ToolTip = "Vendedor: #AXISLABEL\nTotal ventas: #VALY{C2}",
                Legend = leyenda.Name,
                LegendText = "#VALX: #VALY{C2}"
            };

            // 1. Configurar ChartArea 3D
            var area = chart5.ChartAreas[0];
            area.Area3DStyle.Enable3D = true;
            area.Area3DStyle.Inclination = 40;
            area.Area3DStyle.Rotation = 60;
            area.Area3DStyle.LightStyle = LightStyle.Realistic;
            area.Area3DStyle.WallWidth = 0;

            // Opciones 3D y estilo dona
            serie["PieLabelStyle"] = "Disabled";
            serie["PieDrawingStyle"] = "Cylinder";
            serie["DoughnutRadius"] = "60";

            // 3.Agregar la serie al chart
            chart5.Series.Clear();
            chart5.Series.Add(serie);

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
                            // Si quieres, refuerzas aquí:
                            //serie.Points[idx].LegendText = $"{vendedor}, {totalVentas:c2}";
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

        private void CargarVentasPorVendedores()
        {
            chart4.Series.Clear();
            chart4.Titles.Clear();
            chart4.Titles.Add(new Title
            {
                Text = "» Gráfica ventas por vendedores de todos los años «",
                Docking = Docking.Top,
                Font = new Font("Segoe UI", 8, FontStyle.Bold)
            });
            // 1. Configurar ChartArea 3D
            var area = chart4.ChartAreas[0];
            area.Area3DStyle.Enable3D = true;
            area.Area3DStyle.Inclination = 40;
            area.Area3DStyle.Rotation = 60;
            area.Area3DStyle.LightStyle = LightStyle.Realistic;
            area.Area3DStyle.WallWidth = 0;
            // Configuración de la serie
            Series serie = new Series
            {
                Name = "Ventas",
                Color = Color.FromArgb(0, 51, 102),
                IsValueShownAsLabel = false,
                ChartType = SeriesChartType.Doughnut,
                Label = "#VALX, #VALY{c2}",
                ToolTip = "Vendedor: #VALX\nTotal Ventas: #VALY{C2}"
            };
            serie.Points.Clear();
            serie.SmartLabelStyle.Enabled = true;
            serie.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No;
            serie.SmartLabelStyle.CalloutLineColor = Color.Black;
            serie.LabelForeColor = Color.DarkSlateGray;
            serie.LabelBackColor = Color.WhiteSmoke;
            serie["PieLabelStyle"] = "Disabled";
            serie["PieDrawingStyle"] = "Cylinder";
            serie["DoughnutRadius"] = "60";
            chart4.Series.Add(serie);
            // Consulta SQL para obtener las ventas por vendedor
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
            var legend = chart4.Legends[0];
            legend.Font = new Font("Segoe UI", 7, FontStyle.Regular);
        }

        private void LlenarCmbNumeroProductos()
        {
            cmbNumeroProductos.SelectedIndexChanged -= cmbNumeroProductos_SelectedIndexChanged;
            var items = new List<KeyValuePair<string, int>>();
            for (int i = 10; i <= 30; i += 5)
            {
                items.Add(new KeyValuePair<string, int>($"{i} productos", i));
            }
            cmbNumeroProductos.DataSource = items;
            cmbNumeroProductos.DisplayMember = "Key";
            cmbNumeroProductos.ValueMember = "Value";
            cmbNumeroProductos.SelectedIndexChanged += cmbNumeroProductos_SelectedIndexChanged;
            // Forzar un disparo aunque ya esté en 0, primero pon un índice diferente y luego vuelve a 0
            cmbNumeroProductos.SelectedIndex = -1; // No selecciona ningún elemento inicialmente
            cmbNumeroProductos.SelectedIndex = 0; // Selecciona el primer elemento
        }

        private void cmbNumeroProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarTopProductos(Convert.ToInt32(cmbNumeroProductos.SelectedValue));
        }

        private void CargarTopProductos(int cantidad)
        {
            chart3.Series.Clear();
            chart3.Titles.Clear();
            chart3.Titles.Add(new Title
            {
                Text = $"Top {cantidad} productos más vendidos",
                Docking = Docking.Top,
                Font = new Font("Segoe UI", 8, FontStyle.Bold)
            });
            groupBox3.Text = $"» Top {cantidad} productos más vendidos «";
            var series = new Series("Productos")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true,
                Label = "#VALY{n0}",
                LabelFormat = "C2",
                BorderWidth = 2,
                ToolTip = "Producto: #VALX,\nCantidad vendida: #VALY{n0}",
                Font = new Font("Segoe UI", 7, FontStyle.Bold)
            };
            series.Points.Clear();
            // Paleta de 10 colores (ajusta a tu gusto)
            Color[] paleta = {
                Color.SteelBlue, Color.Orange, Color.MediumSeaGreen,
                Color.Goldenrod, Color.Crimson, Color.MediumPurple,
                Color.Tomato, Color.Teal, Color.SlateGray, Color.DeepPink
            };
            var productos = ObtenerTopProductos(cantidad);
            int idx = 0;
            foreach (DataRow row in productos.Rows)
            {
                string nombre = (idx + 1).ToString() + ".- " + row["NombreProducto"].ToString();
                int qty = Convert.ToInt32(row["CantidadVendida"]);

                int pointIndex = series.Points.AddXY(nombre, qty);
                series.Points[pointIndex].Color = paleta[idx % paleta.Length];
                idx++;
            }
            chart3.Series.Add(series);
            chart3.Legends.Clear();
            var area = chart3.ChartAreas[0];
            area.Area3DStyle.Enable3D = true;
            area.Area3DStyle.Inclination = 30;
            area.Area3DStyle.Rotation = 20;
            area.Area3DStyle.LightStyle = LightStyle.Realistic;

            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Angle = -45;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 7, FontStyle.Regular);
            //area.AxisX.Title = "Productos más vendidos";
            area.AxisX.MajorGrid.Enabled = true;
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            area.AxisY.LabelStyle.Format = "N0";
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 7, FontStyle.Regular);
            area.AxisY.Title = "Cantidad vendida (unidades)";
            area.AxisY.TitleFont = new Font("Segoe UI", 8, FontStyle.Regular);
            area.AxisY.MajorGrid.Enabled = true;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
        }

        private DataTable ObtenerTopProductos(int cantidad)
        {
            DataTable dt = new DataTable();
            const string query = @"
                SELECT TOP (@Cantidad) 
                    p.ProductName AS NombreProducto, 
                    SUM(od.Quantity) AS CantidadVendida
                FROM [Order Details] As od
                INNER JOIN Products AS p ON od.ProductID = p.ProductID
                GROUP BY p.ProductName
                ORDER BY CantidadVendida DESC";
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                using (var cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                using (var dap = new SqlDataAdapter(query, cn))
                {
                    dap.SelectCommand.Parameters.AddWithValue("@Cantidad", cantidad);
                    dap.Fill(dt);
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

        private void LlenarCmbUltimosAnios()
        {
            cmbUltimosAnios.SelectedIndexChanged -= cmbUltimosAnios_SelectedIndexChanged;
            var items = new List<KeyValuePair<string, int>>();
            for (int i = 2; i <= 8; i++)
            {
                items.Add(new KeyValuePair<string, int>($"{i} Años ", i));
            }
            cmbUltimosAnios.DataSource = items;
            cmbUltimosAnios.DisplayMember = "Key";
            cmbUltimosAnios.ValueMember = "Value";
            cmbUltimosAnios.SelectedIndexChanged += cmbUltimosAnios_SelectedIndexChanged;
            // forzar un disparo aunque ya esté en 0, primero pon un índice diferente y luego vuelve a 0
            cmbUltimosAnios.SelectedIndex = -1; // No selecciona ningún elemento inicialmente
            cmbUltimosAnios.SelectedIndex = 0; // Selecciona el primer elemento
        }

        private void cmbUltimosAnios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbUltimosAnios.SelectedValue) >= 6)
            {
                MessageBox.Show("Solo existen datos en la base de datos hasta el año 1996", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            CargarComparativoVentasMensuales(Convert.ToInt32(cmbUltimosAnios.SelectedValue));
        }

        private void CargarComparativoVentasMensuales(int years)
        {
            chart2.Series.Clear();
            chart2.Titles.Clear(); // Limpiar títulos previos
            int yearActual = DateTime.Now.Year;
            for (int i = 1; i <= years; i++)
            {
                if (yearActual == 2023)
                    yearActual = 1998; // Si el año actual es 2023, se inicia desde 1998
                else if (yearActual == 1995)
                    break;
                chart2.Series.Add($"Ventas {yearActual}");
                chart2.Series[$"Ventas {yearActual}"].ChartType = SeriesChartType.Line;
                chart2.Series[$"Ventas {yearActual}"].IsValueShownAsLabel = false;
                chart2.Series[$"Ventas {yearActual}"].Label = "#VALY{C}"; // Formato de moneda
                chart2.Series[$"Ventas {yearActual}"].BorderWidth = 2;
                chart2.Series[$"Ventas {yearActual}"].LegendText = $"Ventas {yearActual}"; // Leyenda personalizada
                chart2.Series[$"Ventas {yearActual}"].ToolTip = "#LEGENDTEXT\nde #AXISLABEL:\n#VALY{C2}"; // tooltip con moneda y 2 decimales

                chart2.Series[$"Ventas {yearActual}"].Points.Clear();
                // 2. Obtiene los datos ADO.NET
                var datos = ObtenerVentasMensualesComparativo(yearActual);
                //chart2.Legends["Default"].Font = new Font("Segoe UI", 7, FontStyle.Regular);
                if (chart2.Legends.Count > 0)
                    chart2.Legends[0].Font = new Font("Segoe UI", 7, FontStyle.Regular);
                // 3. Agrega los puntos al gráfico
                foreach (var dato in datos)
                {
                    string nombreMes = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dato.Mes);
                    int index = chart2.Series[$"Ventas {yearActual}"].Points.AddXY(nombreMes, dato.Total);
                    DataPoint dataPoint = chart2.Series[$"Ventas {yearActual}"].Points[index];
                    if (dato.Total != 0)
                    {
                        dataPoint.Label = $"${dato.Total:#,##0.00}"; // Formato de moneda con 2 decimales
                        dataPoint.Font = new Font("Segoe UI", 7, FontStyle.Regular);
                        dataPoint.MarkerStyle = MarkerStyle.Circle; // Estilo de marcador
                        dataPoint.MarkerSize = 10;
                    }
                    else
                    {
                        dataPoint.Label = ""; // No mostrar etiqueta si el total es 0
                    }
                }
                yearActual--;
            }
            var area = chart2.ChartAreas[0];
            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Angle = -45;
            area.AxisX.Title = "Meses";
            area.AxisX.TitleFont = new Font("Segoe UI", 7, FontStyle.Bold);
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 7, FontStyle.Regular);

            area.AxisX.MajorGrid.Enabled = true;
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            area.AxisY.Title = "Ventas Totales";
            area.AxisY.LabelStyle.Format = "C0";
            area.AxisY.TitleFont = new Font("Segoe UI", 7, FontStyle.Bold);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 7, FontStyle.Regular);

            // Crear el título
            Title titulo = new Title();
            if (cmbUltimosAnios.SelectedIndex <= 0 & cmbUltimosAnios.SelectedIndex < 1)
            {
                titulo.Text = "» Comparativo de ventas mensuales de los últimos 2 años «";
                groupBox2.Text = "» Comparativo de ventas mensuales de los últimos 2 años «";
            }
            else
            {
                titulo.Text = $"» Comparativo de ventas mensuales de los últimos {cmbUltimosAnios.Text} «";
                groupBox2.Text = $"» Comparativo de ventas mensuales de los últimos {cmbUltimosAnios.Text} «";
            }
            titulo.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            titulo.Alignment = ContentAlignment.TopCenter;

            // Agregar el título al chart
            chart2.Titles.Add(titulo);
        }

        private List<MonthlySales> ObtenerVentasMensualesComparativo(int year)
        {
            var ventasMensuales = new List<MonthlySales>();
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
                                ventasMensuales.Add(new MonthlySales
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
            return ventasMensuales;
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
                MarkerSize = 10,
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
