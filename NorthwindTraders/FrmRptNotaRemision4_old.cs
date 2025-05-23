using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    public partial class FrmRptNotaRemision4_old : Form
    {

        public int Id;

        public FrmRptNotaRemision4_old()
        {
            InitializeComponent();
            // Configuramos el evento que manejará todos los subreportes
            this.reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptNotaRemision4_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmRptNotaRemision4_Load(object sender, EventArgs e)
        {
            List<DummyData> dummyList = new List<DummyData> { new DummyData { DummyValue = 1 } };

            // Limpia cualquier origen de datos que ya tenga el reporte:
            reportViewer1.LocalReport.DataSources.Clear();

            // Crea el ReportDataSource, asegurándote de usar el nombre exacto definido en el RDLC ("DataSet1")
            ReportDataSource rds = new ReportDataSource("DataSet1", dummyList);

            // Añade el ReportDataSource al reporte:
            reportViewer1.LocalReport.DataSources.Add(rds);

            //// Refresca el reporte:
            ///
            reportViewer1.RefreshReport();
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
                else if (e.ReportPath.Equals("RptOrderDetailsPedPorRangoFechaPed", StringComparison.OrdinalIgnoreCase))
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
