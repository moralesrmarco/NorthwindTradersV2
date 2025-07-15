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
            Title titulo = new Title();
            if (ComboBox.SelectedIndex <= 0 & ComboBox.SelectedIndex < 1)
            {
                titulo.Text = "» Top 10 productos más vendidos «";
                GroupBox.Text = "» Top 10 productos más vendidos «";
            }
            else
            {
                titulo.Text = $"» Top {ComboBox.Text} productos más vendidos «";
                GroupBox.Text = $"» Top {ComboBox.Text} productos más vendidos «";
            }
            titulo.Font = new Font("Arial", 14, FontStyle.Bold);
            titulo.ForeColor = Color.DarkBlue;
            titulo.Alignment = ContentAlignment.TopCenter;
            var datos = ObtenerTopProductos(cantidad);
            ChartTopProductos.Titles.Add(titulo);
            ChartTopProductos.Series.Add("Productos más vendidos");
            ChartTopProductos.Series["Productos más vendidos"].ChartType = SeriesChartType.Column;
            ChartTopProductos.Series["Productos más vendidos"].IsValueShownAsLabel = true;
            ChartTopProductos.Series["Productos más vendidos"].Label = "#VALY{n0}";
            ChartTopProductos.Series["Productos más vendidos"].BorderWidth = 2;
            ChartTopProductos.Series["Productos más vendidos"].ToolTip = "Producto: #VALX, Cantidad vendida: #VALY{n0}"; 
            ChartTopProductos.Series["Productos más vendidos"].Points.Clear();
            foreach (DataRow row in datos.Rows)
            {
                string nombreProducto = row["NombreProducto"].ToString();
                int cantidadVendida = Convert.ToInt32(row["CantidadVendida"]);
                ChartTopProductos.Series["Productos más vendidos"].Points.AddXY(nombreProducto, cantidadVendida);
            }
            // 1. Obtener el área de dibujo
            var area = ChartTopProductos.ChartAreas[0];

            // 2. Habilitar 3D y ajustar ángulo/rotación/profundidad
            area.Area3DStyle.Enable3D = true;
            area.Area3DStyle.Inclination = 30;
            area.Area3DStyle.Rotation = 20;
            area.Area3DStyle.LightStyle = LightStyle.Realistic;

            area.AxisY.LabelStyle.Format = "N0"; // Formato de número sin decimales
            area.AxisX.Interval = 1; // Forzar cada producto
            area.AxisX.LabelStyle.Angle = -45; // Rotar etiquetas del eje X
            area.AxisX.Title = "Productos";
            area.AxisY.Title = "Cantidad Vendida";
            area.AxisX.MajorGrid.Enabled = true;
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
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
            using (var cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                using (var dap = new SqlDataAdapter(query, cn))
            {
                dap.SelectCommand.Parameters.AddWithValue("@Cantidad", cantidad);
                dap.Fill(dt);
            }
            return dt;
        }
    }
}
