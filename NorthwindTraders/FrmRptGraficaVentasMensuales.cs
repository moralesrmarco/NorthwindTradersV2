using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{

    public partial class FrmRptGraficaVentasMensuales : Form
    {

        public FrmRptGraficaVentasMensuales()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptGraficaVentasMensuales_Load(object sender, EventArgs e)
        {
            LlenarCmbVentasMensualesDelAño();
        }

        private void LlenarCmbVentasMensualesDelAño()
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
                                CmbVentasMensualesDelAño.Items.Add(year);
                            }
                        }
                    }
                }
                CmbVentasMensualesDelAño.SelectedIndex = 0; // Selecciona el primer elemento
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

        private void CmbVentasMensualesDelAño_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = GetTableGrafica(Convert.ToInt32(CmbVentasMensualesDelAño.SelectedItem));
            if (dt != null)
            {
                groupBox1.Text = $"» Reporte gráfico de ventas mensuales del año {CmbVentasMensualesDelAño.SelectedItem} «";
                // 1. Limpia fuentes previas
                reportViewer1.LocalReport.DataSources.Clear();
                // 2. Usa el nombre EXACTO del DataSet del RDLC
                var rds = new ReportDataSource("DataSet1", dt);
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.LocalReport.SetParameters(new ReportParameter("Anio", CmbVentasMensualesDelAño.SelectedItem.ToString()));
                reportViewer1.LocalReport.SetParameters(new ReportParameter("Subtitulo", $"Ventas mensuales del año {CmbVentasMensualesDelAño.SelectedItem}"));
                // 3. Refresca el reporte
                reportViewer1.RefreshReport();
            }
        }
       
        private DataTable GetTableGrafica(int año)
        {
            DataTable tbl = new DataTable();
            const string query = @"
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
                                    VentasMensuales AS (
                                    SELECT 
                                        MONTH(o.OrderDate) AS Mes,
                                        SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)) AS Total
                                    FROM Orders o
                                    INNER JOIN [Order Details] od ON o.OrderID = od.OrderID
                                    WHERE YEAR(o.OrderDate) = @Año
                                    GROUP BY MONTH(o.OrderDate)
                                    )
                                    SELECT m.Mes, ISNULL(v.Total, 0) AS Total, m.NombreMes
                                    FROM Meses AS m
                                    LEFT JOIN VentasMensuales AS v
                                    ON m.Mes = v.Mes
                                    ORDER BY m.Mes;
                                ";
            try
            {
                using (var cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    using (var cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@Año", año);
                        cn.Open();
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(tbl);
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
            return tbl;
        }
    }
}
