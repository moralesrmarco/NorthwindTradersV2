using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptNotaRemision8 : Form
    {

        public int Id = 11077;

        public FrmRptNotaRemision8()
        {
            InitializeComponent();
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptNotaRemision8_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmRptNotaRemision8_Load(object sender, EventArgs e)
        {
            ReportParameter[] parameters = new ReportParameter[2];
            parameters[0] = new ReportParameter("PedidoId", Id.ToString());
            parameters[1] = new ReportParameter("Para", "2"); // este parametro ya no se utiliza, pero si lo quito deja de funcionar el informe
            this.reportViewer1.LocalReport.SetParameters(parameters);
            DataTable dt2 = ObtenerPedido(Id);
            ReportDataSource rds2 = new ReportDataSource("DataSet2", dt2);
            reportViewer1.LocalReport.DataSources.Add(rds2);
            // Obtenemos los datos detallados para la nota de remisión específica  
            DataTable dt3 = ObtenerDetallePedidoPorOrderID(Id);
            ReportDataSource rds3 = new ReportDataSource("DataSet3", dt3);
            reportViewer1.LocalReport.DataSources.Add(rds3);
            reportViewer1.LocalReport.Refresh();

            this.reportViewer1.RefreshReport();
        }

        private DataTable ObtenerPedido(int id)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                    SqlCommand cmd = new SqlCommand("SP_PEDIDOS_NOTAREMISION", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("PedidoId", id);
                    SqlDataAdapter dap = new SqlDataAdapter(cmd);
                    dap.Fill(dt);
                    MDIPrincipal.ActualizarBarraDeEstado();
                }
            }
            catch (SqlException ex) { Utils.MsgCatchOueclbdd(ex); }
            catch (Exception ex) { Utils.MsgCatchOue(ex); }
            return dt;
        }

        private DataTable ObtenerDetallePedidoPorOrderID(int orderID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn))
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                    string query = @"Select ProductName, od.UnitPrice, od.Quantity, od.Discount, 
                                    (od.Quantity * od.UnitPrice) * ( 1 - od.Discount ) as Total 
                                    from [Order Details] od join products p on p.ProductId = od.ProductID
                                    where OrderID = " + orderID;
                    SqlCommand cmd = new SqlCommand(query, cn);
                    SqlDataAdapter dap = new SqlDataAdapter(cmd);
                    dap.Fill(dt);
                    MDIPrincipal.ActualizarBarraDeEstado();
                }
            }
            catch (SqlException ex) { Utils.MsgCatchOueclbdd(ex); }
            catch (Exception ex) { Utils.MsgCatchOue(ex); }
            return dt;
        }
    }
}
