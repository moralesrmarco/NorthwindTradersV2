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
    public partial class FrmRptGraficaVentasAnuales : Form
    {
        public FrmRptGraficaVentasAnuales()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptGraficaVentasAnuales_Load(object sender, EventArgs e)
        {
            LlenarCmbVentasAnuales();
        }

        private void LlenarCmbVentasAnuales()
        {
            var items = new List<KeyValuePair<string, int>>();
            for (int i = 2; i <= 8; i++)
            {
                items.Add(new KeyValuePair<string, int>($"{i} Años ", i));
            }
            CmbVentasAnuales.DataSource = items;
            CmbVentasAnuales.DisplayMember = "Key";
            CmbVentasAnuales.ValueMember = "Value";
            CmbVentasAnuales.SelectedIndex = 0;
        }

        private void CmbVentasAnuales_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CargarComparativoVentasAnuales(int years)
        {

        }

        private DataTable GetVentasComparativas(List<int> años)
        {
            DataTable dt = new DataTable();
            // Construir cláusula de filtro dinámico
            string filtroAños = string.Join(",", años);

            string query = $@"
                                    WITH Meses AS (
                                    SELECT 1 AS Mes, 'Ene' AS NombreMes
                                    UNION ALL SELECT 2, 'Feb'
                                    UNION ALL SELECT 3, 'Mar'
                                    UNION ALL SELECT 4, 'Abr'
                                    UNION ALL SELECT 5, 'May'
                                    UNION ALL SELECT 6, 'Jun'
                                    UNION ALL SELECT 7, 'Jul'
                                    UNION ALL SELECT 8, 'Ago'
                                    UNION ALL SELECT 9, 'Sep'
                                    UNION ALL SELECT 10, 'Oct'
                                    UNION ALL SELECT 11, 'Nov'
                                    UNION ALL SELECT 12, 'Dic'
                                    ),                            
                                    Ventas AS (
                                    SELECT 
                                        YEAR(o.OrderDate) AS Año,
                                        MONTH(o.OrderDate) AS Mes,
                                        SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)) AS Total
                                    FROM Orders AS o
                                    INNER JOIN [Order Details] AS od
                                        ON o.OrderID = od.OrderID
                                    WHERE YEAR(o.OrderDate) IN ({filtroAños})
                                    GROUP BY YEAR(o.OrderDate), MONTH(o.OrderDate)
                                    )
                                    SELECT m.Mes, m.NombreMes, y.Año, ISNULL(v.Total, 0) AS Total
                                    FROM Meses AS m
                                    CROSS JOIN (SELECT DISTINCT Año FROM Ventas) As y
                                    LEFT JOIN Ventas As v ON m.Mes = v.Mes AND y.Año = v.Año
                                    ORDER BY m.Mes, y.Año;
                                    ";
            try
            {
                using (var cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    using (var cmd = new SqlCommand(query, cn))
                    {
                        cn.Open();
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
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
            return dt;
        }
    }
}
