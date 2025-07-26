using Microsoft.Reporting.WinForms;
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
            DataTable dt = GetTopProductos(topProductos);
            if (dt != null)
            {
                // 1. Limpia fuentes previas
                reportViewer1.LocalReport.DataSources.Clear();
                // 2. Usa el nombre EXACTO del DataSet del RDLC
                var rds = new ReportDataSource("DataSet1", dt);
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.LocalReport.SetParameters(new ReportParameter("NumProductos", CmbTopProductos.SelectedValue.ToString()));
                reportViewer1.LocalReport.SetParameters(new ReportParameter("Subtitulo", $"Top {CmbTopProductos.SelectedValue.ToString()} productos más vendidos"));
                // 3. Refresca el reporte
                reportViewer1.RefreshReport();
            }
        }

        private DataTable GetTopProductos(int topProductos)
        {
            DataTable dt = new DataTable();
            //const string query = @"
            //    SELECT TOP (@Cantidad) 
            //        p.ProductName AS NombreProducto, 
            //        SUM(od.Quantity) AS CantidadVendida
            //    FROM [Order Details] As od
            //    INNER JOIN Products AS p ON od.ProductID = p.ProductID
            //    GROUP BY p.ProductName
            //    ORDER BY SUM(od.Quantity) DESC";
            const string query = @"
                                    WITH ProductosVendidos AS (
                                    SELECT 
                                        p.ProductName AS NombreProducto, 
                                        SUM(od.Quantity) AS CantidadVendida
                                    FROM [Order Details] AS od
                                    INNER JOIN Products AS p ON od.ProductID = p.ProductID
                                    GROUP BY p.ProductName
                                    )
                                    SELECT TOP (@Cantidad)
                                    ROW_NUMBER() OVER (ORDER BY CantidadVendida DESC) AS Posicion,
                                    CONCAT(ROW_NUMBER() OVER (ORDER BY CantidadVendida DESC), '. ', NombreProducto) AS NombreProducto,
                                    CantidadVendida
                                    FROM ProductosVendidos
                                    ORDER BY CantidadVendida DESC
                                   ";        
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
