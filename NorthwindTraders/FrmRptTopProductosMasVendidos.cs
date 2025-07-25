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

namespace NorthwindTraders
{
    public partial class FrmRptTopProductosMasVendidos : Form
    {
        public FrmRptTopProductosMasVendidos()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptTopProductosMasVendidos_Load(object sender, EventArgs e)
        {
            LlenarCmbTopProductos();
        }

        private void LlenarCmbTopProductos()
        {
            List<KeyValuePair<string, int>> items = new List<KeyValuePair<string, int>>();
            for (int i = 10; i <= 50; i = i + 5)
            {
                items.Add(new KeyValuePair<string, int>($"{i} productos", i));
            }
            CmbTopProductos.SelectedIndexChanged -= CmbTopProductos_SelectedIndexChanged;
            CmbTopProductos.DisplayMember = "Key";
            CmbTopProductos.ValueMember = "Value";
            CmbTopProductos.DataSource = items;
            CmbTopProductos.SelectedIndex = -1;
            CmbTopProductos.SelectedIndexChanged += CmbTopProductos_SelectedIndexChanged;
            CmbTopProductos.SelectedIndex = 0;
        }

        private void CmbTopProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int topProductos = Convert.ToInt32(CmbTopProductos.SelectedValue);
            CargarTopProductos(topProductos);
        }

        private void CargarTopProductos(int topProductos)
        {
            groupBox1.Text = $"» Reporte gráfico top {topProductos} productos más vendidos «";
        }

        private DataTable GetTopProductos(int topProductos)
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
            MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
            try
            {
                using (var cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                using (var dap = new SqlDataAdapter(query, cn))
                {
                    dap.SelectCommand.Parameters.AddWithValue("@Cantidad", topProductos);
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
