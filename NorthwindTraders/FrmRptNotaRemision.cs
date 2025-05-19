using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptNotaRemision : Form
    {

        public int Id;

        public FrmRptNotaRemision()
        {
            InitializeComponent();
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptNotaRemision_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmRptNotaRemision_Load(object sender, EventArgs e)
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                DataTable dt = ObtenerPedido(Id);
                MDIPrincipal.ActualizarBarraDeEstado($"Se encontró el Pedido con Id: {Id}");
                if (dt.Rows.Count > 0)
                {
                    ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                    reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(OrderDetailsSubReportProcessing);
                    reportViewer1.RefreshReport();
                }
                else
                {
                    reportViewer1.LocalReport.DataSources.Clear();
                    ReportDataSource reportDataSource = new ReportDataSource("DataSet1", new DataTable());
                    reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                    reportViewer1.RefreshReport();
                    MessageBox.Show(Utils.noDatos, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex) { Utils.MsgCatchOue(ex); }
        }

        private DataTable ObtenerPedido(int id)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    SqlCommand cmd = new SqlCommand("SP_PEDIDOS_NOTAREMISION", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("PedidoId", id);
                    SqlDataAdapter dap = new SqlDataAdapter(cmd);
                    dap.Fill(dt);
                }
            }
            catch (SqlException ex) { Utils.MsgCatchOueclbdd(ex); }
            catch (Exception ex) { Utils.MsgCatchOue(ex); }
            return dt;
        }

        private void OrderDetailsSubReportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            int orderId = int.Parse(e.Parameters["OrderID"].Values[0].ToString());
            DataTable dt = ObtenerDetallePedidoPorOrderID(orderId);
            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
            e.DataSources.Add(reportDataSource);
        }

        private DataTable ObtenerDetallePedidoPorOrderID(int orderID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    string query = @"Select ProductName, od.UnitPrice, od.Quantity, od.Discount, 
                                    (od.Quantity * od.UnitPrice) * ( 1 - od.Discount ) as Total 
                                    from [Order Details] od join products p on p.ProductId = od.ProductID
                                    where OrderID = " + orderID;
                    SqlCommand cmd = new SqlCommand(query, cn);
                    SqlDataAdapter dap = new SqlDataAdapter(cmd);
                    dap.Fill(dt);
                }
            }
            catch (SqlException ex) { Utils.MsgCatchOueclbdd(ex); }
            catch (Exception ex) { Utils.MsgCatchOue(ex); }
            return dt;
        }
    }
}
