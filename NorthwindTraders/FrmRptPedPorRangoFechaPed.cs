// https://www.youtube.com/watch?v=2-YkNo1Os3Y&list=PL_1AVI-bgZKQ2MSDejVmaaxNenhETwwx_&index=7
// https://www.youtube.com/watch?v=7AvCaq7a1fc&list=PL_1AVI-bgZKQ2MSDejVmaaxNenhETwwx_&index=5
using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptPedPorRangoFechaPed: Form
    {
        public FrmRptPedPorRangoFechaPed()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptPedPorRangoFechaPed_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            MostrarReporte();
        }

        private void MostrarReporte()
        {
            string subtitulo = $"[ Fecha de pedido inicial: {dateTimePicker1.Value.ToShortDateString()} ] - [ Fecha de pedido final: {dateTimePicker2.Value.ToShortDateString()} ]";
            MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
            DataTable dt = ObtenerPedidosPorFechaPedido(dateTimePicker1.Value, dateTimePicker2.Value);
            if (dt.Rows.Count > 0)
            {
                ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                ReportParameter reportParameter = new ReportParameter("subtitulo", subtitulo);
                reportViewer1.LocalReport.SetParameters(new ReportParameter[] { reportParameter });
                reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(OrderDetailsSubReportProcessing);
                reportViewer1.RefreshReport();
            }
            else
            {
                reportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource reportDataSource = new ReportDataSource("DataSet1", new DataTable());
                reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                ReportParameter reportParameter = new ReportParameter("subtitulo", subtitulo);
                reportViewer1.LocalReport.SetParameters(new ReportParameter[] { reportParameter });
                reportViewer1.RefreshReport();
                MessageBox.Show(Utils.noDatos, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private DataTable ObtenerPedidosPorFechaPedido(DateTime from, DateTime to)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
            {
                string query = @"Select OrderDate, RequiredDate, ShippedDate, c.CompanyName, o.OrderID
                                from Orders o join Customers c on c.CustomerID = o.CustomerID
                                where OrderDate >= @from and OrderDate < @to " +
                                "order by OrderDate Desc, c.CompanyName";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.Add("@from", SqlDbType.DateTime).Value = from.Date; // Inicio del día
                cmd.Parameters.Add("@to", SqlDbType.DateTime).Value = to.Date.AddDays(1); // Final del día siguiente
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
            }
            return dt;
        }

        private void OrderDetailsSubReportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            int orderID = int.Parse(e.Parameters["OrderID"].Values[0].ToString());
            DataTable dt = ObtenerDetallePedidoPorOrderID(orderID);
            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
            e.DataSources.Add(reportDataSource);
        }

        private DataTable ObtenerDetallePedidoPorOrderID(int orderID) 
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn)) 
            {
                string query = @"Select ProductName, od.UnitPrice, od.Quantity, (od.Quantity * od.UnitPrice) as Total 
                                from [Order Details] od join products p on p.ProductId = od.ProductID
                                where OrderID = " + orderID + "";
                SqlCommand cmd = new SqlCommand(query,cn);
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows) 
                {
                    dt.Load(reader);
                }
            }
            return dt;
        }
    }
}
