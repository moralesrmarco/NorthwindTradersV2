using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptNotaRemision4 : Form
    {

        public int Id;

        public FrmRptNotaRemision4()
        {
            InitializeComponent();
            // Configuramos el evento que manejará todos los subreportes
            this.reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptNotaRemision4_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmRptNotaRemision4_Load(object sender, EventArgs e)
        {
            // Crear un DataTable para el dataset de copias
            DataTable dtCopias = new DataTable("dsCopias"); // "dsCopias" es el nombre a usar en el RDLC
            dtCopias.Columns.Add("CopyType", typeof(string));

            // Agregar la fila para la copia de cliente (valor "1")
            DataRow row = dtCopias.NewRow();
            row["CopyType"] = "1";
            dtCopias.Rows.Add(row);

            // Agregar la fila para la copia de control interno (valor "2")
            row = dtCopias.NewRow();
            row["CopyType"] = "2";
            dtCopias.Rows.Add(row);

            ReportParameter[] parameters = new ReportParameter[1];
            parameters[0] = new ReportParameter("PedidoId", Id.ToString());
            this.reportViewer1.LocalReport.SetParameters(parameters);

            // Crear el ReportDataSource usando el DataTable.
            // El primer parámetro ("dsCopias") debe coincidir con el nombre del dataset en el RDLC.
            ReportDataSource dsCopias = new ReportDataSource("dsCopias", dtCopias);
            reportViewer1.LocalReport.DataSources.Add(dsCopias);

            this.reportViewer1.RefreshReport();
        }

        // Este método se ejecuta cada vez que se procesa un subreporte en el reporte maestro
        private void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                // Verificamos el nombre (ReportPath) del subreporte para asignar la fuente de datos adecuada
                if (e.ReportPath.Equals("RptNotaRemision5", StringComparison.OrdinalIgnoreCase))
                {

                    // Obtenemos los datos que se mostrarán en la nota de remisión
                    DataTable dt = ObtenerPedido(Id);
                    // "DataSet1" debe coincidir con el nombre del dataset definido en el subreporte "RptNotaRemision3"
                    e.DataSources.Add(new ReportDataSource("DataSet1", dt));
                    //subreportCallCount++;
                }
                else if (e.ReportPath.Equals("RptOrderDetailsPedPorRangoFechaPed2", StringComparison.OrdinalIgnoreCase))
                {
                    // Se espera que se haya definido un parámetro llamado "orderId" en el reporte para filtrar el detalle
                    int orderId = Convert.ToInt32(e.Parameters["OrderID"].Values[0].ToString());
                    // Obtenemos los datos detallados para la nota de remisión específica
                    DataTable dt = ObtenerDetallePedidoPorOrderID(orderId);
                    // "DataSet1" debe coincidir con el nombre del dataset definido en el subreporte anidado "RptOrderDetailsPedPorRangoFechaPed"
                    e.DataSources.Add(new ReportDataSource("DataSet1", dt));
                }
                else
                {
                    // Si no se encuentra el subreporte, puedes manejarlo como desees (por ejemplo, lanzar una excepción o registrar un error)
                    throw new Exception($"Subreporte no encontrado: {e.ReportPath}");
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
