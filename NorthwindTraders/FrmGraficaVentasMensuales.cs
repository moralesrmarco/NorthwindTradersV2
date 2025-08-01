﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace NorthwindTraders
{
    public partial class FrmGraficaVentasMensuales : Form
    {
        public FrmGraficaVentasMensuales()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmGraficaVentasMensuales_Load(object sender, EventArgs e)
        {
            LlenarComboBox();
            CargarVentasMensuales(DateTime.Now.Year);
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            if (ComboBox.SelectedIndex == 0)
            {
                MessageBox.Show("Seleccione un año válido.", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            CargarVentasMensuales(Convert.ToInt32(ComboBox.SelectedItem));
        }

        private void LlenarComboBox()
        {
            MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
            ComboBox.Items.Add("»--- Seleccione ---«");
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
                                ComboBox.Items.Add(year);
                            }
                        }
                    }
                }
                ComboBox.SelectedIndex = 0; // Selecciona el primer elemento
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

        private void CargarVentasMensuales(int year)
        {
            // 1. Obtiene los datos ADO.NET
            var datos = ObtenerVentasMensuales(year);
            // 2. Prepara la serie del Chart
            var serie = chartVentas.Series["Ventas mensuales"];
            serie.Points.Clear();
            serie.ChartType = SeriesChartType.Line;
            serie.BorderWidth = 3;
            serie.ToolTip = "Ventas de #VALX: #VALY{C2}";
            serie.IsValueShownAsLabel = true;
            serie.LabelFormat = "C2"; // Formato de moneda con 2 decimales
            serie.MarkerStyle = MarkerStyle.Circle;
            serie.MarkerSize = 10;
            // 3. Agrega puntos al gráfico
            foreach (var punto in datos)
            {
                string nombreMes = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(punto.Mes);
                serie.Points.AddXY(nombreMes, punto.Total);
            }
            var area = chartVentas.ChartAreas[0];

            // Formato de moneda sin decimales (“$12,345”)

            // PRIMERO: forzar cada mes
            area.AxisX.Interval = 1;
            // LUEGO: asignar formato al label
            area.AxisX.LabelStyle.Angle = -45;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.Title = "Meses";

            chartVentas.Legends[0].Enabled = false;
            area.AxisX.MajorGrid.Enabled = true;
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            area.AxisY.LabelStyle.Format = "C0";
            area.AxisY.Title = "Ventas Totales";
            area.AxisY.MajorGrid.Enabled = true;
            area.AxisY.MajorGrid.LineColor = Color.Gray;
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid;
            area.AxisY.MinorGrid.Enabled = true;
            area.AxisY.MinorGrid.LineColor = Color.LightGray;
            area.AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dash;

            // Crear el título
            Title titulo = new Title();
            titulo.Text = $"Ventas mensuales del año: {year}";
            titulo.Font = new Font("Arial", 14, FontStyle.Bold);
            titulo.ForeColor = Color.DarkBlue;
            titulo.Alignment = ContentAlignment.TopCenter;

            decimal totalVentas = datos.Sum(x => x.Total);
            Title subTitulo = new Title();
            subTitulo.Text = $"Total de ventas del año {year}: {totalVentas:C2}";
            subTitulo.Docking = Docking.Top;
            subTitulo.Font = new Font("Arial", 8, FontStyle.Bold);
            subTitulo.Alignment = ContentAlignment.TopRight;
            //subTitulo.DockingOffset = 30;
            subTitulo.IsDockedInsideChartArea = false;

            // Agregar el título al chart
            chartVentas.Titles.Clear(); // Limpiar títulos previos
            chartVentas.Titles.Add(titulo);
            chartVentas.Titles.Add(subTitulo);

            GroupBox.Text = $"» Ventas mensuales del año: {year} «";
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

        private class MonthlySales
        {
            public int Mes { get; set; }
            public decimal Total { get; set; }
        }
    }
}
