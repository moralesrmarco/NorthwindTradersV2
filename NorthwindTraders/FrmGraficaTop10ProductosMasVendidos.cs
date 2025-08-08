using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NorthwindTraders
{
    public partial class FrmGraficaTop10ProductosMasVendidos : Form
    {
        public FrmGraficaTop10ProductosMasVendidos()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmGraficaTop10ProductosMasVendidos_Load(object sender, EventArgs e)
        {
            LlenarComboBox();
            GroupBox.Text = $"» Top 10 productos más vendidos «";
            CargarTopProductos(10); // Cargar los 10 productos por defecto
        }

        private void LlenarComboBox()
        {
            var items = new List<KeyValuePair<string, int>>();
            items.Add(new KeyValuePair<string, int>("»--- Seleccione ---«", 0));
            for (int i = 10; i <= 30; i = i + 5)
            {
                items.Add(new KeyValuePair<string, int>($"{i} productos", i));
            }
            ComboBox.DataSource = items;
            ComboBox.DisplayMember = "Key";
            ComboBox.ValueMember = "Value";
        }

        private void BtnMostrar_Click(object sender, EventArgs e)
        {
            if (ComboBox.SelectedIndex == 0)
            {
                MessageBox.Show("Seleccione un número de productos válido.", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            CargarTopProductos(Convert.ToInt32(ComboBox.SelectedValue));
        }

        private void CargarTopProductos(int cantidad)
        {
            ChartTopProductos.Series.Clear();
            ChartTopProductos.Titles.Clear();

            // Título y GroupBox
            Title titulo = new Title();
            if (cantidad <= 0)
            {
                titulo.Text = "Top 10 productos más vendidos";
                GroupBox.Text = $"» {titulo.Text} «";
            }
            else
            {
                titulo.Text = $"Top {cantidad} productos más vendidos";
                GroupBox.Text = $"» {titulo.Text} «";
            }
            titulo.Font = new Font("Arial", 14, FontStyle.Bold);
            titulo.Alignment = ContentAlignment.TopCenter;
            ChartTopProductos.Titles.Add(titulo);

            // Datos
            var datos = ObtenerTopProductos(cantidad);

            // 1 serie única
            var series = ChartTopProductos.Series.Add("Productos más vendidos");
            series.ChartType = SeriesChartType.Column;
            series.IsValueShownAsLabel = true;
            series.Label = "#VALY{n0}";
            series.BorderWidth = 2;
            series.ToolTip = "Producto: #VALX, Cantidad vendida: #VALY{n0}";
            series.Font = new Font("Arial", 10, FontStyle.Bold);
            series.Points.Clear();

            // Paleta de 10 colores (ajusta a tu gusto)
                Color[] paleta = {
                Color.SteelBlue, Color.Orange, Color.MediumSeaGreen,
                Color.Goldenrod, Color.Crimson, Color.MediumPurple,
                Color.Tomato, Color.Teal, Color.SlateGray, Color.DeepPink
            };

            // Agregar puntos asignando color a cada uno
            int idx = 0;
            foreach (DataRow row in datos.Rows)
            {
                string nombre = (idx + 1).ToString() + ".- " + row["NombreProducto"].ToString();
                int qty = Convert.ToInt32(row["CantidadVendida"]);

                int pointIndex = series.Points.AddXY(nombre, qty);
                series.Points[pointIndex].Color = paleta[idx % paleta.Length];
                idx++;
            }

            ChartTopProductos.Legends.Clear();

            // Configurar ChartArea en 3D y ejes
            var area = ChartTopProductos.ChartAreas[0];

            area.Area3DStyle.Enable3D = true;
            area.Area3DStyle.Inclination = 30;
            area.Area3DStyle.Rotation = 20;
            area.Area3DStyle.LightStyle = LightStyle.Realistic;

            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Angle = -45;
            area.AxisX.Title = "Productos más vendidos";
            area.AxisX.MajorGrid.Enabled = true;
            area.AxisX.MajorGrid.LineColor = Color.Black;
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            area.AxisY.LabelStyle.Format = "N0";
            area.AxisY.LabelStyle.Angle = -45;
            area.AxisY.LabelStyle.Font = new Font("Arial", 8, FontStyle.Regular);
            area.AxisY.Title = "Cantidad vendida (unidades)";
            area.AxisY.MajorGrid.Enabled = true;
            area.AxisY.MajorGrid.LineColor = Color.Black;
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            area.AxisY.MinorGrid.Enabled = true;
            area.AxisY.MinorGrid.LineColor = Color.Black;
            area.AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dash;
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
                ORDER BY CantidadVendida DESC
                ";
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
    }
}
